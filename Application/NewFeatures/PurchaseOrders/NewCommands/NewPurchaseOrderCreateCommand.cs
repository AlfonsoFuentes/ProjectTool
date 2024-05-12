namespace Application.NewFeatures.PurchaseOrders.NewCommands
{
    public record NewPurchaseOrderCreateCommand(NewPurchaseOrderCreateRequest Data) : IRequest<IResult>;
    internal class NewNewPurchaseOrderCreateRequestHandler : IRequestHandler<NewPurchaseOrderCreateCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewNewPurchaseOrderCreateRequestHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderCreateCommand request, CancellationToken cancellationToken)
        {
            var mwo = await Repository.GetByIdAsync<MWO>(request.Data.PurchaseOrder.MWOId);
            if (mwo == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseOrder.MainBudgetItem.MWOName, ResponseType.NotFound, ClassNames.MWO));
            }

            var purchaseorder = mwo.AddPurchaseOrder();

            request.Data.PurchaseOrder.ToPurchaseOrderFromRequest(purchaseorder);



            foreach (var item in request.Data.PurchaseOrder.PurchaseOrderItems)
            {
                var purchaseorderitem = purchaseorder.AddPurchaseOrderItem(item.BudgetItemId);

                item.ToPurchaseOrderItemFromRequest(purchaseorderitem);

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
