namespace Application.NewFeatures.PurchaseOrders.NewCommands
{
    public record NewPurchaseOrderCreateSalaryCommand(NewPurchaseOrderCreateSalaryRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderCreateSalaryCommandHandler : IRequestHandler<NewPurchaseOrderCreateSalaryCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderCreateSalaryCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderCreateSalaryCommand request, CancellationToken cancellationToken)
        {
            var mwo = await Repository.GetByIdAsync<MWO>(request.Data.PurchaseOrder.MWOId);
            if (mwo == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseOrder.MainBudgetItem.MWOName, ResponseType.NotFound, ClassNames.MWO));
            }

            var purchaseorder = mwo.AddPurchaseOrder();

            request.Data.PurchaseOrder.ToPurchaseOrderFromSalaryRequest(purchaseorder);

            foreach (var item in request.Data.PurchaseOrder.PurchaseOrderItems)
            {
                var purchaseorderitem = purchaseorder.AddPurchaseOrderItem(item.BudgetItemId);

                item.ToPurchaseOrderItemFromRequest(purchaseorderitem);
                var received = purchaseorderitem.AddPurchaseOrderReceived();

                received.CurrencyDate = DateTime.UtcNow;
                received.USDEUR = request.Data.PurchaseOrder.USDEUR;
                received.USDCOP = request.Data.PurchaseOrder.USDCOP;
                received.ValueReceivedCurrency = item.POItemValuePurchaseOrderCurrency;
                await Repository.AddAsync(received);
                await Repository.AddAsync(purchaseorderitem);



            }
            await Repository.AddAsync(purchaseorder);

            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCachePurchaseOrder(purchaseorder));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.Created, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.Created, ClassNames.PurchaseOrders));
        }
    }
}
