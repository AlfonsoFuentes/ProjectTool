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
        public double Budget { get; set; }
        public double Assigned => BudgetItems.Count == 0 ? 0 : BudgetItems.Sum(b => b.Assigned);
        public double PotencialAssigned => BudgetItems.Count == 0 ? 0 : BudgetItems.Sum(b => b.PotencialAssigned);
        public double Actual => BudgetItems.Count == 0 ? 0 : BudgetItems.Sum(b => b.Actual);
        public double Commitment => Assigned - Actual;
        public double Pending => Assigned - Actual - PotencialAssigned - Commitment;

    }
}
