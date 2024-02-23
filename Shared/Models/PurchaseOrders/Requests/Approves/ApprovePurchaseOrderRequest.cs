using Shared.Models.PurchaseorderStatus;
using System.Xml.Linq;

namespace Shared.Models.PurchaseOrders.Requests.Approves
{
    public class ApprovePurchaseOrderRequest
    {
        public List<string> ValidationErrors { get; set; } = new();
        public List<PurchaseorderItemsToApproveRequest> ItemsInPurchaseorder { get; set; } = new();
        public Guid PurchaseorderId { get; set; }
        public string PurchaseorderName { get; set; } = string.Empty;
        public string PurchaseRequisition { get; set; } = string.Empty;
        public string MWOName { get; set; } = string.Empty;
        public string PONumber { get; set; } = string.Empty;
        public void ChangePONumber(string ponumber)
        {
            ValidationErrors.Clear();
            PONumber = ponumber.Trim();


        }
        public Guid MWOId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? ExpetedOn { get; set; } = DateTime.UtcNow;
        public string Supplier { get; set; } = string.Empty;
        public string QuoteNo { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public string AccountAssigment { get; set; } = string.Empty;
        public string MWOCode { get; set; } = string.Empty;
        public string TaxCode { get; set; } = string.Empty;
        public Guid? SupplierId { get; set; }
        public double PurchaseOrderValue { get; set; }
        public bool IsMWONoProductive { get; set; }
        public bool IsAlteration { get; set; }
        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;
    }
}
