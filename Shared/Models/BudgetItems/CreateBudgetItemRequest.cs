using Shared.Models.Brands;
using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;
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
        public CreateBudgetItemRequestDto ConvertTodDto()
        {
            return new()
            {
                IsRegularData = IsRegularData,
                IsEquipmentData = IsEquipmentData,
                IsEngineeringData = IsEngineeringData,
                BrandId = Brand!.Id,
                Budget = Budget,
                BudgetItemDtos = BudgetItemDtos,
                Existing = Existing,
                IsAlteration = IsAlteration,
                IsContingencyData = IsContingencyData,
                IsEngContData = IsEngContData,
                IsTaxesData = IsTaxesData,
                Model = Model,
                MWOId = MWO.Id,
                Name = Name,
                Percentage = Percentage,
                Quantity = Quantity,
                Reference = Reference,
                SumBudgetItems = SumBudgetItems,
                SumPercentage = SumPercentage,
                SumTaxItems = SumTaxItems,
                TaxesNoProductive = TaxesNoProductive,
                Type = Type.Id,
                UnitaryCost = UnitaryCost,


            };
        }
        public Func<Task<bool>> Validator { get; set; } = null!;
        public MWOResponse MWO { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;

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
        public async Task ChangeName(string name)
        {
            Name = name;
            if (Validator != null) await Validator();

        }
        public async Task ChangeQuantity(string stringquantity)
        {

            double quantity = 0;
            if (!double.TryParse(stringquantity, out quantity))
            {

            }

            if (IsRegularData || IsEquipmentData || IsAlteration)
            {
                Quantity = quantity;
                Budget = Quantity * UnitaryCost;
            }
            if (Validator != null) await Validator();
        }
        public async Task ChangeUnitaryCost(string stringunitaryCost)
        {

            double unitarycost = 0;
            if (!double.TryParse(stringunitaryCost, out unitarycost))
            {

            }


            if (IsRegularData || IsEquipmentData || IsAlteration)
            {
                UnitaryCost = unitarycost;
                Budget = Quantity * UnitaryCost;
            }
            if (Validator != null) await Validator();
        }
        public async Task ChangeTaxesItemList(object objeto)
        {

            Budget = Math.Round(SumBudgetTaxes * Percentage / 100.0, 2);
            if (Validator != null) await Validator();
        }
        public async Task ChangePercentage(string stringpercentage)
        {

            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
            {

            }
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
            if (Validator != null) await Validator();
        }
        public async Task ChangeBudget(string unitarycoststring)
        {

            double unitarycost = 0;
            if (!double.TryParse(unitarycoststring, out unitarycost))
            {

            }
            if (IsEngineeringData)
            {
                Percentage = 0;
                UnitaryCost = unitarycost;
                Quantity = 1;
                Budget = UnitaryCost * Quantity;
            }
            if (Validator != null) await Validator();


        }
    }


}
