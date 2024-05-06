using Application.Interfaces;

namespace Application.NewFeatures.MWOS.Commands
{
    public record NewMWOUpdateCommand(NewMWOUpdateRequest Data) : IRequest<IResult>;
    internal class NewMWOUpdateCommandHandler : IRequestHandler<NewMWOUpdateCommand, IResult>
    {
        IAppDbContext appDbContext;
        IRepository repository;

        public NewMWOUpdateCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            this.repository = repository;
            this.appDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewMWOUpdateCommand request, CancellationToken cancellationToken)
        {
            var mwo = await repository.GetMWOById(request.Data.MWOId);
            if (mwo == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.NotFound, ClassNames.MWO));
            }
            request.Data.FromMWOUpdateRequest(mwo);
            if (mwo.IsAssetProductive && !request.Data.IsAssetProductive)
            {
                mwo.IsAssetProductive = false;

                await CreateTaxesForNoProductive(mwo, request.Data);

            }
            else if (!mwo.IsAssetProductive && request.Data.IsAssetProductive)
            {
                mwo.IsAssetProductive = true;
                var taxMainItem = mwo.ItemTaxNoProductive;
                await repository.RemoveAsync(taxMainItem!);

            }
            var SalaryItem = mwo.ItemCapitalizedSalary;
            if (SalaryItem != null)
            {
                SalaryItem.Percentage = request.Data.PercentageEngineering;
                await repository.UpdateAsync(SalaryItem);
            }
            var Contingency = mwo.ItemContingency;
            if (Contingency != null)
            {
                Contingency.Percentage = request.Data.PercentageContingency;
                await repository.UpdateAsync(Contingency);
            }

           await repository.UpdateAsync(mwo);
            var result = await appDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCacheMWO(request.Data.MWOId));
            await repository.UpdateTaxesAndEgineeringItems(mwo,
               !mwo.IsAssetProductive,
               true,
               cancellationToken);


            return result > 0 ?
               Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Updated, ClassNames.MWO)) :
               Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Updated, ClassNames.MWO));

        }
        async Task CreateTaxesForNoProductive(MWO mwo, NewMWOUpdateRequest Data)
        {
            var taxitem = mwo.AddBudgetItem(BudgetItemTypeEnum.Taxes.Id);

            taxitem.IsNotAbleToEditDelete = true;
            taxitem.Name = $"Tax non productive";
            taxitem.Percentage = Data.PercentageAssetNoProductive;
            taxitem.IsMainItemTaxesNoProductive = true;
            var budget = mwo.CapitalForTaxesCalculationsUSD;
            taxitem.UnitaryCost = budget * Data.PercentageAssetNoProductive / 100;
            taxitem.Quantity = 1;
            await repository.AddAsync(taxitem);


        }

    }
}
