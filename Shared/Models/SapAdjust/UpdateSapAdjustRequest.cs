using Shared.Models.MWO;

namespace Shared.Models.SapAdjust
{
    public class UpdateSapAdjustRequest
    {
        public Guid SapAdjustId { get; set; }
        public DateTime Date { get; set; }
        public double ActualSap { get; set; }
       
      
        public double CommitmentSap { get; set; }
       
        public double PotencialSap { get; set; }
       
        public double BudgetCapital { get; set; }
        public double PendingCapital { get; set; }
        public double ActualSoftware { get; set; }
        public double CommitmentSoftware { get; set; }
        public double PotencialSoftware { get; set; }
        public double AssignedSap => ActualSap + CommitmentSap + PotencialSap;
        public double AssignedSoftware => ActualSoftware + CommitmentSoftware + PotencialSoftware;
        public double DiferenceActual => ActualSap == 0 ? 0 : ActualSap - ActualSoftware;
        public double DiferenceCommitment => CommitmentSap == 0 ? 0 : CommitmentSap - CommitmentSoftware;
        public double DiferencePotencial => PotencialSap == 0 ? 0 : PotencialSap - PotencialSoftware;
        public double DiferenceAssigned => AssignedSap == 0 ? 0 : AssignedSap - AssignedSoftware;

        public double PecentageActual => ActualSoftware == 0 ? 0 : Math.Abs(100 - Math.Abs(ActualSap / ActualSoftware) * 100);
        public double PecentageCommitment => CommitmentSoftware == 0 ? 0 : Math.Abs(100 - Math.Abs(CommitmentSap / CommitmentSoftware) * 100);
        public double PecentagePotencial => PotencialSoftware == 0 ? 0 : Math.Abs(100 - Math.Abs(PotencialSap / PotencialSoftware) * 100);
        public string Justification { get; set; } = string.Empty;
        public string ImageTitle { get; set; } = string.Empty;
        public string ImageData { get; set; } = string.Empty;
       
      
        public string MWOName { get; set; }=string.Empty;
        public string CECMWOName { get; set; } = string.Empty;
        public Guid MWOId { get; set; }

        
    }
}
