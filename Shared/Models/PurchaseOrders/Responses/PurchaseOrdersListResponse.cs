namespace Shared.Models.PurchaseOrders.Responses
{
    public class PurchaseOrdersListResponse
    {
        public IEnumerable<PurchaseOrderResponse> PurchaseordersCreated { get; set; } = null!;
        public IEnumerable<PurchaseOrderResponse> PurchaseordersApproved { get; set; } = null!;
        public IEnumerable<PurchaseOrderResponse> PurchaseordersClosed { get; set; } = null!;
    }
}
