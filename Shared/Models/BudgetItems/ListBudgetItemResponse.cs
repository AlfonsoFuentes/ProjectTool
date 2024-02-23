using Shared.Models.MWO;

namespace Shared.Models.BudgetItems
{
    public class ListBudgetItemResponse
    {
        public List<BudgetItemResponse> BudgetItems { get; set; } = new();
        public MWOResponse MWO { get; set; } = new();

        public double Capital { get; set; }
        public double Expenses { get; set; }
        public double Appropiation => Capital + Expenses;

    }
}
