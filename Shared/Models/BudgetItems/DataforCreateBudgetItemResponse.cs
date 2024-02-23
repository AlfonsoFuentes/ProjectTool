namespace Shared.Models.BudgetItems
{
    public class DataforCreateBudgetItemResponse
    {
        public List<BudgetItemDto> BudgetItems { get; set; } = new();
        public string MWOName { get; set; } = string.Empty;

        public double SumBudget { get; set; }
        public double SumEngContPercentage { get; set; }
    }
}
