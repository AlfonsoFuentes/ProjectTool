namespace Shared.Models.PurchaseOrders.Requests.Approves
{
    public class PurchaseorderItemsToApproveRequest
    {
        public double POValueUSD { get; set; }
        public Guid BudgetItemId { get; set; }

        public string BudgetItemName { get; set; }=string.Empty;

    }
}
