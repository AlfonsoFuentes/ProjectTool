using Shared.Enums.CostCenter;
using Shared.Enums.MWOStatus;
using Shared.NewModels.PurchaseOrders.Responses;

namespace Shared.NewModels.MWOs.Reponses
{

    public class NewMWOApprovedReponse
    {
        public Guid MWOId { get; set; }
        public CostCenterEnum CostCenter { get; set; } = CostCenterEnum.None;
        public string MWONumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum Type { get; set; } = MWOTypeEnum.None;
        public FocusEnum Focus { get; set; } = FocusEnum.None;
        public MWOStatusEnum Status => MWOStatusEnum.Approved;
        public bool IsAssetProductive { get; set; }
        public double PercentageTaxForAlterations { get; set; }

        public List<NewBudgetItemMWOApprovedResponse> BudgetItems { get; set; } = new List<NewBudgetItemMWOApprovedResponse>();


        public DateTime ApprovedDate { get; set; }

        public List<NewBudgetItemMWOApprovedResponse> BudgetItemsExpenses => BudgetItems == null ? new() : BudgetItems.Where(x => x.IsAlteration).ToList();

        public List<NewBudgetItemMWOApprovedResponse> BudgetItemsCapital => BudgetItems == null ? new() : BudgetItems.Where(x => x.IsAlteration == false).ToList();

        public double ExpensesUSD => BudgetItemsExpenses.Count == 0 ? 0 : BudgetItemsExpenses.Sum(x => x.BudgetUSD);

        public double CapitalUSD => BudgetItemsCapital.Count == 0 ? 0 : BudgetItemsCapital.Sum(x => x.BudgetUSD);

        public double AppropiationUSD => ExpensesUSD + CapitalUSD;

        public bool HasExpenses => BudgetItemsExpenses.Count > 0;
        #region MWONotApproved

        public List<NewBudgetItemMWOApprovedResponse> BudgetItemsWithoutEngineering => BudgetItemsCapital == null ? new() :
            BudgetItemsCapital.Where(x => x.IsEngineeringItem == false).ToList();

        public List<NewBudgetItemMWOApprovedResponse> BudgetItemsWithoutEngineeringOrTaxes => BudgetItemsCapital == null ? new() :
            BudgetItemsCapital.Where(x => !(x.IsEngineeringItem == true || x.IsMainItemTaxesNoProductive == true)).ToList();


        public List<NewBudgetItemMWOApprovedResponse> BudgetItemsEngineering => BudgetItemsCapital == null ? new() :
            BudgetItemsCapital.Where(x => x.IsEngineeringItem == true).ToList();


        public NewBudgetItemMWOApprovedResponse? ItemCapitalizedSalary => BudgetItemsEngineering == null ? new() :
           BudgetItemsEngineering.FirstOrDefault(x => x.Type.Id == BudgetItemTypeEnum.Engineering.Id && x.IsEngineeringItem == true);

        public NewBudgetItemMWOApprovedResponse? ItemContingency => BudgetItemsEngineering == null ? new() :
            BudgetItemsEngineering.FirstOrDefault(x => x.Type.Id == BudgetItemTypeEnum.Contingency.Id && x.IsEngineeringItem == true);

        public NewBudgetItemMWOApprovedResponse? ItemTaxNoProductive => BudgetItemsCapital == null ? new() :
            BudgetItemsCapital.FirstOrDefault(x => x.Type.Id == BudgetItemTypeEnum.Taxes.Id && x.IsMainItemTaxesNoProductive == true);

        #region EngineeringDataForCalculation

        public double TotalPercentEnginContingency => BudgetItemsEngineering.Count == 0 ? 0 :
            BudgetItemsEngineering.Sum(x => x.Percentage);

        public double PercentageCapitalizedSalary => ItemCapitalizedSalary == null ? 0 : ItemCapitalizedSalary.Percentage;

        public double PercentageContingency => ItemContingency == null ? 0 : ItemContingency.Percentage;

        public double CapitalForEngineeringCalculationUSD => BudgetItemsWithoutEngineering.Count == 0 ? 0 : BudgetItemsWithoutEngineering.Sum(x => x.BudgetUSD);

        #endregion
        #region TaxesCalculations

        public double PercentageAssetNoProductive => ItemTaxNoProductive == null ? 0 : ItemTaxNoProductive.Percentage;

        public double CapitalForTaxesCalculationsUSD => BudgetItemsWithoutEngineeringOrTaxes.Count == 0 ? 0 : BudgetItemsWithoutEngineeringOrTaxes.Sum(x => x.BudgetUSD);

        #endregion
        #endregion




        public string CECName => Status.Id == MWOStatusEnum.Created.Id ? "MWO not approved" : $"CEC0000{MWONumber}";

        #region DataMWOApproved

        #region CapitalMWOApproved
        public double CapitalPendingToCommitUSD => CapitalUSD - CapitalAssignedUSD;
        public double CapitalActualUSD => BudgetItemsCapital == null || BudgetItemsCapital.Count == 0 ? 0 :
             BudgetItemsCapital.Sum(x => x.ActualUSD);

        public double CapitalAssignedUSD => BudgetItemsCapital == null || BudgetItemsCapital.Count == 0 ? 0 :
            BudgetItemsCapital.Sum(x => x.AssignedUSD);

        public double CapitalApprovedUSD => BudgetItemsCapital == null || BudgetItemsCapital.Count == 0 ? 0 :
            BudgetItemsCapital.Sum(x => x.ApprovedUSD);

        public double CapitalPotentialCommitmentUSD => BudgetItemsCapital == null || BudgetItemsCapital.Count == 0 ? 0 :
            BudgetItemsCapital.Sum(x => x.PotentialCommitmentUSD);


        public double CapitalPendingToReceiveUSD => BudgetItemsCapital == null || BudgetItemsCapital.Count == 0 ? 0 :
            BudgetItemsCapital.Sum(x => x.PendingToReceiveUSD);
        #endregion
        #region ExpensesMWOApproved
        public double ExpensesPendingToCommitUSD => ExpensesUSD - ExpensesAssignedUSD;
        public double ExpensesActualUSD => BudgetItemsExpenses == null || BudgetItemsExpenses.Count == 0 ? 0 :
             BudgetItemsExpenses.Sum(x => x.ActualUSD);

        public double ExpensesAssignedUSD => BudgetItemsExpenses == null || BudgetItemsExpenses.Count == 0 ? 0 :
            BudgetItemsExpenses.Sum(x => x.AssignedUSD);

        public double ExpensesApprovedUSD => BudgetItemsExpenses == null || BudgetItemsExpenses.Count == 0 ? 0 :
            BudgetItemsExpenses.Sum(x => x.ApprovedUSD);

        public double ExpensesPotentialCommitmentUSD => BudgetItemsExpenses == null || BudgetItemsExpenses.Count == 0 ? 0 :
            BudgetItemsExpenses.Sum(x => x.PotentialCommitmentUSD);


        public double ExpensesPendingToReceiveUSD => BudgetItemsExpenses == null || BudgetItemsExpenses.Count == 0 ? 0 :
            BudgetItemsExpenses.Sum(x => x.PendingToReceiveUSD);
        #endregion
        #endregion
    }

}
