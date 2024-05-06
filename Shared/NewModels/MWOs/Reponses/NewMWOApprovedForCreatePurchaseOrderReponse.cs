using Shared.Enums.CostCenter;
using Shared.Enums.MWOStatus;

namespace Shared.NewModels.MWOs.Reponses
{
    public class NewMWOApprovedForCreatePurchaseOrderReponse
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

        public List<NewBudgetItemToCreatePurchaseOrderResponse> BudgetItems { get; set; } = new List<NewBudgetItemToCreatePurchaseOrderResponse>();


        public DateTime ApprovedDate { get; set; }

        public List<NewBudgetItemToCreatePurchaseOrderResponse> BudgetItemsExpenses => BudgetItems == null ? new() : BudgetItems.Where(x => x.IsAlteration).ToList();

        public List<NewBudgetItemToCreatePurchaseOrderResponse> BudgetItemsCapital => BudgetItems == null ? new() : BudgetItems.Where(x => x.IsAlteration == false).ToList();

        public double ExpensesUSD => BudgetItemsExpenses.Count == 0 ? 0 : BudgetItemsExpenses.Sum(x => x.BudgetUSD);

        public double CapitalUSD => BudgetItemsCapital.Count == 0 ? 0 : BudgetItemsCapital.Sum(x => x.BudgetUSD);

        public double AppropiationUSD => ExpensesUSD + CapitalUSD;

        public bool HasExpenses => BudgetItemsExpenses.Count > 0;
        #region MWONotApproved

        public List<NewBudgetItemToCreatePurchaseOrderResponse> BudgetItemsWithoutEngineering => BudgetItemsCapital == null ? new() :
            BudgetItemsCapital.Where(x => x.IsEngineeringItem == false).ToList();

        public List<NewBudgetItemToCreatePurchaseOrderResponse> BudgetItemsWithoutEngineeringOrTaxes => BudgetItemsCapital == null ? new() :
            BudgetItemsCapital.Where(x => !(x.IsEngineeringItem == true || x.IsMainItemTaxesNoProductive == true)).ToList();


        public List<NewBudgetItemToCreatePurchaseOrderResponse> BudgetItemsEngineering => BudgetItemsCapital == null ? new() :
            BudgetItemsCapital.Where(x => x.IsEngineeringItem == true).ToList();


        public NewBudgetItemToCreatePurchaseOrderResponse? ItemCapitalizedSalary => BudgetItemsEngineering == null ? new() :
           BudgetItemsEngineering.FirstOrDefault(x => x.Type.Id == BudgetItemTypeEnum.Engineering.Id && x.IsEngineeringItem == true);

        public NewBudgetItemToCreatePurchaseOrderResponse? ItemContingency => BudgetItemsEngineering == null ? new() :
            BudgetItemsEngineering.FirstOrDefault(x => x.Type.Id == BudgetItemTypeEnum.Contingency.Id && x.IsEngineeringItem == true);

        public NewBudgetItemToCreatePurchaseOrderResponse? ItemTaxNoProductive => BudgetItemsCapital == null ? new() :
            BudgetItemsCapital.FirstOrDefault(x => x.Type.Id == BudgetItemTypeEnum.Taxes.Id && x.IsMainItemTaxesNoProductive == true);

        #region EngineeringDataForCalculation


        public double CapitalForEngineeringCalculationUSD => BudgetItemsWithoutEngineering.Count == 0 ? 0 : BudgetItemsWithoutEngineering.Sum(x => x.BudgetUSD);

        #endregion
        #region TaxesCalculations

      
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
