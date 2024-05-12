using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.NewCommands
{
    public record NewPurchaseOrderEditCreateCommand(NewPurchaseOrderEditCreateRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderEditCreateCommandHandler : IRequestHandler<NewPurchaseOrderEditCreateCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderEditCreateCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderEditCreateCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await Repository.GetPurchaseOrderByIdCreatedAsync(request.Data.PurchaseOrder.PurchaseOrderId);
            if (purchaseOrder == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.NotFound, ClassNames.PurchaseOrders));
            }

            request.Data.PurchaseOrder.ToPurchaseOrderFromRequest(purchaseOrder);

            foreach (var item in purchaseOrder.PurchaseOrderItems)
            {
                if (!request.Data.PurchaseOrder.PurchaseOrderItems.Any(x => x.BudgetItemId == item.BudgetItemId))
                {
                    await Repository.RemoveAsync(item);
                }
            }

            foreach (var item in request.Data.PurchaseOrder.PurchaseOrderItems)
            {
                var purchaseorderitem = await Repository.GetByIdAsync<PurchaseOrderItem>(item.PurchaseOrderItemId);
                if (purchaseorderitem == null)
                {
                    purchaseorderitem = purchaseOrder.AddPurchaseOrderItem(item.BudgetItemId);
                    item.ToPurchaseOrderItemFromRequest(purchaseorderitem);
                    await Repository.AddAsync(purchaseorderitem);
                }
                else
                {
                    item.ToPurchaseOrderItemFromRequest(purchaseorderitem);
                    await Repository.UpdateAsync(purchaseorderitem);
                }
            }


            await Repository.UpdateAsync(purchaseOrder);

            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCachePurchaseOrder(purchaseOrder));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.Updated, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.Updated, ClassNames.PurchaseOrders));
        }
    }
}
