using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.Commands
{
    public record NewPurchaseOrderReceiveCommand(OldPurchaseOrderReceiveRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderReceiveCommandHandler : IRequestHandler<NewPurchaseOrderReceiveCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderReceiveCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderReceiveCommand request, CancellationToken cancellationToken)
        {
            var purchaseorder = await Repository.GetPurchaseOrderByIdCreatedAsync(request.Data.PurchaseOrderId);
            if (purchaseorder == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            purchaseorder.PurchaseOrderStatus = request.Data.IsCompletedReceived ? PurchaseOrderStatusEnum.Closed.Id : PurchaseOrderStatusEnum.Receiving.Id;
            purchaseorder.POClosedDate = request.Data.IsCompletedReceived ? DateTime.UtcNow : null;

            foreach (var row in request.Data.PurchaseOrderItems)
            {
                var purchaseorderitem = await Repository.GetByIdAsync<PurchaseOrderItem>(row.PurchaseOrderItemId);
                if (purchaseorderitem != null)
                {
                    var received = purchaseorderitem.AddPurchaseOrderReceived();
                    received.CurrencyDate = DateTime.UtcNow;
                   
                    received.USDEUR = row.ReceiveUSDEUR;
                    received.USDCOP = row.ReceiveUSDCOP;
                    received.ValueReceivedCurrency = row.ReceivingCurrency;
                    await Repository.AddAsync(received);
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
