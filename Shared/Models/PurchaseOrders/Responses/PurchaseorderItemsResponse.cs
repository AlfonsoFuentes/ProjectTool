namespace Shared.Models.PurchaseOrders.Responses
{
    public class PurchaseorderItemsResponse
    {
        public double POValueUSD { get; set; }
        public Guid BudgetItemId { get; set; }
        public Guid PurchaseorderItemId { get; set; }

    }
}
