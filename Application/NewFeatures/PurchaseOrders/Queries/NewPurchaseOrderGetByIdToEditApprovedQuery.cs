using Application.Mappers.PurchaseOrders;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.Queries
{
    public record NewPurchaseOrderGetByIdToEditApprovedQuery(Guid PurchaseOrderId) : IRequest<IResult<NewPurchaseOrderEditApprovedRequest>>;
    internal class NewPurchaseOrderGetByIdToEditApprovedQueryHandler : IRequestHandler<NewPurchaseOrderGetByIdToEditApprovedQuery, IResult<NewPurchaseOrderEditApprovedRequest>>
    {
        private IQueryRepository Repository { get; set; }

        public NewPurchaseOrderGetByIdToEditApprovedQueryHandler(IQueryRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<NewPurchaseOrderEditApprovedRequest>> Handle(NewPurchaseOrderGetByIdToEditApprovedQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetPurchaseOrderByIdToReceiveAsync(request.PurchaseOrderId);
            if (row == null)
            {
                return Result<NewPurchaseOrderEditApprovedRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));
            }

            var result = row.ToPurchaseOrderEditApprovedRequest();

            return Result<NewPurchaseOrderEditApprovedRequest>.Success(result);
        }
    }
}
