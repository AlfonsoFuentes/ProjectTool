using Shared.Enums.BudgetItemTypes;
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

        public double Expenses => BudgetItemsAlterations.Sum(x => x.BudgetUSD);
        public double Capital => BudgetItemsCapital.Sum(x => x.BudgetUSD);
        public double Appropiation => Expenses + Capital;


        public double ActualExpenses => BudgetItemsAlterations.Sum(x => x.ActualUSD);
        public double ActualCapital => BudgetItemsCapital.Sum(x => x.ActualUSD);

        public double AssignedExpenses => BudgetItemsAlterations.Sum(x => x.AssignedUSD);
        public double AssignedCapital => BudgetItemsCapital.Sum(x => x.AssignedUSD);

        public double PotencialExpenses => BudgetItemsAlterations.Sum(x => x.PotentialUSD);
        public double PotencialCapital => BudgetItemsCapital.Sum(x => x.PotentialUSD);

        public double CommitmentExpenses => AssignedExpenses - ActualExpenses;
        public double PendingExpenses => Expenses - AssignedExpenses - PotencialExpenses;

        public double CommitmentCapital => AssignedCapital - ActualCapital;
        public double PendingCapital => Capital - AssignedCapital - PotencialCapital;
    }
}
