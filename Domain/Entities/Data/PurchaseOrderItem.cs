using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Data
{
    public class PurchaseOrderItem : BaseEntity, ITenantEntity
    {
        public Guid BudgetItemId { get; private set; }
        public BudgetItem BudgetItem { get; set; } = null!;
        public Guid PurchaseOrderId { get; private set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
        public string TenantId { get; set; } = string.Empty;
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

        public double UnitaryValueCurrency { get; set; }
    
        public double ActualCurrency { get; set; }
        public double Quantity { get; set; }

        public bool IsTaxNoProductive { get; set; } = false;
        public bool IsTaxAlteration { get; set; } = false;
        
    }
}
