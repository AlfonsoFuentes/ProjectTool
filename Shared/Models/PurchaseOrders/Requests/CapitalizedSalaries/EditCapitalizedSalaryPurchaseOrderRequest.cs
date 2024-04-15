namespace Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries
{
    public class EditCapitalizedSalaryPurchaseOrderRequest: CreateCapitalizedSalaryPurchaseOrderRequest
    {
        
        public Guid PurchaseOrderId { get; set; }
       

    }
}
