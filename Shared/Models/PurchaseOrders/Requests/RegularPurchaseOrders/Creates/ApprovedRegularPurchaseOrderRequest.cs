using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits;
using Shared.Models.PurchaseorderStatus;
using Shared.Models.Suppliers;
using System.Xml.Linq;

namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates
{
    public class ApprovedRegularPurchaseOrderRequest : EditPurchaseOrderRegularCreatedRequest
    {


        public DateTime? ExpectedDate { get; set; }
        public string PONumber { get; set; } = string.Empty;
       
       

    }
}
