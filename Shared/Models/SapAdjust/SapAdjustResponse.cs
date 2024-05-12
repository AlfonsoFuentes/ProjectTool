namespace Shared.Models.SapAdjust
{
    public class SapAdjustResponse
    {
        public Guid MWOId {  get; set; }
        public Guid SapAdjustId { get; set; }
        public DateTime Date { get; set; }
        public double ActualSap { get; set; }
        public double CommitmentSap { get; set; }
        public double PotencialSap { get; set; }
        public string Name => $"SAP Conciliation in {CECName} in {Date.ToString("d")}";
        public string CECName { get; set; } = string.Empty;
        public double ActualSoftware { get; set; }
        public double CommitmentSoftware { get; set; }
        public double PotencialSoftware { get; set; }
        public string Justification { get; set; } = string.Empty;
        public string ImageTitle { get; set; } = string.Empty;
        public string ImageData { get; set; } = string.Empty;
        public double BudgetCapital {  get; set; }
        public double PendingSap => BudgetCapital - AssignedSap;
        public double AssignedSap => ActualSap + CommitmentSap + PotencialSap;
        public double AssignedSoftware => ActualSoftware + CommitmentSoftware + PotencialSoftware;
        
    }
}
