using Shared.Models.MWOStatus;
using Shared.Models.MWOTypes;

namespace Shared.Models.MWO
{
    public class MWOClosedResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum MWOType { get; set; } = MWOTypeEnum.None;
        public string Type => MWOType.Name;
        public MWOStatusEnum MWOStatus { get; set; } = MWOStatusEnum.None;
        public string Status => MWOStatus.Name;
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;

        public double AppropiationUSD => Capital.BudgetUSD + Expenses.BudgetUSD;
        public string CECName { get; set; } = string.Empty;
        public string CostCenter { get; set; } = string.Empty;
        public bool HasExpenses { get; set; }
        public bool IsAssetProductive { get; set; } = true;
        public double PercentageAssetNoProductive { get; set; } = 19;
        public double PercentageEngineering { get; set; } = 6;
        public double PercentageContingency { get; set; } = 10;
        public double PercentageTaxForAlterations { get; set; } = 19;
        public MWOCount Capital { get; set; } = new();
        public MWOCount Expenses { get; set; } = new();

        public double BudgetCapitalUSD => Capital.BudgetUSD;
        public double AssignedCapitalUSD => Capital.AssignedUSD;
        public double ApprovedCapitalUSD => Capital.ApprovedUSD;
        public double ActualCapitalUSD => Capital.ActualUSD;
        public double CommitmentCapitalUSD => Capital.CommitmentUSD;
        public double PotentialCommitmentCapitalUSD => Capital.PotentialCommitmentUSD;
        public double PendingToCommitCapitalUSD => Capital.PendingToCommitUSD;

        public double BudgetExpensesUSD => Expenses.BudgetUSD;
        public double AssignedExpensesUSD => Expenses.AssignedUSD;
        public double ApprovedExpensesUSD => Expenses.ApprovedUSD;
        public double ActualExpensesUSD => Expenses.ActualUSD;
        public double CommitmentExpensesUSD => Expenses.CommitmentUSD;
        public double PotentialCommitmentExpensesUSD => Expenses.PotentialCommitmentUSD;
        public double PendingToCommitExpensesUSD => Expenses.PendingToCommitUSD;

    }
}
