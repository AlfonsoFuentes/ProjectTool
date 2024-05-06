namespace Shared.NewModels.BudgetItems.Request
{
    public class NewBudgetItemMWOUpdateRequest
    {
        public Guid BudgetItemId { get; set; }
        public string ComposedName => (Type.Id == BudgetItemTypeEnum.Equipments.Id || Type.Id == BudgetItemTypeEnum.Instruments.Id) ?
              Brand != null ? $"{Name} {Brand.Name} Qty: {Quantity}" : $"{Name} Qty: {Quantity}" :
              Type.Id == BudgetItemTypeEnum.Contingency.Id ? $"{Name} {Percentage}%" :
              Type.Id == BudgetItemTypeEnum.Engineering.Id ? Percentage > 0 ? $"{Name} {Percentage}%" :
              Name :
              Type.Id == BudgetItemTypeEnum.Taxes.Id ? $"{Name} {Percentage}%" :
              Name;
        public Guid MWOId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MWOName { get; set; } = string.Empty;
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;
        public double UnitaryCost { get; set; }
        public double Budget => UnitaryCost * Quantity;
        public int Order { get; set; }
        public bool Existing { get; set; }
        public double Quantity { get; set; }
        public Guid? BrandId => Brand == null ? Guid.Empty : Brand.BrandId;
        public NewBrandResponse? Brand { get; set; } = null!;
        public string? Model { get; set; } = string.Empty;
        public string? Reference { get; set; } = string.Empty;
        public double Percentage { get; set; }
        public string Nomeclatore => $"{Type.Letter}{Order}";
        public bool IsMainItemTaxesNoProductive { get; set; }
        public bool IsNotAbleToEditDelete { get; set; }
        public bool IsEngineeringItem { get; set; }
        public bool IsRegularData =>
               Type.Id == BudgetItemTypeEnum.EHS.Id
            || Type.Id == BudgetItemTypeEnum.Structural.Id
            || Type.Id == BudgetItemTypeEnum.Foundations.Id
            || Type.Id == BudgetItemTypeEnum.Electrical.Id
            || Type.Id == BudgetItemTypeEnum.Piping.Id
            || Type.Id == BudgetItemTypeEnum.Insulations.Id
            || Type.Id == BudgetItemTypeEnum.Testing.Id
            || Type.Id == BudgetItemTypeEnum.Painting.Id;
        public bool IsEquipmentData => Type.Id == BudgetItemTypeEnum.Equipments.Id
           || Type.Id == BudgetItemTypeEnum.Instruments.Id;
        public bool IsEngineeringData => Type.Id == BudgetItemTypeEnum.Engineering.Id;
        public bool IsContingencyData => Type.Id == BudgetItemTypeEnum.Contingency.Id;
        public bool IsTaxesData => Type.Id == BudgetItemTypeEnum.Taxes.Id;
        public bool IsEngContData => IsContingencyData || IsEngineeringData;
        public bool IsAlteration => Type.Id == BudgetItemTypeEnum.Alterations.Id;

        public bool MustUpdateEngineeringItems =>IsAlteration ? false : true;
        public bool MustUpdateTaxesNotProductive => !IsAssetProductive && !IsAlteration;
        public List<NewBudgetItemMWOCreatedResponse> TaxesSelectedItems { get; set; } = new();

        public double TotalTaxesApplied => TaxesSelectedItems.Count == 0 ? 0 : TaxesSelectedItems.Sum(x => x.Budget);
        public bool IsAssetProductive { get; set; }
        public bool CreateTaxItem => GetCreateTaxItem();

        bool GetCreateTaxItem()
        {
            if (IsAssetProductive)
            {
                if (IsAlteration)
                {
                    return false;
                }
                if (IsEngineeringData)
                {
                    if (Budget > 0 && Percentage == 0)
                    {
                        return true;
                    }

                }

            }

            return false;
        }

    }
}
