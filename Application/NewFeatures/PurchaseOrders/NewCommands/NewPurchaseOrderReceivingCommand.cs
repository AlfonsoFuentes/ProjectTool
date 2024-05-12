namespace Application.NewFeatures.PurchaseOrders.NewCommands
{
    public record NewPurchaseOrderReceivingCommand(NewPurchaseOrderReceiveRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderReceivingCommandHandler : IRequestHandler<NewPurchaseOrderReceivingCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderReceivingCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderReceivingCommand request, CancellationToken cancellationToken)
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
                var purchaseorderitem = await Repository.GetByIdAsync<PurchaseOrderItem>(item.PurchaseOrderItemId);
                if (purchaseorderitem != null)
                {
                    var NewReceived = purchaseorderitem.AddPurchaseOrderReceived();
                    NewReceived.CurrencyDate = DateTime.UtcNow;

                    NewReceived.USDEUR = item.ReceivingValue.USDEUR;
                    NewReceived.USDCOP = item.ReceivingValue.USDCOP;
                    NewReceived.ValueReceivedCurrency = item.ReceivingValue.ReceivingValueCurrency;
                    await Repository.AddAsync(NewReceived);
                }

            }


            await Repository.UpdateAsync(purchaseorder);

            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCachePurchaseOrder(purchaseorder));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.Approve, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.Approve, ClassNames.PurchaseOrders));
        }

    }
}
