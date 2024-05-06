namespace Shared.NewModels.PurchaseOrders.Request
{
    public class NewPurchaseOrderReOpenRequest
    {
        public Guid PurchaseOrderId { get; set; }
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public string PurchaseorderName { get; set; } = string.Empty;
    }
}
