using Application.Interfaces;

namespace Application.NewFeatures.BudgetItems.Commands
{
    public record NewBudgetItemDeleteCommand(NewBudgetItemMWOCreatedResponse Data) : IRequest<IResult>;
    internal class NewBudgetItemDeleteCommandHandler : IRequestHandler<NewBudgetItemDeleteCommand, IResult>
    {
        private readonly IRepository Repository;
        private readonly IAppDbContext appDbContext;

        public NewBudgetItemDeleteCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            this.Repository = repository;

            this.appDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewBudgetItemDeleteCommand request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetBudgetItemToUpdate(request.Data.BudgetItemId);
            if (row == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Delete, ClassNames.BudgetItems));
            }
            var taxesitems = await Repository.GetTaxesItemsToDeleteBudgetItem(request.Data.BudgetItemId);
            if (taxesitems.Count > 0)
            {
                await Repository.RemoveRangeAsync(taxesitems);
            }

            await Repository.RemoveAsync(row);

            var result = await appDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, $"{Cache.GetMWOByCreated}:{request.Data.MWOId}");

            if (taxesitems.Count > 0)
            {
                await UpdateTaxesRelatedBudgetItem(taxesitems);

                await appDbContext.SaveChangesAsync(cancellationToken);
            }

            var mwo = await Repository.GetMWOById(request.Data.MWOId);

            if (mwo == null) return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.MWOName, ResponseType.NotFound, ClassNames.MWO));

            await Repository.UpdateTaxesAndEgineeringItems(mwo,
               request.Data.MustUpdateTaxesNotProductive,
               request.Data.MustUpdateEngineeringItems,
               cancellationToken);

            return result > 0 ?
               Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Delete, ClassNames.BudgetItems)) :
               Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Delete, ClassNames.BudgetItems));
        }
        async Task UpdateTaxesRelatedBudgetItem(List<TaxesItem> taxesitems)
        {
            foreach (var item in taxesitems)
            {
                BudgetItem? budgetitem = await Repository.GetBudgetItemToUpdateTaxes(item.BudgetItemId);

                var totaltaxesBudget = budgetitem!.TaxesItems.Sum(x => x.Selected.Budget);

                budgetitem.UnitaryCost = totaltaxesBudget * budgetitem.Percentage / 100;

                await Repository.UpdateAsync(budgetitem);
            }



        }

    }
}
