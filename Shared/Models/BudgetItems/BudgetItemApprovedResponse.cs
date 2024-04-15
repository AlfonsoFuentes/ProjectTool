using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;

namespace Shared.Models.BudgetItems
{
    public class BudgetItemApprovedResponse
    {
        public int Order { get; set; }
        public Guid BudgetItemId { get; set; }
        public Guid MWOId { get; set; }
        public string MWOName { get; set; } = string.Empty;

        public string CostCenter { get; set; } = string.Empty;
        public string MWOCECName { get; set; } = string.Empty;

        public string Nomenclatore => $"{Type.Letter}{Order}";
        public string Name { get; set; } = string.Empty;
        public string ComposedName => (Type.Id == BudgetItemTypeEnum.Equipments.Id || Type.Id == BudgetItemTypeEnum.Instruments.Id) ?
                Brand != string.Empty ? $"{Name} {Brand} Qty: {Quantity}" : $"{Name} Qty: {Quantity}" :
                Type.Id == BudgetItemTypeEnum.Contingency.Id ? $"{Name} {Percentage}%" :
                Type.Id == BudgetItemTypeEnum.Engineering.Id ? Percentage > 0 ? $"{Name} {Percentage}%" :
                Name :
                Type.Id == BudgetItemTypeEnum.Taxes.Id ? $"{Name} {Percentage}%" :
                Name;

     
        public double Quantity { get; set; }

        public List<PurchaseOrderResponse> PurchaseOrders { get; set; } = new();
        public bool HasPurchaseOrders => PurchaseOrders.Count > 0;
        public string NomenclatoreName => $"{Nomenclatore} - {Name}";
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;

        public bool IsMainItemTaxesNoProductive { get; set; }
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

        public bool CanCreatePurchaseOrder => !IsTaxesMainTaxesData;
        public bool IsAlteration => Type.Id == BudgetItemTypeEnum.Alterations.Id;
        public bool IsEngineeringData => Type.Id == BudgetItemTypeEnum.Engineering.Id && Percentage > 0;
        public bool IsTaxesMainTaxesData => Type.Id == BudgetItemTypeEnum.Taxes.Id && IsMainItemTaxesNoProductive;
        public bool IsTaxesNotMainTaxesData => !IsTaxesMainTaxesData;
        public bool CreateNormalPurchaseOrder => IsRegularData;
        public bool CreateTaxPurchaseOrder => Type.Id == BudgetItemTypeEnum.Taxes.Id && !IsMainItemTaxesNoProductive;
        public bool CreateCapitalizedSalaries => IsEngineeringData;
        public double Budget { get; set; }

       
        public double Assigned => PurchaseOrders.Count == 0 ? 0 : PurchaseOrders.Sum(x => x.GetAssignedByItem(BudgetItemId));
        public double Potencial => PurchaseOrders.Count == 0 ? 0 : PurchaseOrders.Sum(x => x.GetPotentialByItem(BudgetItemId));
        public double Actual => PurchaseOrders.Count == 0 ? 0 : PurchaseOrders.Sum(x => x.GetActualByItem(BudgetItemId));
        public double Commitment => Assigned - Actual - Potencial;
        public double Pending => Budget - Assigned ;
        public double Percentage { get; set; }


        public string? Brand { get; set; }


        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;


    }
}
