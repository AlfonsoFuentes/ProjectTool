namespace Shared.Models.PurchaseOrders.Responses
{
    public class PurchaseOrdersListResponse
    {
        public List<PurchaseOrderResponse> Purchaseorders { get; set; } = new();
    }

}
