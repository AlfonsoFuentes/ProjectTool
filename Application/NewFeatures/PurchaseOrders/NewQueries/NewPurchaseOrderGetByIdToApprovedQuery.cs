using Application.Mappers.PurchaseOrders.NewMappers;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.NewQueries
{
    public record NewPurchaseOrderGetByIdToApprovedQuery(Guid PurchaseOrderId) : IRequest<IResult<NewPurchaseOrderApproveRequest>>;
    internal class NewPurchaseOrderGetByIdToApprovedQueryHandler : IRequestHandler<NewPurchaseOrderGetByIdToApprovedQuery, IResult<NewPurchaseOrderApproveRequest>>
    {
        private IQueryRepository Repository { get; set; }

        public NewPurchaseOrderGetByIdToApprovedQueryHandler(IQueryRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<NewPurchaseOrderApproveRequest>> Handle(NewPurchaseOrderGetByIdToApprovedQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetPurchaseOrderByIdCreatedAsync(request.PurchaseOrderId);

            if (row == null)
            {
                return Result<NewPurchaseOrderApproveRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            var budgetItem = await Repository.GetBudgetItemMWOApprovedAsync(row.MainBudgetItemId);
            if (budgetItem == null)
                return Result<NewPurchaseOrderApproveRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));

            var result = row.ToPurchaseOrderApproveRequest(budgetItem);

            return Result<NewPurchaseOrderApproveRequest>.Success(result!);
        }
    }
}
