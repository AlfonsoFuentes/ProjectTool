namespace Shared.Models.PurchaseOrders.Responses
{
    public class NewPurchaseOrdersListResponse
    {
        public IEnumerable<NewPurchaseOrderCreatedResponse> PurchaseordersCreated { get; set; } = null!;
        public IEnumerable<NewPurchaseOrderApprovedResponse> PurchaseordersApproved { get; set; } = null!;
        public IEnumerable<NewPurchaseOrderClosedResponse> PurchaseordersClosed { get; set; } = null!;
    }
}
