using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Responses;

namespace Shared.Models.BudgetItems
{
    public class BudgetItemResponse
    {

        public List<string> ValidationErrors { get; set; } = new();
        public MWOResponse MWO { get; set; } = new MWOResponse();
        public Guid Id { get; set; }
        public string Nomenclatore { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ComposedName => (Type.Id == BudgetItemTypeEnum.Equipments.Id || Type.Id == BudgetItemTypeEnum.Instruments.Id) ?
                Brand != string.Empty ? $"{Name} {Brand} Qty: {Quantity}" : $"{Name} Qty: {Quantity}" :
                Type.Id == BudgetItemTypeEnum.Contingency.Id ? $"{Name} {Percentage}%" :
                Type.Id == BudgetItemTypeEnum.Engineering.Id ? Percentage > 0 ? $"{Name} {Percentage}%" :
                Name :
                Type.Id == BudgetItemTypeEnum.Taxes.Id ? $"{Name} {Percentage}%" :
                Name;

        public List<PurchaseOrderItemForBudgetItemResponse> PurchaseOrders { get; set; } = new();
        public string NomenclatoreName => $"{Nomenclatore} - {Name}";
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;
        public bool IsNotAbleToEditDelete { get; set; }
        public bool IsMainItemTaxesNoProductive { get; set; }

        public bool IsEngineeringData => Type.Id == BudgetItemTypeEnum.Engineering.Id;
        public bool IsContingencyData => Type.Id == BudgetItemTypeEnum.Contingency.Id;
        public bool IsTaxesData => Type.Id == BudgetItemTypeEnum.Taxes.Id;
        public bool IsEngContData => IsContingencyData || IsEngineeringData;
        public bool IsEngineeringBudget => Type.Id == BudgetItemTypeEnum.Engineering.Id && !IsNotAbleToEditDelete;
        public double Budget { get; set; }

        public double SumBudgetForTaxes => SelectedBudgetItemForTaxes.Sum(x => x.Budget);
        public int Order { get; set; }
        public bool Existing { get; set; }
        public double Quantity { get; set; } = 1;

        public double UnitaryCost { get; set; }

        public string? Brand { get; set; }

        public string? Model { get; set; } = string.Empty;
        public string? Reference { get; set; } = string.Empty;
        public double Percentage { get; set; }


        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;

        public List<BudgetItemDto> SelectedBudgetItemForTaxes { get; set; } = new List<BudgetItemDto>();
    }
}
