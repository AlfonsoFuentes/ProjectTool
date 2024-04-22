namespace Shared.Models.MWO
{
    public class MWOCount
    {
        public double BudgetUSD { get; set; }
        public double AssignedUSD { get; set; }
        public double ApprovedUSD { get; set; }
        public double ActualUSD { get; set; }
        public double CommitmentUSD => ApprovedUSD - ActualUSD;
        public double PotentialCommitmentUSD { get; set; }
        public double PendingToCommitUSD => BudgetUSD - AssignedUSD;
    }
}
