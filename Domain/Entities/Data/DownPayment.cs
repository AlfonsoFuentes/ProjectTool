namespace Domain.Entities.Data
{
    public class DownPayment : BaseEntity, ITenantEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public static DownPayment Create(Guid purchaseorderid)
        {
            DownPayment item = new DownPayment();
            item.PurchaseOrderId = purchaseorderid;
            item.Id = Guid.NewGuid();

            return item;
        }
        public Guid PurchaseOrderId { get; private set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
        public bool Approved { get; set; }

        public DateTime? RequestDate { get; set; }
        public string? ManagerEmail { get; set; }
        public string? CBSRequesText { get; set; }
        public string? CBSRequesNo { get; set; }
        public string? ProformaInvoice { get; set; }
        public int DownpaymentStatus { get; set; }
        public string? Payterms { get; set; }
        public DateTime? DownPaymentDueDate { get; set; }
        public DateTime? DeliveryDueDate { get; set; }
        public DateTime? RealDate { get; set; }
        public double Percentage { get; set; }
        public double DownPaymentAmount { get; set; }
        public string? DownpaymentJustification { get; set; }
        public string? Incotherm { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public string? DownpaymentName { get; set; }

    }
}
