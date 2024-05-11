using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Data
{
    public class PurchaseOrder : BaseEntity, ITenantEntity
    {
        public Guid MWOId { get; set; }
        public MWO MWO { get; set; } = null!;
        public string TenantId { get; set; } = string.Empty;
        public Guid MainBudgetItemId { get; set; }
        public Guid? SupplierId { get; set; }
        public Supplier? Supplier { get; set; } = null!;
        public ICollection<DownPayment> DownPayments { get; set; } = new List<DownPayment>();
        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
        
        public static PurchaseOrder Create(Guid mwoid)
        {
            PurchaseOrder item = new PurchaseOrder();
            item.MWOId = mwoid;
            item.Id = Guid.NewGuid();

            return item;
        }
        
        public PurchaseOrderItem AddPurchaseOrderItem(Guid mwobudgetitemid)
        {
            var row = PurchaseOrderItem.Create(Id, mwobudgetitemid);
            return row;
        }
        public PurchaseOrderItem AddPurchaseOrderItemForNoProductiveTax(Guid mwobudgetitemid)
        {
            var row = PurchaseOrderItem.Create(Id, mwobudgetitemid);
            row.IsTaxNoProductive = true;
            return row;
        }
        public PurchaseOrderItem AddPurchaseOrderItemForAlteration(Guid mwobudgetitemid)
        {
            var row = PurchaseOrderItem.Create(Id, mwobudgetitemid);
            row.IsTaxAlteration = true;
            return row;
        }
        public string QuoteNo { get; set; } = "";
        public int QuoteCurrency { get; set; }
        public int PurchaseOrderCurrency { get; set; }
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
        [NotMapped]
        public double ActualCurrency => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.POItemActualCurrency);
        [NotMapped]
        public double ActualUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.ActualUSD);
       
        [NotMapped]
        public double AssignedUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.POItemValueUSD);
        [NotMapped]
        public double ApprovedUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.ApprovedUSD);
        [NotMapped]
        public double PotentialCommitmentUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.PotentialCommitmentUSD);
        
        [NotMapped]
        public double PendingToReceiveUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.CommitmentUSD);
        [NotMapped]
        public double QuoteValueCurrency => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.POItemQuoteValueCurrency);
    }
}
