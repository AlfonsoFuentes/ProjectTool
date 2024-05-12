

namespace Application.NewFeatures.BudgetItems.Commands
{
    public record NewBudgetItemCreateCommand(NewBudgetItemMWOCreatedRequest Data) : IRequest<IResult>;

    internal class NewBudgetItemCreateCommandHandler : IRequestHandler<NewBudgetItemCreateCommand, IResult>
    {
        private IAppDbContext _appDbContext;
        private IRepository Repository { get; set; }

        public NewBudgetItemCreateCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            _appDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewBudgetItemCreateCommand request, CancellationToken cancellationToken)
        {
            var mwo = await Repository.GetMWOById(request.Data.MWOId);
            if (mwo == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.MWO));
            }
            var budgetitem = mwo.AddBudgetItem(request.Data.Type.Id);
            if (request.Data.IsTaxesData)
            {
                await CreatedItemsTaxesData(budgetitem, request.Data);
            }

            request.Data.FromBudgetItemCreateRequest(budgetitem);

            await Repository.AddAsync(budgetitem);
            var result = await _appDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken,$"{Cache.GetMWOByCreated}:{request.Data.MWOId}");
            await Repository.UpdateTaxesAndEgineeringItems(mwo,
                request.Data.MustUpdateTaxesNotProductive,
                request.Data.MustUpdateEngineeringItems,
                cancellationToken);


            return result > 0 ?
                Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.Name, ResponseType.Created, ClassNames.Brand)) :
                Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.Name, ResponseType.Created, ClassNames.Brand));
        }

        async Task CreatedItemsTaxesData(BudgetItem item, NewBudgetItemMWOCreatedRequest Data)
        {
            foreach (var itemdto in Data.TaxesSelectedItems)
            {

                var taxItem = item.AddTaxItem(itemdto.BudgetItemId);
                await Repository.AddAsync(taxItem);
            }
        }
    }
}
