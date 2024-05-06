

namespace Shared.NewModels.BudgetItems.Responses
{
    public class NewBudgetItemMWOCreatedResponse
    {

        public Guid BudgetItemId { get; set; }
        public Guid MWOId { get; set; }
        public string MWOName { get; set; } = string.Empty;
        public string ComposedName => (Type.Id == BudgetItemTypeEnum.Equipments.Id || Type.Id == BudgetItemTypeEnum.Instruments.Id) ?
              Brand != null ? $"{Name} {Brand.Name} Qty: {Quantity}" : $"{Name} Qty: {Quantity}" :
              Type.Id == BudgetItemTypeEnum.Contingency.Id ? $"{Name} {Percentage}%" :
              Type.Id == BudgetItemTypeEnum.Engineering.Id ? Percentage > 0 ? $"{Name} {Percentage}%" :
              Name :
              Type.Id == BudgetItemTypeEnum.Taxes.Id ? $"{Name} {Percentage}%" :
              Name;
        public string Name { get; set; } = string.Empty;
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;
        public double UnitaryCost { get; set; }
        public double Budget => UnitaryCost * Quantity;
        public int Order { get; set; }
        public bool Existing { get; set; }
        public double Quantity { get; set; }
        public Guid? BrandId => Brand == null ? null : Brand.BrandId;
        public NewBrandResponse? Brand { get; set; } = null!;
        public string BrandName => Brand == null ? string.Empty : Brand.Name;
        public bool IsMainItemTaxesNoProductive { get; set; }
        public bool IsNotAbleToEditDelete { get; set; }
        public string Nomeclatore => $"{Type.Letter}{Order}";
        public bool IsAlteration => Type.Id == BudgetItemTypeEnum.Alterations.Id;
        public double Percentage {  get; set; }
        public bool IsEngineeringData => Type.Id == BudgetItemTypeEnum.Engineering.Id;
        public bool IsContingencyData => Type.Id == BudgetItemTypeEnum.Contingency.Id;
        public bool IsTaxesData => Type.Id == BudgetItemTypeEnum.Taxes.Id;
        public bool IsEngContData => IsContingencyData || IsEngineeringData;
        public bool IsEngineeringBudget => Type.Id == BudgetItemTypeEnum.Engineering.Id && !IsNotAbleToEditDelete;
        public bool IsAssetProductive { get; set; }
        public bool MustUpdateEngineeringItems =>
           IsAlteration ? false :
           IsEngContData ? IsNotAbleToEditDelete ? false : true : true;
        public bool MustUpdateTaxesNotProductive => !IsAssetProductive && !IsAlteration;

    }
}
