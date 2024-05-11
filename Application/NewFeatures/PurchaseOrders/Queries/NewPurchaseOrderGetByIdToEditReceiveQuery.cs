using Application.Mappers.PurchaseOrders;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.Queries
{
    public record OldPurchaseOrderGetByIdToEditReceiveQuery(Guid PurchaseOrderId) : IRequest<IResult<OldPurchaseOrderEditReceiveRequest>>;
    internal class NewPurchaseOrderGetByIdToEditReceiveQueryHandler : IRequestHandler<OldPurchaseOrderGetByIdToEditReceiveQuery, IResult<OldPurchaseOrderEditReceiveRequest>>
    {
        private IQueryRepository Repository { get; set; }

        public NewPurchaseOrderGetByIdToEditReceiveQueryHandler(IQueryRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<OldPurchaseOrderEditReceiveRequest>> Handle(OldPurchaseOrderGetByIdToEditReceiveQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetPurchaseOrderByIdToReceiveAsync(request.PurchaseOrderId);
            if (row == null)
            {
                return Result<OldPurchaseOrderEditReceiveRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));
            }

            var result = row.ToPurchaseOrderEditReceiveRequest();

            return Result<OldPurchaseOrderEditReceiveRequest>.Success(result);
        }
    }
}
