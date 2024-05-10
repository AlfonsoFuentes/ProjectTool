using Application.Mappers.PurchaseOrders;
using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.Commands
{
    public record NewPurchaseOrderEditApprovedCommand(NewPurchaseOrderEditApprovedRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderEditApprovedCommandHandler : IRequestHandler<NewPurchaseOrderEditApprovedCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderEditApprovedCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderEditApprovedCommand request, CancellationToken cancellationToken)
        {
            var purchaseorder = await Repository.GetPurchaseOrderByIdCreatedAsync(request.Data.PurchaseOrderId);
            if (purchaseorder == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
          
            purchaseorder.PONumber = request.Data.PurchaseOrderNumber;
            purchaseorder.POExpectedDateDate = request.Data.ExpectedDate!.Value;
            purchaseorder.USDCOP = request.Data.USDCOP;
            purchaseorder.USDEUR = request.Data.USDEUR;

            foreach (var item in purchaseorder.PurchaseOrderItems)
            {
                if (!request.Data.PurchaseOrderItems.Any(x => x.BudgetItemId == item.BudgetItemId))
                {
                    await Repository.RemoveAsync(item);
                }
            }

            foreach (var item in request.Data.PurchaseOrderItems)
            {
                var purchaseorderitem = await Repository.GetByIdAsync<PurchaseOrderItem>(item.PurchaseOrderItemId);
                if (purchaseorderitem == null)
                {
                    purchaseorderitem = purchaseorder.AddPurchaseOrderItem(item.BudgetItemId);
                    item.ToPurchaseOrderItemFromCreateRequest(purchaseorderitem);
                    await Repository.AddAsync(purchaseorderitem);
                }
                else
                {
                    item.ToPurchaseOrderItemFromCreateRequest(purchaseorderitem);
                    await Repository.UpdateAsync(purchaseorderitem);
                }
            }
            await Repository.UpdateAsync(purchaseorder);

            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCachePurchaseOrder(purchaseorder));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseorderName, ResponseType.Received, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseorderName, ResponseType.Received, ClassNames.PurchaseOrders));
        }
    }
}
