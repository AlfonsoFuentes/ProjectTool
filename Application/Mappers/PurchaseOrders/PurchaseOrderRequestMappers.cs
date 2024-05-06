using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.Mappers.PurchaseOrders
{
    public static class PurchaseOrderRequestMappers
    {

        public static PurchaseOrder FromCreatePurchaseOrderRequest(this NewPurchaseOrderCreateRequest request, PurchaseOrder po)
        {
            po.AccountAssigment = request.AccountAssigment;
            po.CreatedDate = DateTime.UtcNow;
            po.CurrencyDate = request.CurrencyDate;
            po.IsAlteration = request.IsAlteration;
            po.IsCapitalizedSalary = request.IsCapitalizedSalary;
            po.MainBudgetItemId = request.MainBudgetItemId;
            po.PurchaseorderName = request.PurchaseorderName;
            po.PurchaseOrderStatus = request.PurchaseOrderStatus.Id;
            po.PurchaseRequisition = request.PurchaseRequisition;
            po.QuoteCurrency = request.QuoteCurrency.Id;
            po.PurchaseOrderCurrency=request.PurchaseOrderCurrency.Id;
            po.QuoteNo = request.QuoteNo;
            po.USDCOP = request.USDCOP;
            po.USDEUR = request.USDEUR;
            po.SupplierId = request.SupplierId;
            po.TaxCode = request.TaxCode;
            po.SPL = request.SPL;
            po.PONumber = request.PurchaseOrderNumber;
            po.IsTaxEditable = request.IsTaxEditable;

            return po;
        }
    }
}
