namespace Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems
{
    public class ReceivePurchaseorderItemRequestDto
    {
        public Guid PurchaseOrderItemId { get; set; }
        public Guid BudgetItemId { get; set; }
        public double POActualUSD { get; set; }
        public double POPendingUSD { get; set; }


    }
}
