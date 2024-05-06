using Application.Mappers.BudgetItems;
using Azure.Core;
using MediatR;

namespace Application.NewFeatures.BudgetItems.Commands
{
    public record NewBudgetItemUpdateCommand(NewBudgetItemMWOUpdateRequest Data) : IRequest<IResult>;
    internal class NewBudgetItemUpdateCommandHandler : IRequestHandler<NewBudgetItemUpdateCommand, IResult>
    {
        private IAppDbContext _appDbContext;
        private IRepository Repository { get; set; }

        public NewBudgetItemUpdateCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            _appDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewBudgetItemUpdateCommand request, CancellationToken cancellationToken)
        {
            var budgetitem = await Repository.GetBudgetItemToUpdate(request.Data.BudgetItemId);

            if (budgetitem == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.NotFound, ClassNames.BudgetItems));
            }

            request.Data.FromBudgetItemUpdateRequest(budgetitem);
            if (request.Data.IsTaxesData && !request.Data.IsMainItemTaxesNoProductive)
            {
                await UpdateTaxesItem(budgetitem, request.Data);
            }


            await Repository.UpdateAsync(budgetitem);
            var result = await _appDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, $"{Cache.GetMWOByCreated}:{request.Data.MWOId}");
            var mwo = await Repository.GetMWOById(request.Data.MWOId);
            if (mwo == null) return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.MWOName, ResponseType.NotFound, ClassNames.MWO));
            await Repository.UpdateTaxesAndEgineeringItems(mwo,
               request.Data.MustUpdateTaxesNotProductive,
               request.Data.MustUpdateEngineeringItems,
               cancellationToken);


            return result > 0 ?
                Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Created, ClassNames.Brand)) :
                Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Created, ClassNames.Brand));
        }
        async Task UpdateTaxesItem(BudgetItem budgetitem, NewBudgetItemMWOUpdateRequest Data)
        {
            foreach (var taxitem in budgetitem.TaxesItems)
            {
                if (!Data.TaxesSelectedItems.Any(x => x.BudgetItemId == taxitem.SelectedId))
                {
                    await Repository.RemoveAsync(taxitem);
                }
            }
            foreach (var itemdto in Data.TaxesSelectedItems)
            {

                if (!budgetitem.TaxesItems.Any(x => x.SelectedId == itemdto.BudgetItemId))
                {
                    var taxItem = budgetitem.AddTaxItem(itemdto.BudgetItemId);
                    await Repository.AddAsync(taxItem);
                }


            }
        }

    }
}
