using Application.Mappers.PurchaseOrders;
using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.Commands
{
    public record NewPurchaseOrderApproveCommand(NewPurchaseOrderApproveRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderApproveCommandHandler : IRequestHandler<NewPurchaseOrderApproveCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderApproveCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderApproveCommand request, CancellationToken cancellationToken)
        {
            var purchaseorder = await Repository.GetByIdAsync<PurchaseOrder>(request.Data.PurchaseOrderId);
            if (purchaseorder == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            purchaseorder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Approved.Id;
            purchaseorder.PONumber = request.Data.PurchaseOrderNumber;
            purchaseorder.POExpectedDateDate = request.Data.ExpectedDate!.Value;
            purchaseorder.USDCOP = request.Data.USDCOP;
            purchaseorder.USDEUR = request.Data.USDEUR;
            //Falta cuando la orden es de alteraciones o es no productiva


            await Repository.UpdateAsync(purchaseorder);

            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken,
                Cache.GetParamsCachePurchaseOrderCreated(purchaseorder.MWOId, request.Data.PurchaseOrderItems));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseorderName, ResponseType.Approve, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.Approve, ClassNames.PurchaseOrders));
        }
    }
}
