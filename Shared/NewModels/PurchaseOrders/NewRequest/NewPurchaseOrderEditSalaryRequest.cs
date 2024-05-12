using Shared.NewModels.PurchaseOrders.Base;

namespace Shared.NewModels.PurchaseOrders.Request
{
    public class NewPurchaseOrderEditSalaryRequest
    {
        public NewPurchaseOrderRequest PurchaseOrder { get; set; } = new NewPurchaseOrderRequest();
        public NewPurchaseOrderEditSalaryRequest()
        {
        }
     

        public bool CreatePurchaseOrderNumber { get; set; }

        public double PurchaseOrderSalary {  get; set; }

        public void EditPurchaseOrderSalary(double purchaseOrderSalary)
        {
            PurchaseOrderSalary=purchaseOrderSalary;
            if(PurchaseOrder.PurchaseOrderItems.Count > 0)
            {
                PurchaseOrder.PurchaseOrderItems[0].UnitaryValueQuoteCurrency = purchaseOrderSalary;
                if(PurchaseOrder.PurchaseOrderItems[0].PurchaseOrderReceiveds.Count > 0)
                {
                    PurchaseOrder.PurchaseOrderItems[0].PurchaseOrderReceiveds[0].ValueReceivedCurrency = purchaseOrderSalary;
                }
                
            }
            

        }
    }
}
