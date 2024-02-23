using Shared.Models.PurchaseorderStatus;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class PurchaseOrderResponse
    {

        public List<PurchaseorderItemsResponse> PurchaseOrderItems { get; set; } = new();
        public Guid PurchaseorderId { get; set; }
        public string PurchaseorderName { get; set; } = string.Empty;
        public string PurchaseRequisition { get; set; } = string.Empty;
        public string MWOName { get; set; } = string.Empty;
        public string PONumber { get; set; } = string.Empty;

        public Guid MWOId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? ExpetedOn { get; set; }
        public string Supplier { get; set; } = string.Empty;
        public string QuoteNo { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public string AccountAssigment { get; set; } = string.Empty;
        public string MWOCode { get; set; } = string.Empty;
        public string TaxCode { get; set; } = string.Empty;
        public Guid? SupplierId { get; set; }
        public double PurchaseOrderValue => PurchaseOrderItems.Sum(x => x.POValueUSD);

        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;
        public string PurchaseOrderStatusName => PurchaseOrderStatus.Name;
    }


}
