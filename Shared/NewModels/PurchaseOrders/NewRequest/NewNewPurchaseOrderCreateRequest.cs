using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Base;

namespace Shared.NewModels.PurchaseOrders.Request
{
    public class NewNewPurchaseOrderCreateRequest : NewPurchaseOrderRequest
    {

        public NewNewPurchaseOrderCreateRequest()
        {
            PurchaseOrderStatus = PurchaseOrderStatusEnum.Created;
        }



    }
}
