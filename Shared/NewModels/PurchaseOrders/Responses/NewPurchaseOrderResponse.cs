using Shared.Enums.PurchaseorderStatus;

namespace Shared.NewModels.PurchaseOrders.Responses
{
    public class NewPriorPurchaseOrderCreatedResponse
    {
        public List<NewPriorPurchaseOrderResponse> PurchaseOrders { get; set; } = new();
    }
    public class NewPriorPurchaseOrderApprovedResponse
    {
        public List<NewPriorPurchaseOrderResponse> PurchaseOrders { get; set; } = new();
    }
    public class NewPriorPurchaseOrderClosedResponse
    {
        public List<NewPriorPurchaseOrderResponse> PurchaseOrders { get; set; } = new();
    }
    public class NewPriorPurchaseOrderResponse
    {
        public Guid PurchaseOrderId { get; set; }
        public Guid MWOId { get; set; }

        public string MWOName { get; set; } = string.Empty;
        public string CECName { get; set; } = string.Empty;
        public Guid MainBudgetItemId { get; set; }
        public Guid? SupplierId => Supplier == null ? Guid.Empty : Supplier.SupplierId;
        public NewSupplierResponse? Supplier { get; set; } = null!;

        public string SupplierNickName => Supplier == null ? string.Empty : Supplier.NickName;
        public string SupplierName => Supplier == null ? string.Empty : Supplier.Name;
        public string SupplierVendorCode => Supplier == null ? string.Empty : Supplier.VendorCode;
        public List<NewPriorPurchaseOrderItemResponse> PurchaseOrderItems { get; set; } = new List<NewPriorPurchaseOrderItemResponse>();


        public string QuoteNo { get; set; } = "";
        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum Currency { get; set; } = CurrencyEnum.None;
        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;
        public string PurchaseRequisition { get; set; } = "";
        public string CreatedOn => CreatedDate.ToString("d");
        public string ExpectedOn => POExpectedDateDate == null ? string.Empty : POExpectedDateDate!.Value.ToString("d");
        public bool DueDate => POExpectedDateDate == null ? false : DateTime.Now > POExpectedDateDate.Value;
        public string ClosedOn => POClosedDate == null ? string.Empty : POClosedDate!.Value.ToString("d");
        public DateTime CreatedDate { get; set; }
        public DateTime? POApprovedDate { get; set; }
        public DateTime? POExpectedDateDate { get; set; }
        public DateTime? POClosedDate { get; set; }
        public string PurchaseOrderNumber { get; set; } = "";
        public string SPL { get; set; } = "";
        public string TaxCode { get; set; } = "";
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public DateTime CurrencyDate { get; set; }
        public string AccountAssigment { get; set; } = "";
        public string PurchaseorderName { get; set; } = string.Empty;
        public bool IsAlteration { get; set; } = false;
        public bool IsCapitalizedSalary { get; set; } = false;

        public string PurchaseOrderLegendToDelete => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ?
          PurchaseRequisition : PurchaseOrderNumber;

        public bool IsTaxEditable { get; set; } = false;

        public double ActualCurrency => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.ActualCurrency);

        public double ActualUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.ActualUSD);

        public double AssignedUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.AssignedUSD);

        public double ApprovedUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.ApprovedUSD);

        public double PotentialCommitmentUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.PotentialCommitmentUSD);


        public double CommitmentUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.CommitmentUSD);

        public double QuoteValueCurrency => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.QuoteValueCurrency);
    }
}
