using Application.Features.MWOs.Commands;
using Application.Interfaces;
using Shared.Enums.MWOStatus;
using Shared.Models.MWO;

namespace Application.NewFeatures.MWOS.Commands
{
    public record NewMWOApproveCommand(NewMWOApproveRequest Data) : IRequest<IResult>;
    public class NewMWOApproveCommandHandler : IRequestHandler<NewMWOApproveCommand, IResult>
    {
        private IRepository Repository { get; set; }
        private IAppDbContext AppDbContext;
        public NewMWOApproveCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;

            AppDbContext = appDbContext;

        }

        public async Task<IResult> Handle(NewMWOApproveCommand request, CancellationToken cancellationToken)
        {

            var mwo = await Repository.GetMWOById(request.Data.MWOId);
            if (mwo == null)
            {
                return Result.Fail($"{request.Data.Name} was not found.");
            }
            mwo.Status = MWOStatusEnum.Approved.Id;
            mwo.MWONumber = request.Data.MWONumber;
            mwo.CostCenter = request.Data.CostCenter.Id;
            mwo.PercentageTaxForAlterations = request.Data.PercentageTaxForAlterations;

            mwo.ApprovedDate = DateTime.Now;

            if (mwo.IsAssetProductive && !request.Data.IsAssetProductive)
            {
                mwo.IsAssetProductive = false;

                await CreateTaxesForNoProductive(mwo, request.Data);

            }
            else if (!mwo.IsAssetProductive && request.Data.IsAssetProductive)
            {
                mwo.IsAssetProductive = true;
                var taxMainItem = mwo.ItemTaxNoProductive;
                await Repository.RemoveAsync(taxMainItem!);

            }
            var SalaryItem = mwo.ItemCapitalizedSalary;
            if (SalaryItem != null)
            {
                SalaryItem.Percentage = request.Data.PercentageEngineering;
                await Repository.UpdateAsync(SalaryItem);
            }
            var Contingency = mwo.ItemContingency;
            if (Contingency != null)
            {
                Contingency.Percentage = request.Data.PercentageContingency;
                await Repository.UpdateAsync(Contingency);
            }
            await Repository.UpdateAsync(mwo!);
   
            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCacheMWO(request.Data.MWOId));

            await Repository.UpdateTaxesAndEgineeringItems(mwo, !request.Data.IsAssetProductive, true, cancellationToken);


            return result > 0 ?
               Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Approve, ClassNames.MWO)) :
               Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Approve, ClassNames.MWO));
        }
        async Task CreateTaxesForNoProductive(MWO mwo, NewMWOApproveRequest Data)
        {
            var taxitem = mwo.AddBudgetItem(BudgetItemTypeEnum.Taxes.Id);

            taxitem.IsNotAbleToEditDelete = true;
            taxitem.Name = $"Tax non productive";
            taxitem.Percentage = Data.PercentageAssetNoProductive;
            taxitem.IsMainItemTaxesNoProductive = true;
            var budget = mwo.CapitalForTaxesCalculationsUSD;
            taxitem.UnitaryCost = budget * Data.PercentageAssetNoProductive / 100;
            taxitem.Quantity = 1;
            await Repository.AddAsync(taxitem);


        }
    }
}
