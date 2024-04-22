using Shared.Models.Suppliers;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class NewPurchaseOrderResponse
    {
        public List<NewPurchaseOrderItemResponse> PurchaseOrderItems { get; set; } = new();
        public Guid MWOId { get; set; }

        public Guid MainBudgetItemId { get; set; }
        public Guid? SupplierId { get; set; }
        public SupplierResponse? Supplier { get; set; } = null!;
        public string QuoteNo { get; set; } = "";
        public int QuoteCurrency { get; set; }
        public int Currency { get; set; }
        public int PurchaseOrderStatus { get; set; }
        public string PurchaseRequisition { get; set; } = "";
        public DateTime? POApprovedDate { get; set; }
        public DateTime? POExpectedDateDate { get; set; }
        public DateTime? POClosedDate { get; set; }
        public string PONumber { get; set; } = "";
        public string SPL { get; set; } = "";
        public string TaxCode { get; set; } = "";
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public DateTime CurrencyDate { get; set; }
        public string AccountAssigment { get; set; } = "";

        public string PurchaseorderName { get; set; } = string.Empty;

        public bool IsAlteration { get; set; } = false;
        public bool IsCapitalizedSalary { get; set; } = false;

        public bool IsTaxEditable { get; set; } = false;

        public double ActualUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.ActualUSD);

        public double AssignedUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.AssignedUSD);

        public double ApprovedUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.ApprovedUSD);

        public double PotentialCommitmentUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.PotentialCommitmentUSD);


        public double PendingToReceiveUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.PendingToReceiveUSD);
    }
}
