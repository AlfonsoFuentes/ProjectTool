namespace Shared.Models.SapAdjust
{
    public class UpdateSapAdjustRequestDto
    {
        public Guid SapAdjustId { get; set; }
        public DateTime Date { get; set; }
        public double ActualSap { get; set; }
        public double CommitmentSap { get; set; }
        public double PotencialSap { get; set; }
        

        public double ActualSoftware { get; set; }
        public double CommitmentSoftware { get; set; }
        public double PotencialSoftware { get; set; }
        public string Justification { get; set; } = string.Empty;
        public string ImageTitle { get; set; } = string.Empty;
        public string ImageData { get; set; } = string.Empty;


    }
}
