using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.Commands
{
    public record NewPurchaseOrderUnApproveCommand(NewPurchaseOrderUnApproveRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderUnApproveCommandHandler : IRequestHandler<NewPurchaseOrderUnApproveCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderUnApproveCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderUnApproveCommand request, CancellationToken cancellationToken)
        {
            var purchaseorder = await Repository.GetByIdAsync<PurchaseOrder>(request.Data.PurchaseOrderId);
            if (purchaseorder == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Created.Id;
            purchaseorder.PONumber = request.Data.PurchaseOrderNumber;
            purchaseorder.POExpectedDateDate = null;
            //Falta cuando la orden es de alteraciones o es no productiva


            await Repository.UpdateAsync(purchaseorder);

            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken,
                Cache.GetParamsCachePurchaseOrderCreated(purchaseorder.MWOId));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseorderName, ResponseType.UnApprove, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.UnApprove, ClassNames.PurchaseOrders));
        }
    }
}
