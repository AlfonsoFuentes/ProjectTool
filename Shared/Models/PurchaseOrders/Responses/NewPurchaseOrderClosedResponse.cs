using Shared.Enums.Currencies;
using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.Suppliers.Reponses;


namespace Shared.Models.PurchaseOrders.Responses
{
    public class NewPurchaseOrderClosedResponse
    {
        public Guid PurchaseOrderId {  get; set; }
        public Guid MWOId { get; set; }
        public string MWOName { get; set; } = string.Empty;
        public string CECName {  get; set; } = string.Empty;
        public Guid MainBudgetItemId { get; set; }
        public Guid? SupplierId => Supplier == null ? Guid.Empty : Supplier.SupplierId;
        public NewSupplierResponse? Supplier { get; set; } = null!;
        public string SupplierNickName => Supplier == null ? string.Empty : Supplier.NickName;
        public string SupplierName => Supplier == null ? string.Empty : Supplier.Name;
        public string VendorCode => Supplier == null ? string.Empty : Supplier.VendorCode;
        public string QuoteNo { get; set; } = "";
        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;
        public string PurchaseRequisition { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
        public string ClosedOn { get; set; } = string.Empty;
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public string SPL { get; set; } = string.Empty;
        public string TaxCode { get; set; } = string.Empty;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public string CurrencyDate { get; set; } = string.Empty;
        public string AccountAssigment { get; set; } = string.Empty;
        public string PurchaseorderName { get; set; } = string.Empty;
        public bool IsAlteration { get; set; } = false;
        public bool IsCapitalizedSalary { get; set; } = false;
        public bool IsTaxEditable { get; set; } = false;
        public double ApprovedUSD { get; set; }

    }
}
