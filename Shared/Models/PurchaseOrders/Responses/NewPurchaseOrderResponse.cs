using Shared.Enums.Currencies;
using Shared.Enums.PurchaseorderStatus;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class NewPurchaseOrderResponse
    {

        public string LabelAction => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? $"Edit {PurchaseRequisition}" : $"Edit {PurchaseOrderNumber}";
        public Guid PurchaseOrderId { get; set; }
        public Guid MWOId { get; set; }
        public string MWOName { get; set; } = string.Empty;
        public string CECName { get; set; } = string.Empty;
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
        public string ExpectedOn => DateExpectedOn == null ? string.Empty : DateExpectedOn!.Value.ToString("d");
        public string ClosedOn => DateClosedOn==null?string.Empty: DateClosedOn!.Value.ToString("d");
        public DateTime? DateExpectedOn { get; set; }
        public DateTime? DateClosedOn { get; set; }
        public DateTime? DateApprovedOn { get; set; }
        public bool DueDate => DateTime.Now > DateExpectedOn;
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
        public bool IsTaxNoProductive => !IsTaxEditable;
        public double ActualUSD { get; set; }
        public double CommitmentUSD => ApprovedUSD - ActualUSD;
        public double ApprovedUSD { get; set; }
        public double AssignedUSD {  get; set; }
        public double PotentialCommitmentUSD { get; set; }
    }
}
