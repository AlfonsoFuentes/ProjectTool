namespace Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems
{
    public class PurchaseorderItemsToApproveRequest
    {
        public double POValueUSD { get; set; }
        public Guid BudgetItemId { get; set; }

        public string BudgetItemName { get; set; } = string.Empty;
        public string PurchaseorderItemName {  get; set; } = string.Empty;
        public Guid PurchaseOrderItemId { get; set; }
    }
}
