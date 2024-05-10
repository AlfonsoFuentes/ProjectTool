using Application.Mappers.PurchaseOrders;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.Queries
{
    public record NewPurchaseOrderGetByIdToEditCreatedQuery(Guid PurchaseOrderId) : IRequest<IResult<NewPurchaseOrderCreateEditRequest>>;
    internal class NewPurchaseOrderGetByIdToEditCreatedQueryHandler : IRequestHandler<NewPurchaseOrderGetByIdToEditCreatedQuery, IResult<NewPurchaseOrderCreateEditRequest>>
    {
        private IQueryRepository Repository { get; set; }

        public NewPurchaseOrderGetByIdToEditCreatedQueryHandler(IQueryRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<NewPurchaseOrderCreateEditRequest>> Handle(NewPurchaseOrderGetByIdToEditCreatedQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetPurchaseOrderByIdCreatedAsync(request.PurchaseOrderId);
            
            if (row == null)
            {
                return Result<NewPurchaseOrderCreateEditRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            var budgetItem = await Repository.GetBudgetItemMWOApprovedAsync(row.MainBudgetItemId);
            if (budgetItem == null)
                return Result<NewPurchaseOrderCreateEditRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));

            var result = row.ToPurchaseOrderCreateEditRequest(budgetItem);

            return Result<NewPurchaseOrderCreateEditRequest>.Success(result);
        }
    }
}
