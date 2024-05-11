using Shared.NewModels.PurchaseOrders.Base;

namespace Shared.NewModels.PurchaseOrders.Request
{
    public class NewPurchaseOrderEditReceiveRequest
    {
        public NewPurchaseOrderRequest PurchaseOrder { get; set; } = new NewPurchaseOrderRequest();
        public NewPurchaseOrderEditReceiveRequest()
        {

        }

    }
}
