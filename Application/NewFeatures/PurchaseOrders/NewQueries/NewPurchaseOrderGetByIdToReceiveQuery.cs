using Application.Mappers.PurchaseOrders.NewMappers;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.NewQueries
{
    public record NewPurchaseOrderGetByIdToReceiveQuery(Guid PurchaseOrderId) : IRequest<IResult<NewPurchaseOrderReceiveRequest>>;
    internal class NewPurchaseOrderGetByIdToReceiveQueryHandler : IRequestHandler<NewPurchaseOrderGetByIdToReceiveQuery, IResult<NewPurchaseOrderReceiveRequest>>
    {
        private IQueryRepository Repository { get; set; }

        public NewPurchaseOrderGetByIdToReceiveQueryHandler(IQueryRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<NewPurchaseOrderReceiveRequest>> Handle(NewPurchaseOrderGetByIdToReceiveQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetPurchaseOrderByIdToReceiveAsync(request.PurchaseOrderId);

            if (row == null)
            {
                return Result<NewPurchaseOrderReceiveRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            var budgetItem = await Repository.GetBudgetItemMWOApprovedAsync(row.MainBudgetItemId);
            if (budgetItem == null)
                return Result<NewPurchaseOrderReceiveRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));

            var result = row.ToPurchaseOrderReceiveRequest(budgetItem);

            return Result<NewPurchaseOrderReceiveRequest>.Success(result!);
        }
    }
}
