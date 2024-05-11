using Application.Mappers.PurchaseOrders.NewMappers;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.NewQueries
{
    public record NewPurchaseOrderGetByIdToEditReceiveQuery(Guid PurchaseOrderId) : IRequest<IResult<NewPurchaseOrderEditReceiveRequest>>;
    internal class NewPurchaseOrderGetByIdToEditReceiveQueryHandler : IRequestHandler<NewPurchaseOrderGetByIdToEditReceiveQuery, IResult<NewPurchaseOrderEditReceiveRequest>>
    {
        private IQueryRepository Repository { get; set; }

        public NewPurchaseOrderGetByIdToEditReceiveQueryHandler(IQueryRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<NewPurchaseOrderEditReceiveRequest>> Handle(NewPurchaseOrderGetByIdToEditReceiveQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetPurchaseOrderByIdToReceiveAsync(request.PurchaseOrderId);

            if (row == null)
            {
                return Result<NewPurchaseOrderEditReceiveRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            var budgetItem = await Repository.GetBudgetItemMWOApprovedAsync(row.MainBudgetItemId);
            if (budgetItem == null)
                return Result<NewPurchaseOrderEditReceiveRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));

            var result = row.ToPurchaseOrderEditReceiveRequest(budgetItem);

            return Result<NewPurchaseOrderEditReceiveRequest>.Success(result!);
        }
    }
}
