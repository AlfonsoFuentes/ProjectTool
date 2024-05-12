using Application.NewFeatures.PurchaseOrders.Queries;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.NewQueries
{
    public record NewPurchaseOrderGetByIdToEditCreatedQuery(Guid PurchaseOrderId) : IRequest<IResult<NewPurchaseOrderEditCreateRequest>>;
    internal class NewPurchaseOrderGetByIdToEditCreatedQueryHandler : IRequestHandler<NewPurchaseOrderGetByIdToEditCreatedQuery, IResult<NewPurchaseOrderEditCreateRequest>>
    {
        private IQueryRepository Repository { get; set; }

        public NewPurchaseOrderGetByIdToEditCreatedQueryHandler(IQueryRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<NewPurchaseOrderEditCreateRequest>> Handle(NewPurchaseOrderGetByIdToEditCreatedQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetPurchaseOrderByIdCreatedAsync(request.PurchaseOrderId);

            if (row == null)
            {
                return Result<NewPurchaseOrderEditCreateRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            var budgetItem = await Repository.GetBudgetItemMWOApprovedAsync(row.MainBudgetItemId);
            if (budgetItem == null)
                return Result<NewPurchaseOrderEditCreateRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));

            var result = row.ToPurchaseOrderEditCreatedRequest(budgetItem);

            return Result<NewPurchaseOrderEditCreateRequest>.Success(result!);
        }
    }
}
