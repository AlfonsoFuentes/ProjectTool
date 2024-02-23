namespace Domain.Entities.Data
{
    public class PurchaseOrderItem : BaseEntity
    {
        public Guid BudgetItemId { get; private set; }
        public BudgetItem BudgetItem { get; set; } = null!;
        public Guid PurchaseOrderId { get; private set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public bool IsTaxNoProductive {  get;  set; }
        public static PurchaseOrderItem Create(Guid purchasorderid, Guid mwobudgetitemid)
        {
            PurchaseOrderItem item = new PurchaseOrderItem();
            item.Id = Guid.NewGuid();
            item.BudgetItemId = mwobudgetitemid;
            item.PurchaseOrderId = purchasorderid;

            return item;
        }
        public string Name { get; set; } = string.Empty;
      
        public void ChangeBudgetItem(Guid newbudgetitemid)
        {
            BudgetItemId = newbudgetitemid;
        }
        public double POValueUSD { get; set; }

        public void SetApplyDiscount(double discount)
        {
            POValueUSD = POValueUSD * (100 - discount) / 100;
        }
        public double Quantity { get; set; }
        public double Actual {  get; set; }
     
    }
}
