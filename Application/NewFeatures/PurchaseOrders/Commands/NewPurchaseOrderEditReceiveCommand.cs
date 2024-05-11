using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.Commands
{
    public record NewPurchaseOrderEditReceiveCommand(OldPurchaseOrderEditReceiveRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderEditReceiveCommandHandler : IRequestHandler<NewPurchaseOrderEditReceiveCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderEditReceiveCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderEditReceiveCommand request, CancellationToken cancellationToken)
        {
            var purchaseorder = await Repository.GetPurchaseOrderByIdCreatedAsync(request.Data.PurchaseOrderId);
            if (purchaseorder == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            purchaseorder.PurchaseOrderStatus = request.Data.IsCompletedReceived ? PurchaseOrderStatusEnum.Closed.Id : PurchaseOrderStatusEnum.Receiving.Id;
            purchaseorder.POClosedDate = request.Data.IsCompletedReceived ? DateTime.UtcNow : null;
            purchaseorder.POClosedDate = DateTime.UtcNow;
            foreach (var row in request.Data.PurchaseOrderItems)
            {
                foreach(var rowreceived in row.Receiveds)
                {
                    var received = await Repository.GetByIdAsync<PurchaseOrderItemReceived>(rowreceived.ReceivedId);
                    if (received!=null)
                    {
                        received.CurrencyDate = DateTime.UtcNow;
                        
                        received.USDEUR = rowreceived.USDEUR;
                        received.USDCOP = rowreceived.USDCOP;
                        received.ValueReceivedCurrency = rowreceived.ReceivedCurrency;
                        await Repository.UpdateAsync(received);
                    }
                    
                }
                
            }
            await Repository.UpdateAsync(purchaseorder);

            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken,Cache.GetParamsCachePurchaseOrder(purchaseorder));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseorderName, ResponseType.Received, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.Received, ClassNames.PurchaseOrders));
        }
    }
}
