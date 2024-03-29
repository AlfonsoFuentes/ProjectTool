using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.Currencies;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;
using Shared.Models.Suppliers;

namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits
{

    public class EditPurchaseOrderRegularCreatedRequest : CreatedRegularPurchaseOrderRequest
    {
        public EditPurchaseOrderRegularCreatedRequest()
        {

        }
        public Guid PurchaseOrderId { get; set; }

    }

}
