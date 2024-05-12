using Application.NewFeatures.PurchaseOrders.Commands;
using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.NewFeatures.PurchaseOrders.NewCommands
{
    public record NewPurchaseOrderApproveCommand(NewPurchaseOrderApproveRequest Data) : IRequest<IResult>;
    internal class NewPurchaseOrderApproveCommandHandler : IRequestHandler<NewPurchaseOrderApproveCommand, IResult>
    {
        private IAppDbContext AppDbContext { get; set; }
        private IRepository Repository { get; set; }

        public NewPurchaseOrderApproveCommandHandler(IRepository repository, IAppDbContext appDbContext)
        {
            Repository = repository;
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(NewPurchaseOrderApproveCommand request, CancellationToken cancellationToken)
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
                await CreateTaxesForAlterations(purchaseorder, request);
            }
            else if (!request.Data.PurchaseOrder.IsAssetProductive)
            {

                await CreateTaxesForNonProductive(purchaseorder, request);

            }

            await Repository.UpdateAsync(purchaseorder);

            var result = await AppDbContext.SaveChangesAndRemoveCacheAsync(cancellationToken, Cache.GetParamsCachePurchaseOrder(purchaseorder));

            return result > 0 ?
              Result.Success(ResponseMessages.ReponseSuccesfullyMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.Approve, ClassNames.PurchaseOrders)) :
              Result.Fail(ResponseMessages.ReponseFailMessage(request.Data.PurchaseOrder.PurchaseOrderName, ResponseType.Approve, ClassNames.PurchaseOrders));
        }
        async Task CreateTaxesForAlterations(PurchaseOrder purchaseorder, NewPurchaseOrderApproveCommand request)
        {
            foreach (var row in request.Data.PurchaseOrder.PurchaseOrderItems)
            {
                var purchaseordertaxestem = purchaseorder.AddPurchaseOrderItemForAlteration(row.BudgetItemId);
                purchaseordertaxestem.Name = $"{request.Data.PurchaseOrder.PurchaseOrderNumber} Tax {row.BudgetItem.Name} {purchaseorder.MWO.PercentageTaxForAlterations}%";

                var povalucurrency = purchaseorder.MWO.PercentageTaxForAlterations / 100.0 * row.POItemValuePurchaseOrderCurrency;
                purchaseordertaxestem.UnitaryValueCurrency = povalucurrency;
                purchaseordertaxestem.Quantity = 1;

                await Repository.AddAsync(purchaseordertaxestem);
            }
        }
        async Task CreateTaxesForNonProductive(PurchaseOrder purchaseorder, NewPurchaseOrderApproveCommand request)
        {
            var TaxBudgetitem = await Repository.GetTaxBudgetItemNoProductive(purchaseorder.MWOId);
            if (TaxBudgetitem != null)
            {

                var purchaseordertaxestem = purchaseorder.AddPurchaseOrderItemForNoProductiveTax(TaxBudgetitem.Id);
                purchaseordertaxestem.Name = $"{request.Data.PurchaseOrder.PurchaseOrderNumber} Tax {TaxBudgetitem.Percentage}%";
                purchaseordertaxestem.UnitaryValueCurrency = TaxBudgetitem.Percentage / 100.0 * purchaseorder.QuoteValueCurrency;
                purchaseordertaxestem.Quantity = 1;
                await Repository.AddAsync(purchaseordertaxestem);

            }
        }
    }
}
