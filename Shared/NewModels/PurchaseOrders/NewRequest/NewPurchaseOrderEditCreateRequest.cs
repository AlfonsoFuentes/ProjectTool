using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Base;

namespace Shared.NewModels.PurchaseOrders.Request
{
    public class NewPurchaseOrderEditCreateRequest
    {
        public NewPurchaseOrderRequest PurchaseOrder { get; set; } = new NewPurchaseOrderRequest();
        public NewPurchaseOrderEditCreateRequest()
        {
          
        }



    }
}
