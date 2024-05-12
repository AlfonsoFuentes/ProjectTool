using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Base;

namespace Shared.NewModels.PurchaseOrders.Request
{
    public class NewPurchaseOrderCreateSalaryRequest
    {
        public NewPurchaseOrderRequest PurchaseOrder { get; set; } = new NewPurchaseOrderRequest();
        public NewPurchaseOrderCreateSalaryRequest()
        {
            PurchaseOrder.PurchaseOrderStatus = PurchaseOrderStatusEnum.Created;
     
        }
        public NewPurchaseOrderItemRequest PurchaseOrderItemSalary { get; set; } = new();

        public bool CreatePurchaseOrderNumber {  get; set; }
    }
}
