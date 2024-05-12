

using Shared.NewModels.PurchaseOrders.Responses;

namespace Shared.NewModels.BudgetItems.Responses
{
    public class NewBudgetItemToCreatePurchaseOrderResponse
    {
        public Guid BudgetItemId { get; set; }

        public Guid MWOId { get; set; }
        public string MWOName { get; set; } = string.Empty;
        public string MWOCECName { get; set; } = string.Empty;
        public string MWOCostCenter { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;
        public double UnitaryCostUSD { get; set; }
        public int Order { get; set; }
        public string Nomenclatore => $"{Type.Letter}{Order}";
        public string NomenclatoreName => $"{Nomenclatore}-{Name}";
        public double Quantity { get; set; }
        public bool IsMainItemTaxesNoProductive { get; set; }
        public bool IsEngineeringItem { get; set; }
        public bool IsNotAbleToEditDelete { get; set; }
        public List<NewPriorPurchaseOrderItemResponse> PurchaseOrderItems { get; set; } = new List<NewPriorPurchaseOrderItemResponse>();
        public bool IsTaxes => Type.Id == BudgetItemTypeEnum.Taxes.Id;
        public bool IsTaxesMainTaxesData => Type.Id == BudgetItemTypeEnum.Taxes.Id && IsMainItemTaxesNoProductive;
        public bool CanCreatePurchaseOrder => !IsTaxesMainTaxesData;
        public string Nomeclatore => $"{Type.Letter}{Order}";
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

        public double PendingToReceiveUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.CommitmentUSD);

        public double BudgetUSD => UnitaryCostUSD * Quantity;
    }
}
