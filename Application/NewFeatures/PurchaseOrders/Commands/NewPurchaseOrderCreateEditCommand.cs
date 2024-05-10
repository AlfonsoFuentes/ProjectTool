using Application.Mappers.PurchaseOrders;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.Commands
{
    public record NewPurchaseOrderCreateEditCommand(NewPurchaseOrderCreateEditRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderCreateEditCommandHandler : IRequestHandler<NewPurchaseOrderCreateEditCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderCreateEditCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderCreateEditCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await Repository.GetPurchaseOrderByIdCreatedAsync(request.Data.PurchaseOrderId);
            if (purchaseOrder == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.NotFound, ClassNames.PurchaseOrders));
            }

            request.Data.FromCreatedEditPurchaseOrderRequest(purchaseOrder);

            foreach(var item in purchaseOrder.PurchaseOrderItems)
            {
                if(!request.Data.PurchaseOrderItems.Any(x=>x.BudgetItemId == item.BudgetItemId))
                {
                    await Repository.RemoveAsync(item);
                }
            }

            foreach (var item in request.Data.PurchaseOrderItems)
            {
                var purchaseorderitem = await Repository.GetByIdAsync<PurchaseOrderItem>(item.PurchaseOrderItemId);
                if (purchaseorderitem == null)
                {
                    purchaseorderitem = purchaseOrder.AddPurchaseOrderItem(item.BudgetItemId);
                    item.ToPurchaseOrderItemFromCreateRequest(purchaseorderitem);
                    await Repository.AddAsync(purchaseorderitem);
                }
                else
                {
                    item.ToPurchaseOrderItemFromCreateRequest(purchaseorderitem);
                    await Repository.UpdateAsync(purchaseorderitem);
                }
            }

               
            await Repository.UpdateAsync(purchaseOrder);

            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken,Cache.GetParamsCachePurchaseOrder(purchaseOrder));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseorderName, ResponseType.Updated, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.Updated, ClassNames.PurchaseOrders));
        }
    }
}
