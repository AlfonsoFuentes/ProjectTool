using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.NewCommands
{
    public record NewPurchaseOrderEditReceivingCommand(NewPurchaseOrderEditReceiveRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderEditReceivingCommandHandler : IRequestHandler<NewPurchaseOrderEditReceivingCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderEditReceivingCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderEditReceivingCommand request, CancellationToken cancellationToken)
        {
            var purchaseorder = await Repository.GetPurchaseOrderByIdCreatedAsync(request.Data.PurchaseOrder.PurchaseOrderId);
            if (purchaseorder == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            purchaseorder.PurchaseOrderStatus = request.Data.PurchaseOrder.IsCompletedReceived ? PurchaseOrderStatusEnum.Closed.Id : PurchaseOrderStatusEnum.Receiving.Id;
            purchaseorder.POClosedDate = request.Data.PurchaseOrder.IsCompletedReceived ? DateTime.UtcNow : null;

            foreach (var item in request.Data.PurchaseOrder.PurchaseOrderItems)
            {
                foreach(var receiveditem in item.PurchaseOrderReceiveds)
                {
                    var received=await Repository.GetByIdAsync<PurchaseOrderItemReceived>(receiveditem.PurchaseOrderItemReceivedId); 
                    if (received != null)
                    {
                        received.USDCOP=receiveditem.USDCOP;
                        received.USDEUR=receiveditem.USDEUR;
                        received.ValueReceivedCurrency=receiveditem.ValueReceivedCurrency;
                        await Repository.UpdateAsync(received);
                    }
                }
                
            }


            await Repository.UpdateAsync(purchaseorder);

            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCachePurchaseOrder(purchaseorder));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.Received, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.Received, ClassNames.PurchaseOrders));
        }

    }
}
