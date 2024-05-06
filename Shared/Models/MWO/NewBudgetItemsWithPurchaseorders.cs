using Shared.Enums.BudgetItemTypes;
using Shared.Models.PurchaseOrders.Responses;

namespace Shared.Models.MWO
{
    public class NewBudgetItemsWithPurchaseorders
    {
        public List<NewPurchaseOrderItemResponse> PurchaseOrderItems { get; set; } = new();

        public Guid BudgetItemId { get; set; }
        public Guid MWOId { get; set; }
        public string Name { get; set; } = string.Empty;
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;
     
        public double BudgetUSD { get; set; }
        public string Nomenclatore {  get; set; }=string.Empty;
        public bool IsMainItemTaxesNoProductive { get; set; }
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

        public bool HasPurchaseOrders => PurchaseOrderItems.Count > 0;
        public string NomenclatoreName => $"{Nomenclatore} - {Name}";

        public bool IsRegularData => Type.Id == BudgetItemTypeEnum.EHS.Id
            || Type.Id == BudgetItemTypeEnum.Alterations.Id
           || Type.Id == BudgetItemTypeEnum.Structural.Id
           || Type.Id == BudgetItemTypeEnum.Foundations.Id
           || Type.Id == BudgetItemTypeEnum.Electrical.Id
           || Type.Id == BudgetItemTypeEnum.Piping.Id
           || Type.Id == BudgetItemTypeEnum.Insulations.Id
           || Type.Id == BudgetItemTypeEnum.Testing.Id
           || Type.Id == BudgetItemTypeEnum.Painting.Id
           || Type.Id == BudgetItemTypeEnum.Contingency.Id
           || !IsEngineeringData;

        public bool IsMWOAssetProductive { get; set; }
        public double Percentage { get; set; }
        public bool CanCreatePurchaseOrder => !IsTaxesMainTaxesData;
  
        public bool IsEngineeringData => Type.Id == BudgetItemTypeEnum.Engineering.Id && Percentage > 0;
        public bool IsTaxesMainTaxesData => Type.Id == BudgetItemTypeEnum.Taxes.Id && IsMainItemTaxesNoProductive;
        public bool IsTaxesNotMainTaxesData => !IsTaxesMainTaxesData;
        public bool CreateNormalPurchaseOrder => IsRegularData;
        public bool CreateTaxPurchaseOrder => Type.Id == BudgetItemTypeEnum.Taxes.Id && !IsMainItemTaxesNoProductive;
        public bool CreateCapitalizedSalaries => IsEngineeringData;
    }
}
