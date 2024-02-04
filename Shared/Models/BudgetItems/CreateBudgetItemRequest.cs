using Shared.Models.Brands;
using Shared.Models.BudgetItemTypes;
using System;
using System.Collections.Generic;
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
        public Guid MWOId { get; set; }
        public string Name { get; set; } = string.Empty;
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;

        public bool Existing { get; set; }
        public bool IsRegularData => Type == BudgetItemTypeEnum.Alterations
            || Type == BudgetItemTypeEnum.EHS
            || Type == BudgetItemTypeEnum.Structural
            || Type == BudgetItemTypeEnum.Foundations
            || Type == BudgetItemTypeEnum.Electrical
            || Type == BudgetItemTypeEnum.Piping
            || Type == BudgetItemTypeEnum.Insulations
            || Type == BudgetItemTypeEnum.Testing
            || Type == BudgetItemTypeEnum.Alterations
            || Type == BudgetItemTypeEnum.Painting;
        public bool IsEquipmentData => Type == BudgetItemTypeEnum.Equipments
           || Type == BudgetItemTypeEnum.Instruments;
        public bool IsEngContData => Type == BudgetItemTypeEnum.Engineering
           || Type == BudgetItemTypeEnum.Contingency;
        public bool IsTaxesData => Type == BudgetItemTypeEnum.Taxes;

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


        public void ChangeQuantity(double quantity)
        {

            if (IsRegularData || IsEquipmentData)
            {
                Quantity = quantity;
                Budget = Quantity * UnitaryCost;
            }
        }
        public void ChangeUnidatryCost(double unidatryCost)
        {

            if (IsRegularData || IsEquipmentData)
            {
                UnitaryCost = unidatryCost;
                Budget = Quantity * UnitaryCost;
            }
        }
        public void ChangePercentage(double percentage)
        {
            if (IsEngContData)
            {
                SumPercentage -= Percentage;
                Percentage = percentage;
                SumPercentage += Percentage;
                Budget = SumBudgetItems * Percentage / (100-SumPercentage);
            }
            if (IsTaxesData)
            {
                Percentage = percentage;
                Budget = SumTaxItems * Percentage / 100;
            }

        }
    }


}
