using Azure.Core;
using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.Mappers.PurchaseOrders
{
    public static class PurchaseOrderRequestMappers
    {

       
     
        public static PurchaseOrder FromCreatePurchaseOrderSalaryRequest(this NewPurchaseOrderCreateSalaryRequest request, PurchaseOrder po)
        {
            po.AccountAssigment = request.AccountAssigment;
            po.CreatedDate = DateTime.UtcNow;
       
            po.IsAlteration = request.IsAlteration;
            po.IsCapitalizedSalary = request.IsCapitalizedSalary;
            po.MainBudgetItemId = request.MainBudgetItemId;
            po.PurchaseorderName = request.PurchaseorderName;
       
            
            
            po.USDCOP = request.USDCOP;
            po.USDEUR = request.USDEUR;
            po.SPL = request.SPL;
            po.PONumber = request.PurchaseOrderNumber;
            po.IsTaxEditable = false;
            po.POClosedDate = DateTime.Now;
            po.POExpectedDateDate = DateTime.Now;
            po.PurchaseOrderStatus = PurchaseOrderStatusEnum.Closed.Id;
            po.CurrencyDate = DateTime.Now;
            po.PurchaseOrderCurrency = CurrencyEnum.USD.Id;
            po.QuoteCurrency = CurrencyEnum.USD.Id;
            return po;
        }
        public static PurchaseOrder FromEditPurchaseOrderSalaryrequest(this NewPurchaseOrderEditSalaryRequest request, PurchaseOrder po)
        {
            po.AccountAssigment = request.AccountAssigment;
            po.CreatedDate = DateTime.UtcNow;
         
            po.IsAlteration = request.IsAlteration;
            po.IsCapitalizedSalary = request.IsCapitalizedSalary;
            po.MainBudgetItemId = request.MainBudgetItemId;
            po.PurchaseorderName = request.PurchaseorderName;



            po.USDCOP = request.USDCOP;
            po.USDEUR = request.USDEUR;
            po.SPL = request.SPL;
            po.PONumber = request.PurchaseOrderNumber;
            po.IsTaxEditable = false;
            
            return po;
        }
    }
}
