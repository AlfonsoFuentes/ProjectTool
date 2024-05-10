using Shared.Enums.BudgetItemTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Data
{
    public class BudgetItem : BaseEntity, ITenantEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public MWO MWO { get; set; } = null!;
        public Guid MWOId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MWOName => MWO == null ? string.Empty : MWO.Name;
        public string MWOCECName => MWO == null ? string.Empty : MWO.CECName;
        public string MWOCostCenter => MWO == null ? string.Empty : MWO.CostCenterName;
        public int Type { get; set; } = 0;
        public double UnitaryCost { get; set; }

        public int Order { get; set; }
        public bool Existing { get; set; }
        public double Quantity { get; set; }
        public Guid? BrandId { get; set; }
        public Brand? Brand { get; set; } = null!;
        public bool IsMainItemTaxesNoProductive { get; set; }
        public bool IsEngineeringItem { get; set; }
        public string? Model { get; set; } = string.Empty;
        public string? Reference { get; set; } = string.Empty;
        public double Percentage { get; set; }
        public bool IsNotAbleToEditDelete { get; set; }
        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
        public ICollection<TaxesItem> TaxesItems { get; set; } = new List<TaxesItem>();
        public ICollection<TaxesItem> Selecteds { get; set; } = new List<TaxesItem>();

        public TaxesItem AddTaxItem(Guid SelectedItemId)
        {
            TaxesItem result = new TaxesItem();
            result.Id = Guid.NewGuid();
            result.BudgetItemId = Id;
            result.SelectedId = SelectedItemId;
            TaxesItems.Add(result);
            return result;
        }

        [NotMapped]
        public string Nomeclatore => $"{BudgetItemTypeEnum.GetLetter(Type)}{Order}";
        [NotMapped]
        public bool IsAlteration => Type == BudgetItemTypeEnum.Alterations.Id;

        [NotMapped]
        public double ActualUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.ActualUSD);
        [NotMapped]
        public double AssignedUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.AssignedUSD);
        [NotMapped]
        public double ApprovedUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.ApprovedUSD);
        [NotMapped]
        public double PotentialCommitmentUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.PotentialCommitmentUSD);
        [NotMapped]
        public double PendingToCommitUSD => Budget - AssignedUSD;
        [NotMapped]
        public double CommitmentUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.CommitmentUSD);
        [NotMapped]
        public double Budget => UnitaryCost * Quantity;
    }
}
