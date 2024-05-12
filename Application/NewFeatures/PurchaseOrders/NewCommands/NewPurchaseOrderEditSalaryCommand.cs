

namespace Application.NewFeatures.PurchaseOrders.NewCommands
{
    public record NewPurchaseOrderEditSalaryCommand(NewPurchaseOrderEditSalaryRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderEditSalaryCommandHandler : IRequestHandler<NewPurchaseOrderEditSalaryCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderEditSalaryCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderEditSalaryCommand request, CancellationToken cancellationToken)
        {
            var purchaseorder = await Repository.GetPurchaseOrderByIdCreatedAsync(request.Data.PurchaseOrder.PurchaseOrderId);
            if (purchaseorder == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            purchaseorder.PONumber = request.Data.PurchaseOrder.PurchaseOrderNumber;
            purchaseorder.PurchaseorderName = request.Data.PurchaseOrder.PurchaseOrderName;
     
            foreach (var item in request.Data.PurchaseOrder.PurchaseOrderItems)
            {
                var purchaseorderitem = await Repository.GetByIdAsync<PurchaseOrderItem>(item.PurchaseOrderItemId);
                if(purchaseorderitem != null)
                {
                    purchaseorderitem.UnitaryValueCurrency=item.UnitaryValuePurchaseOrderCurrency;
                    await Repository.UpdateAsync(purchaseorderitem);
                }
                foreach (var receiveditem in item.PurchaseOrderReceiveds)
                {
                    var received = await Repository.GetByIdAsync<PurchaseOrderItemReceived>(receiveditem.PurchaseOrderItemReceivedId);
                    if (received != null)
                    {
                        received.USDCOP = receiveditem.USDCOP;
                        received.USDEUR = receiveditem.USDEUR;
                        received.ValueReceivedCurrency = receiveditem.ValueReceivedCurrency;
                        await Repository.UpdateAsync(received);
                    }
                }

            }


            await Repository.UpdateAsync(purchaseorder);
            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCachePurchaseOrder(purchaseorder));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.Created, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.Created, ClassNames.PurchaseOrders));
        }
    }
}
