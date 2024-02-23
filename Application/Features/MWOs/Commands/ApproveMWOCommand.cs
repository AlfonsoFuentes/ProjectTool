using Application.Features.MWOs.Validators;
using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;
using Shared.Models.MWOStatus;

namespace Application.Features.MWOs.Commands
{
    public record ApproveMWOCommand(ApproveMWORequest Data) : IRequest<IResult>;
    public class ApproveMWOCommandHandler : IRequestHandler<ApproveMWOCommand, IResult>
    {
        private IMWORepository Repository { get; set; }
        private IBudgetItemRepository RepositoryBudgetItem { get; set; }
        private IAppDbContext AppDbContext;
        public ApproveMWOCommandHandler(IMWORepository repository, IAppDbContext appDbContext, IBudgetItemRepository repositoryBudgetItem)
        {
            Repository = repository;

            AppDbContext = appDbContext;
            RepositoryBudgetItem = repositoryBudgetItem;
        }

        public async Task<IResult> Handle(ApproveMWOCommand request, CancellationToken cancellationToken)
        {
            var validator = new ApproveMWOValidator(Repository);
            var validationresult = await validator.ValidateAsync(request.Data);
            if (!validationresult.IsValid)
            {
                return Result.Fail(validationresult.Errors.Select(x => x.ErrorMessage).ToArray());
            }
            var mwo = await Repository.GetMWOById(request.Data.Id);
            if (mwo == null)
            {
                return Result.Fail($"{request.Data.Name} was not found.");
            }
            mwo.Status = MWOStatusEnum.Approved.Id;
            mwo.MWONumber = request.Data.MWONumber;
            mwo.CostCenter = request.Data.CostCenter.Id;
    
            mwo.PercentageContingency = request.Data.PercentageContingency;
            mwo.PercentageEngineering = request.Data.PercentageEngineering;
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
                return Result.Success($"{request.Data.Name} approved succesfully");
            }

            return Result.Fail($"{request.Data.Name} was not approved succesfully");
        }
        async Task CreateTaxesForNoProductive(MWO mwo, ApproveMWORequest Data)
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
