using Shared.Enums.PurchaseorderStatus;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits;

namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates
{
    public class ApprovedRegularPurchaseOrderRequest : EditPurchaseOrderRegularCreatedRequest
    {

        public override PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.Approved;
        public DateTime? ExpectedDate { get; set; }
        public string PONumber { get; set; } = string.Empty;
       
       

    }
}
