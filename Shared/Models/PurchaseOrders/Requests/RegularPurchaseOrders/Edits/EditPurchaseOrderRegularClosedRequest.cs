using Shared.Models.BudgetItems;
using Shared.Models.Currencies;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseorderStatus;
using Shared.Models.Suppliers;

namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits
{
    public class EditPurchaseOrderRegularClosedRequest : EditPurchaseOrderRegularApprovedRequest
    {
        public EditPurchaseOrderRegularClosedRequest()
        {

        }

        public double POValueUSDOriginal { get; set; }
        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;
    }
}
