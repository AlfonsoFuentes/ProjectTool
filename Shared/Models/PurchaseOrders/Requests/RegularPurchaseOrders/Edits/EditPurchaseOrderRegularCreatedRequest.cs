using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;

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
