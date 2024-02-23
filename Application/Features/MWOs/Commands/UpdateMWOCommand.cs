using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;

namespace Application.Features.MWOs.Commands
{
    public record UpdateMWOCommand(UpdateMWORequest Data) : IRequest<IResult>;
    public class UpdateMWOCommandHandler : IRequestHandler<UpdateMWOCommand, IResult>
    {
        private IMWORepository Repository { get; set; }

        private IAppDbContext AppDbContext;
        private IBudgetItemRepository RepositoryBudgetItem { get; set; }
        public UpdateMWOCommandHandler(IMWORepository repository, IAppDbContext appDbContext, IBudgetItemRepository repositoryBudgetItem)
        {
            Repository = repository;

            AppDbContext = appDbContext;
            RepositoryBudgetItem = repositoryBudgetItem;
        }

        public async Task<IResult> Handle(UpdateMWOCommand request, CancellationToken cancellationToken)
        {
            var mwo = await AppDbContext.MWOs.SingleOrDefaultAsync(x => x.Id == request.Data.Id);
            if (mwo == null)
            {
                return Result.Fail($"{request.Data.Name} was not found.");
            }
            mwo.Name = request.Data.Name;
            mwo.Type = request.Data.Type.Id;
            mwo.PercentageContingency = request.Data.PercentageContingency;
            mwo.PercentageEngineering = request.Data.PercentageEngineering;
            mwo.PercentageAssetNoProductive = request.Data.PercentageAssetNoProductive;

            if (mwo.IsAssetProductive && !request.Data.IsAssetProductive)
            {
                mwo.IsAssetProductive = false;
            

                await CreateTaxesForNoProductive(mwo, request.Data);

            }
            else if (!mwo.IsAssetProductive && request.Data.IsAssetProductive)
            {
                mwo.IsAssetProductive = true;
                var taxMainItem = await RepositoryBudgetItem.GetMainBudgetTaxItemByMWO(request.Data.Id);
              
                AppDbContext.BudgetItems.Remove(taxMainItem);
            }


            await Repository.UpdateMWO(mwo!);
            var result = await AppDbContext.SaveChangesAsync(cancellationToken);
            await RepositoryBudgetItem.UpdateTaxesAndEngineeringContingencyItems(mwo.Id, cancellationToken);

            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} updated succesfully");
            }

            return Result.Fail($"{request.Data.Name} was not updated succesfully");
        }
        async Task CreateTaxesForNoProductive(MWO mwo, UpdateMWORequest Data)
        {
            var taxitem = mwo.AddBudgetItem(BudgetItemTypeEnum.Taxes.Id);

            taxitem.IsNotAbleToEditDelete = true;
            taxitem.Name = $"Tax non productive";
            taxitem.Percentage = Data.PercentageAssetNoProductive;
            taxitem.IsMainItemTaxesNoProductive = true;
            await RepositoryBudgetItem.AddBudgetItem(taxitem);
            var currentBudgetItemListToApplyTaxes = await RepositoryBudgetItem.GetItemToApplyTaxes(Data.Id);

            foreach (var itemdto in currentBudgetItemListToApplyTaxes)
            {

                var taxItem = taxitem.AddTaxItem(itemdto.Id);
                await RepositoryBudgetItem.AddTaxSelectedItem(taxItem);
            }

        }
       
    }
}
