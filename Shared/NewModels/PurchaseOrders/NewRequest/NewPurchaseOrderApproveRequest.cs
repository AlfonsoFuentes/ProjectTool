using Shared.NewModels.PurchaseOrders.Base;

namespace Shared.NewModels.PurchaseOrders.Request
{
    public class NewPurchaseOrderApproveRequest
    {
        public NewPurchaseOrderRequest PurchaseOrder { get; set; } = new NewPurchaseOrderRequest();
        public NewPurchaseOrderApproveRequest()
        {

        }

    }
}
