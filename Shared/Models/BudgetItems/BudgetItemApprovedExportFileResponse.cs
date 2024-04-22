using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models.BudgetItems
{
    public class BudgetItemApprovedExportFileResponse
    {
        public string MWOName { get; set; } = string.Empty;
        public string Nomenclatore { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public double BudgetUSD { get; set; }

        public double ApprovedUSD { get; set; }
        public double ActualUSD { get; set; }
        public double AssignedUSD { get; set; }
        public double PotentialCommitmentUSD { get; set; }
        public double PendingToCommitUSD => BudgetUSD - AssignedUSD;
        public double PendingToReceiveUSD => ApprovedUSD - ActualUSD;

    }
}
