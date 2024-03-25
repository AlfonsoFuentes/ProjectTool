namespace Shared.Models.PurchaseOrders.Responses
{
    public class PurchaseorderItemsResponse
    {
        public double POValueUSD { get; set; }
        public Guid BudgetItemId { get; set; }
        public Guid PurchaseorderItemId { get; set; }
        public double Actual {  get; set; }
        public double Commitmment => POValueUSD - Actual;
        public double Assigned => Actual + Commitmment + Potencial;
        public double Potencial {  get; set; }

    }
}
