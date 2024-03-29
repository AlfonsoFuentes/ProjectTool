using Shared.Models.MWO;

namespace Shared.Models.SapAdjust
{
    public class UpdateSapAdjustRequest
    {
        public Guid SapAdjustId { get; set; }
        public DateTime Date { get; set; }
        public double ActualSap { get; set; }
        public Func<Task<bool>> Validator { get; set; } = null!;
        public async Task ChangeActualSap(string actualsapstring)
        {
            double actualsap = 0;
            if (double.TryParse(actualsapstring, out actualsap))
            {
                ActualSap = actualsap;
            }
            if (Validator != null) await Validator();
        }
        public double CommitmentSap { get; set; }
        public async Task ChangeCommitmentSap(string commitmentsapstring)
        {
            double commitmsap = 0;
            if (double.TryParse(commitmentsapstring, out commitmsap))
            {
                CommitmentSap = commitmsap;
            }
            if (Validator != null) await Validator();
        }
        public double PotencialSap { get; set; }
        public async Task ChangePotencialSap(string potencialsapstring)
        {
            double potencialsap = 0;
            if (double.TryParse(potencialsapstring, out potencialsap))
            {
                PotencialSap = potencialsap;
            }
            if (Validator != null) await Validator();
        }
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
        public async Task ChangeImage(string image)
        {
            ImageData = string.Empty;
            if (!string.IsNullOrWhiteSpace(image))
            {
                ImageData = image;
            }
            
            if(Validator!=null) { await Validator(); }
        }
      
        public string MWOName { get; set; }=string.Empty;
        public Guid MWOId { get; set; }

        public UpdateSapAdjustRequestDto ConvertToDto()

        {
            return new()
            {
                SapAdjustId = SapAdjustId,
                ActualSap = ActualSap,
                ActualSoftware = ActualSoftware,
                CommitmentSap = CommitmentSap,
                CommitmentSoftware = CommitmentSoftware,
                Date = Date,
                ImageData = ImageData,
                ImageTitle = ImageTitle,
                Justification = Justification,

                PotencialSap = PotencialSap,
                PotencialSoftware = PotencialSoftware,
            };
        }
    }
}
