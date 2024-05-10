using Application.Mappers.PurchaseOrders;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.Queries
{
    public record NewPurchaseOrderGetByIdToEditSalaryQuery(Guid PurchaseOrderId) : IRequest<IResult<NewPurchaseOrderEditSalaryRequest>>;
    internal class NewPurchaseOrderGetByIdToEditSalaryQueryHandler : IRequestHandler<NewPurchaseOrderGetByIdToEditSalaryQuery, IResult<NewPurchaseOrderEditSalaryRequest>>
    {
        private IQueryRepository Repository { get; set; }

        public NewPurchaseOrderGetByIdToEditSalaryQueryHandler(IQueryRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<NewPurchaseOrderEditSalaryRequest>> Handle(NewPurchaseOrderGetByIdToEditSalaryQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetPurchaseOrderByIdToReceiveAsync(request.PurchaseOrderId);
       
            if (row == null)
            {
                return Result<NewPurchaseOrderEditSalaryRequest>.Fail(ResponseMessages.ReponseFailMessage("", ResponseType.NotFound, ClassNames.PurchaseOrders));
            }

            var result = row.ToPurchaseOrderEditSalaryRequest();

            return Result<NewPurchaseOrderEditSalaryRequest>.Success(result);
        }
    }
}
