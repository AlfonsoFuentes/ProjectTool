using Shared.Models.Brands;
using Shared.Models.BudgetItemTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.BudgetItems
{
    public class CreateBudgetItemRequest
    {
        public CreateBudgetItemRequest()
        {

        }
        public List<string> ValidationErrors { get; set; } = new();
        public Guid MWOId { get; set; }
        public string Name { get; set; } = string.Empty;
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;
        public string MWOName { get; set; } = string.Empty;
        public bool Existing { get; set; }
        public bool IsRegularData => Type == BudgetItemTypeEnum.EHS
            || Type == BudgetItemTypeEnum.Structural
            || Type == BudgetItemTypeEnum.Foundations
            || Type == BudgetItemTypeEnum.Electrical
            || Type == BudgetItemTypeEnum.Piping
            || Type == BudgetItemTypeEnum.Insulations
            || Type == BudgetItemTypeEnum.Testing

            || Type == BudgetItemTypeEnum.Painting;
        public bool IsEquipmentData => Type == BudgetItemTypeEnum.Equipments
           || Type == BudgetItemTypeEnum.Instruments;
        public bool IsEngineeringData => Type == BudgetItemTypeEnum.Engineering;
        public bool IsContingencyData => Type == BudgetItemTypeEnum.Contingency;
        public bool IsTaxesData => Type == BudgetItemTypeEnum.Taxes;
        public bool IsEngContData => IsContingencyData || IsEngineeringData;
        public bool IsAlteration => Type == BudgetItemTypeEnum.Alterations;
        public double UnitaryCost { get; set; }
        public double Budget { get; set; }
        public double Quantity { get; set; } = 1;
        public BrandResponse? Brand { get; set; }
        public string? Model { get; set; } = string.Empty;
        public string? Reference { get; set; } = string.Empty;
        public double SumBudgetItems { get; set; }
        public double SumTaxItems { get; set; }
        public double Percentage { get; set; }
        public double SumPercentage { get; set; }

        public double TaxesNoProductive { get; set; } = 19;
        public List<BudgetItemDto> BudgetItemDtos { get; set; } = new List<BudgetItemDto>();

        public double SumBudgetTaxes => Math.Round(BudgetItemDtos.Sum(x => x.Budget), 2);
        public void ChangeName(string name)
        {
            Name = name;
            ValidationErrors.Clear();

        }
        public void ChangeQuantity(string stringquantity)
        {
            ValidationErrors.Clear();
            double quantity = 0;
            if (!double.TryParse(stringquantity, out quantity))
                return;

            if (IsRegularData || IsEquipmentData||IsAlteration)
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
            Budget = Math.Round(SumBudgetTaxes * Percentage / 100.0, 2);
        }
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
    }


}
