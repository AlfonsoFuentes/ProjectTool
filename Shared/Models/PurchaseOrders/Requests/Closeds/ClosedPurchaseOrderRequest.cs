using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.Approves;
using Shared.Models.PurchaseorderStatus;
using Shared.Models.Suppliers;

namespace Shared.Models.PurchaseOrders.Requests.Closeds
{
    public class ClosedPurchaseOrderRequest
    {
        public List<string> ValidationErrors { get; set; } = new();
      
        public Guid PurchaseorderId { get; set; }
        public string PurchaseorderName { get; set; } = string.Empty;
        public string PurchaseRequisition { get; set; } = string.Empty;
        public string MWOName { get; set; } = string.Empty;
        public string PONumber { get; set; } = string.Empty;
        public bool IsNoAssetProductive { get; set; }
        public Guid MWOId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? ExpetedOn { get; set; } = DateTime.UtcNow;
        public SupplierResponse Supplier { get; set; } = null!;
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
       
        public double USDCOP {  get; set; }
        public double USDEUR { get; set; }
        public double PercentageAlteration { get; set; }
        public string MWOCECName {  get; set; } = string.Empty;
        public DateTime CurrencyDate {  get; set; } = DateTime.MinValue;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;
        public bool AssetRealProductive { get; set; }
        public string PurchaseOrderName {  get; set; } = string.Empty;
        public Guid MainBudgetItemId {  get; set; }
        public PurchaseOrderStatusEnum PurchaseOrderStatusEnum { get; set; } = PurchaseOrderStatusEnum.None;
    }
}
