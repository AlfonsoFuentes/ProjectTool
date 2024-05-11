using Application.Mappers.PurchaseOrders.NewMappers;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.NewCommands
{
    public record NewPurchaseOrderEditApproveCommand(NewPurchaseOrderEditApproveRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderEditApproveCommandHandler : IRequestHandler<NewPurchaseOrderEditApproveCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderEditApproveCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderEditApproveCommand request, CancellationToken cancellationToken)
        {
            var purchaseorder = await Repository.GetPurchaseOrderByIdCreatedAsync(request.Data.PurchaseOrder.PurchaseOrderId);
            if (purchaseorder == null)
            {
                return Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.NotFound, ClassNames.PurchaseOrders));
            }
            request.Data.PurchaseOrder.ToPurchaseOrderApprovedFromRequest(purchaseorder);
            foreach (var item in purchaseorder.PurchaseOrderItems)
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
                    purchaseorderitem = purchaseorder.AddPurchaseOrderItem(item.BudgetItemId);
                    item.ToPurchaseOrderItemFromRequest(purchaseorderitem);
                    await Repository.AddAsync(purchaseorderitem);
                }
                else
                {
                    item.ToPurchaseOrderItemFromRequest(purchaseorderitem);
                    await Repository.UpdateAsync(purchaseorderitem);
                }
            }
            if (request.Data.PurchaseOrder.IsAlteration)
            {
                await UpdateTaxesForAlterations(purchaseorder, request);
            }
            else if (!request.Data.PurchaseOrder.IsAssetProductive)
            {

                await UpdateTaxesForNonProductive(purchaseorder, request);

            }

            await Repository.UpdateAsync(purchaseorder);

            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCachePurchaseOrder(purchaseorder));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.Approve, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.Approve, ClassNames.PurchaseOrders));
        }
        async Task UpdateTaxesForAlterations(PurchaseOrder purchaseorder, NewPurchaseOrderEditApproveCommand request)
        {
            foreach (var row in request.Data.PurchaseOrder.PurchaseOrderItems)
            {
                var purchaseordertaxestem =await Repository.GetBudgetItemTaxAlteration(request.Data.PurchaseOrder.PurchaseOrderId,
                    row.BudgetItemId);
                if(purchaseordertaxestem != null)
                {
                    var povalucurrency = purchaseorder.MWO.PercentageTaxForAlterations / 100.0 * row.POItemValuePurchaseOrderCurrency;
                    purchaseordertaxestem.UnitaryValueCurrency = povalucurrency;
                    purchaseordertaxestem.Quantity = 1;

                    await Repository.UpdateAsync(purchaseordertaxestem);
                }
                

                
            }
        }
        async Task UpdateTaxesForNonProductive(PurchaseOrder purchaseorder, NewPurchaseOrderEditApproveCommand request)
        {
            var TaxBudgetitem = await Repository.GetTaxBudgetItemNoProductive(purchaseorder.MWOId);
            if (TaxBudgetitem != null)
            {

                var purchaseordertaxestem = await Repository.GetBudgetItemTaxNoProductive(request.Data.PurchaseOrder.PurchaseOrderId, TaxBudgetitem.Id);
                if(purchaseordertaxestem != null)
                {
                    
                    purchaseordertaxestem.UnitaryValueCurrency = TaxBudgetitem.Percentage / 100.0 * purchaseorder.QuoteValueCurrency;
                    purchaseordertaxestem.Quantity = 1;
                    await Repository.UpdateAsync(purchaseordertaxestem);
                }
                

            }
        }
    }
}
