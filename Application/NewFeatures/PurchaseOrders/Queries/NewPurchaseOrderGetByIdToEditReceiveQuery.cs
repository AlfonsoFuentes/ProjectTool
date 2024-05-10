using Application.Mappers.PurchaseOrders;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.Queries
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

            var result = row.ToPurchaseOrderEditReceiveRequest();

            return Result<NewPurchaseOrderEditReceiveRequest>.Success(result);
        }
    }
}
