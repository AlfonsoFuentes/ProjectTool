using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits;
using Shared.Models.PurchaseorderStatus;
using Shared.Models.Suppliers;
using System.Xml.Linq;

namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates
{
    public class ApprovedRegularPurchaseOrderRequest: EditPurchaseOrderRegularCreatedRequest
    {
       
      
        public DateTime? ExpectedDate { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public async Task ChangePONumber(string ponumber)
        {
           
            PONumber = ponumber.Trim();
            if (Validator != null) await Validator();

        }
        public async Task ChangedExpectedDate(DateTime? expected)
        {
            ExpectedDate = expected;
            if (Validator != null) await Validator();
        }


    }
}
