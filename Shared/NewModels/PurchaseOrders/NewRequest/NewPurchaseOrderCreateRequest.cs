using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Base;

namespace Shared.NewModels.PurchaseOrders.Request
{
    public class NewPurchaseOrderCreateRequest 
    {
        public NewPurchaseOrderRequest PurchaseOrder {  get; set; }=new NewPurchaseOrderRequest();
        public NewPurchaseOrderCreateRequest()
        {
            PurchaseOrder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Created;
        }



    }
}
