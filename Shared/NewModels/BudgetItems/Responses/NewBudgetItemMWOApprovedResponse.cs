

using Shared.Enums.CostCenter;
using Shared.Enums.MWOStatus;
using Shared.NewModels.PurchaseOrders.Responses;

namespace Shared.NewModels.BudgetItems.Responses
{
    public class NewBudgetItemMWOApprovedResponse
    {
        public Guid BudgetItemId { get; set; }
       
        public Guid MWOId { get; set; }
        public string MWOName { get; set; } = string.Empty;
        public string MWOCECName { get; set; } = string.Empty;
        public CostCenterEnum MWOCostCenter { get; set; } = CostCenterEnum.None;
        public MWOTypeEnum MWOType {  get; set; } = MWOTypeEnum.None;
        public FocusEnum MWOFocus {  get; set; } = FocusEnum.None;
        public MWOStatusEnum MWOStatus { get; set; } = MWOStatusEnum.None;
        public bool MWOIsAssetProductive {  get; set; } = false;
        public string Name { get; set; } = string.Empty;
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;
        public double UnitaryCostUSD { get; set; }

        public int Order { get; set; }
        public bool Existing { get; set; }
        public double Quantity { get; set; }
        public Guid? BrandId => Brand == null ? Guid.Empty : Brand.BrandId;
        public NewBrandResponse? Brand { get; set; } = null!;
        public string BrandName => Brand == null ? string.Empty : Brand.Name;
        public bool IsMainItemTaxesNoProductive { get; set; }
        public bool IsEngineeringItem { get; set; }
        public string? Model { get; set; } = string.Empty;
        public string? Reference { get; set; } = string.Empty;
        public double Percentage { get; set; }
        public bool IsNotAbleToEditDelete { get; set; }
        public List<NewPriorPurchaseOrderItemResponse> PurchaseOrderItems { get; set; } = new List<NewPriorPurchaseOrderItemResponse>();
        public bool IsTaxesMainTaxesData => Type.Id == BudgetItemTypeEnum.Taxes.Id && IsMainItemTaxesNoProductive;
        public bool CanCreatePurchaseOrder => !IsTaxesMainTaxesData;
        public string Nomenclatore => $"{Type.Letter}{Order}";
        public string NomenclatoreName => $"{Nomenclatore}-{Name}";
        public bool IsCapitalizedSalary => Type.Id == BudgetItemTypeEnum.Engineering.Id && IsEngineeringItem;
        public bool IsAlteration => Type.Id == BudgetItemTypeEnum.Alterations.Id;

        
        public double ActualUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.ActualUSD);
        
        public double AssignedUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.AssignedUSD);
        
        public double ApprovedUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.ApprovedUSD);
        
        public double PotentialCommitmentUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.PotentialCommitmentUSD);
        
        public double PendingToCommitUSD => BudgetUSD - AssignedUSD;
        
        public double CommitmentUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.CommitmentUSD);
        
        public double BudgetUSD => UnitaryCostUSD * Quantity;
    }
}
