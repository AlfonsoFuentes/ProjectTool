using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;

namespace Shared.Models.BudgetItems
{
    public class ListBudgetItemResponse
    {
        public List<BudgetItemResponse> BudgetItems { get; set; } = new();
        public List<BudgetItemResponse> BudgetItemsEngineeringCost => BudgetItems.Where(x => (x.Type.Id == BudgetItemTypeEnum.Engineering.Id ||
        x.Type.Id == BudgetItemTypeEnum.Contingency.Id) && x.Percentage > 0).ToList();
        public MWOResponse MWO { get; set; } = new();

        public double Capital => BudgetItems.Where(x => x.Type.Id != BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget);
        public double Expenses => BudgetItems.Where(x => x.Type.Id == BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget);
        public double EngineeringCost => BudgetItemsEngineeringCost.Sum(x => x.Budget);
        public double Appropiation => Capital + Expenses;
        public double Budget => Capital - EngineeringCost;

        public double SumEngCostPercentage => BudgetItemsEngineeringCost.Sum(x => x.Percentage);

        public double SumBudgetTaxes {  get; set; }
        public void ChangePercentage(BudgetItemResponse item, string Percentagestring)
        {
            item.ValidationErrors.Clear();
            double percentage = 0;
            if (!double.TryParse(Percentagestring, out percentage))
                return;

            item.Percentage = percentage;

            if (item.Type.Id == BudgetItemTypeEnum.Taxes.Id)
            {
                item.Budget = SumBudgetTaxes * item.Percentage / 100.0;
            }
            else
            {
                item.Budget = Budget * item.Percentage / (100 - SumEngCostPercentage);
            }

        }
        public void ChangeUnitaryCost(BudgetItemResponse item,string UnitaryCoststring)
        {
            item.ValidationErrors.Clear();
            double unitarycost = 0;
            if (!double.TryParse(UnitaryCoststring, out unitarycost))
                return;

            item.UnitaryCost = unitarycost;
            item.Budget = item.Quantity * item.UnitaryCost;
            CalculateEngineeringCost();
        }
        public void ChangeQuantity(BudgetItemResponse item, string quantitystring)
        {
            item.ValidationErrors.Clear();
            double quantity = 0;
            if (!double.TryParse(quantitystring, out quantity))
                return;

            item.Quantity = quantity;
            item.Budget = item.Quantity * item.UnitaryCost;
            CalculateEngineeringCost();
        }
        void CalculateEngineeringCost()
        {
            foreach(var row in BudgetItemsEngineeringCost)
            {
                row.Budget = row.Percentage * Budget / (100 - SumEngCostPercentage);
            }
        }
    }
}
