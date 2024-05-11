using Application.Mappers.PurchaseOrders.NewMappers;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.NewQueries
{
    public record NewPurchaseOrderGetByIdToEditApprovedQuery(Guid PurchaseOrderId) : IRequest<IResult<NewPurchaseOrderEditApproveRequest>>;
    internal class NewPurchaseOrderGetByIdToEditApprovedQueryHandler : IRequestHandler<NewPurchaseOrderGetByIdToEditApprovedQuery, IResult<NewPurchaseOrderEditApproveRequest>>
    {
        private IQueryRepository Repository { get; set; }

        public NewPurchaseOrderGetByIdToEditApprovedQueryHandler(IQueryRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<NewPurchaseOrderEditApproveRequest>> Handle(NewPurchaseOrderGetByIdToEditApprovedQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetPurchaseOrderByIdToReceiveAsync(request.PurchaseOrderId);

            if (row == null)
            {
                return Result<NewPurchaseOrderEditApproveRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            var budgetItem = await Repository.GetBudgetItemMWOApprovedAsync(row.MainBudgetItemId);
            if (budgetItem == null)
                return Result<NewPurchaseOrderEditApproveRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));

            var result = row.ToPurchaseOrderEditApprovedRequest(budgetItem);

            return Result<NewPurchaseOrderEditApproveRequest>.Success(result!);
        }
    }
}
