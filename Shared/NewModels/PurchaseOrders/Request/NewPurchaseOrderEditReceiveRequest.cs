using Shared.Enums.PurchaseorderStatus;

namespace Shared.NewModels.PurchaseOrders.Request
{
    public class OldPurchaseOrderEditReceiveRequest
    {
        public Guid PurchaseOrderId { get; set; }
        public string PurchaseOrderNumber { get; set; } = string.Empty;

        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public Guid MWOId { get; set; }
        public string CECName { get; set; } = string.Empty;
        public string MWOName { get; set; } = string.Empty;
        public Guid MainBudgetItemId { get; set; }
        public Guid? SupplierId => Supplier == null ? Guid.Empty : Supplier.SupplierId;
        public string SupplierName => Supplier == null ? string.Empty : Supplier.Name;
        public string SupplierNickName => Supplier == null ? string.Empty : Supplier.NickName;
        public string SupplierVendorCode => Supplier == null ? string.Empty : Supplier.VendorCode;
        public string TaxCode { get; set; } = string.Empty;
        public NewSupplierResponse? Supplier { get; set; } = null!;
        public List<NewPurchaseOrderReceiveItemRequest> PurchaseOrderItems { get; set; } = new List<NewPurchaseOrderReceiveItemRequest>();
        public double MaxPercentageToReceive => POValueCurrency == 0 ? 0 : Math.Round(PendingCurrency / POValueCurrency * 100.0, 2);
        public string QuoteNo { get; set; } = "";

        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.Created;
        public string PurchaseRequisition { get; set; } = "";
        public string SPL { get; set; } = string.Empty;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public DateTime CurrencyDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public string AccountAssigment { get; set; } = string.Empty;

        public string MWOCECName { get; set; } = string.Empty;
        public string PurchaseorderName { get; set; } = string.Empty;
        public bool IsAlteration { get; set; }
        public bool IsCapitalizedSalary { get; set; }

        public bool IsTaxEditable { get; set; }

        public double POValueUSD => PurchaseOrderItems.Sum(x => x.ItemQuoteValueUSD);
        public double POValueCurrency => PurchaseOrderItems.Sum(x => x.ItemQuoteValueCurrency);
        public bool IsAnyValueNotDefined => PurchaseOrderItems.Any(x => x.IsAnyValueNotDefined);
        public bool IsAnyNameEmpty => PurchaseOrderItems.Any(x => x.IsNameEmpty);

        public double TotalBudgetUSD => PurchaseOrderItems.Sum(x => x.BudgetUSD);
        public double TotalAssignedUSD => PurchaseOrderItems.Sum(x => x.BudgetAssignedUSD);
        public double TotalPotentialUSD => PurchaseOrderItems.Sum(x => x.BudgetPotentialUSD);
        public double NewTotalPendingToCommitUSD => PurchaseOrderItems.Sum(x => x.PendingToCommitmUSD);
        public double NewTotalAssignedUSD => TotalAssignedUSD;
        public double NewTotalPotentialUSD => POValueUSD + TotalPotentialUSD;

        public double ActualUSD => PurchaseOrderItems.Sum(x => x.NewActualUSD);
        public double PendingUSD => PurchaseOrderItems.Sum(x => x.NewPendingUSD);
        public double ActualCurrency => PurchaseOrderItems.Sum(x => x.NewActualCurrency);
        public double PendingCurrency => PurchaseOrderItems.Sum(x => x.NewPendingCurrency);
        public double ReceivingCurrency => PurchaseOrderItems.Sum(x => x.ReceivingCurrency);
        public double ReceivedCurrency => PurchaseOrderItems.Sum(x => x.ActualCurrency);
        public double ReceivingUSD => PurchaseOrderItems.Sum(x => x.ReceivingUSD);

        public double ReceivedUSD => PurchaseOrderItems.Sum(x => x.ActualUSD);
        public bool IsCompletedReceived => PendingCurrency == 0;


    }
}
