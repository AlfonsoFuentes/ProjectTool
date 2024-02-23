namespace Domain.Entities.Data
{
    public class PurchaseOrder : BaseEntity
    {
        public Guid MWOId { get; set; }
        public MWO MWO { get; set; } = null!;

        public Guid MainBudgetItemId { get; set; }
        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;
        public ICollection<DownPayment> DownPayments { get; set; } = new List<DownPayment>();
        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
        public static PurchaseOrder Create(Guid mwoid)
        {
            PurchaseOrder item = new PurchaseOrder();
            item.MWOId = mwoid;
            item.Id = Guid.NewGuid();

            return item;
        }
        public PurchaseOrderItem AddPurchaseOrderItem(Guid mwobudgetitemid, string name)
        {
            var row = PurchaseOrderItem.Create(Id, mwobudgetitemid);
            row.Name = name;
         
            return row;
        }
        public PurchaseOrderItem AddPurchaseOrderItemForNoProductiveTax(Guid mwobudgetitemid, string name)
        {
            var row = PurchaseOrderItem.Create(Id, mwobudgetitemid);
            row.Name = name;
            row.IsTaxNoProductive = true;
            return row;
        }
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
        public bool IsAlteration { get; set; } = false;
        public double POValueUSD { get; set; }
        public string PurchaseorderName { get; set; } = string.Empty;
        public bool IsDiscountApplied { get; set; }
        public double DiscountPercentage { get; set; }
        public double Actual { get; set; }
        

    }
}
