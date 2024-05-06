namespace Shared.NewModels.PurchaseOrders.Request
{
    public class NewPurchaseOrderDeleteRequest
    {
        public Guid PurchaseOrderId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
