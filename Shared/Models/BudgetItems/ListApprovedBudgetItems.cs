using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;

namespace Shared.Models.BudgetItems
{

    public class ListApprovedBudgetItemsResponse
    {
        public List<BudgetItemApprovedResponse> BudgetItems { get; set; } = new();
        public MWOResponse MWO { get; set; }=new();
       
        public bool HasExpenses=> BudgetItemsAlterations.Count > 0;
        public List<BudgetItemApprovedResponse> BudgetItemsAlterations =>BudgetItems.Where(x => x.Type.Id == BudgetItemTypeEnum.Alterations.Id).ToList();

        public List<BudgetItemApprovedResponse> BudgetItemsCapital => BudgetItems.Where(x => x.Type.Id != BudgetItemTypeEnum.Alterations.Id).ToList();

        public double Expenses => BudgetItemsAlterations.Sum(x => x.Budget);
        public double Capital => BudgetItemsCapital.Sum(x => x.Budget);
        public double Appropiation => Expenses + Capital;


        public double ActualExpenses => BudgetItemsAlterations.Sum(x => x.Actual);
        public double ActualCapital => BudgetItemsCapital.Sum(x => x.Actual);

        public double AssignedExpenses => BudgetItemsAlterations.Sum(x => x.Assigned);
        public double AssignedCapital => BudgetItemsCapital.Sum(x => x.Assigned);

        public double PotencialExpenses => BudgetItemsAlterations.Sum(x => x.Potencial);
        public double PotencialCapital => BudgetItemsCapital.Sum(x => x.Potencial);

        public double CommitmentExpenses => AssignedExpenses - ActualExpenses;
        public double PendingExpenses => Expenses - AssignedExpenses - PotencialExpenses;

        public double CommitmentCapital => AssignedCapital - ActualCapital;
        public double PendingCapital => Capital - AssignedCapital - PotencialCapital;
    }
}
