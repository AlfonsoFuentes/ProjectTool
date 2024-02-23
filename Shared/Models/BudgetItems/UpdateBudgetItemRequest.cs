using Shared.Models.Brands;
using Shared.Models.BudgetItemTypes;

namespace Shared.Models.BudgetItems
{
    public class UpdateBudgetItemRequest
    {
        public Guid MWOId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MWOName { get; set; } = string.Empty;
        public string Nomenclatore {  get; set; } = string.Empty;
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;
        public double UnitaryCost { get; set; }
        public double Budget { get; set; }

        public bool Existing { get; set; }
        public double Quantity { get; set; }

        public string? Model { get; set; } = string.Empty;
        public string? Reference { get; set; } = string.Empty;
        public double Percentage { get; set; }
        public List<BudgetItemDto> SelectedBudgetItemDtos { get; set; } = new List<BudgetItemDto>();
        public List<BudgetItemDto> BudgetItemDtos { get; set; } = new List<BudgetItemDto>();
        public List<Guid> SelectedIdBudgetItemDtos { get; set; } = new List<Guid>();

        public double SumBudgetTaxes => Math.Round(SelectedBudgetItemDtos.Sum(x => x.Budget), 2);
        public List<string> ValidationErrors { get; set; } = new();

        public bool IsRegularData => Type.Id == BudgetItemTypeEnum.EHS.Id
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
        public double SumPercentage { get; set; }
        public BrandResponse? Brand { get; set; }
        public void ChangeQuantity(string stringquantity)
        {
            ValidationErrors.Clear();
            double quantity = 0;
            if (!double.TryParse(stringquantity, out quantity))
                return;

            if (IsRegularData || IsEquipmentData || IsAlteration)
            {
                Quantity = quantity;
                Budget = Quantity * UnitaryCost;
            }
        }
        public void ChangeUnitaryCost(string stringunitaryCost)
        {
            ValidationErrors.Clear();
            double unitarycost = 0;
            if (!double.TryParse(stringunitaryCost, out unitarycost))
                return;


            if (IsRegularData || IsEquipmentData || IsAlteration)
            {
                UnitaryCost = unitarycost;
                Budget = Quantity * UnitaryCost;
            }
        }
        public void ChangeTaxesItemList(object objeto)
        {
            ValidationErrors.Clear();
            SelectedBudgetItemDtos.Clear();
            foreach (var item in SelectedIdBudgetItemDtos)
            {
                SelectedBudgetItemDtos.Add(BudgetItemDtos.Single(x => x.Id == item));
            }
            Budget = Math.Round(SumBudgetTaxes * Percentage / 100.0, 2);
        }
        public double SumBudgetItems { get; set; }
        public void ChangePercentage(string stringpercentage)
        {
            ValidationErrors.Clear();
            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
                return;
            if (IsEngContData)
            {
                SumPercentage -= Percentage;
                Percentage = percentage;
                SumPercentage += Percentage;
                Budget = Math.Round(SumBudgetItems * Percentage / (100 - SumPercentage), 2);
            }
            if (IsTaxesData)
            {
                Percentage = percentage;
                Budget = Math.Round(SumBudgetTaxes * Percentage / 100, 2);
            }

        }
        public void ChangeBudget(string stringbudget)
        {
            ValidationErrors.Clear();
            double budget = 0;
            if (!double.TryParse(stringbudget, out budget))
                return;
            if (IsEngineeringData)
            {
                Percentage = 0;
                Budget = budget;
            }



        }
        public void ChangeName(string name)
        {
            Name = name;
            ValidationErrors.Clear();

        }
    }
}
