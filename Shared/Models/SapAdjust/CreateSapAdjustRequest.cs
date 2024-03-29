using Shared.Models.MWO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.SapAdjust
{
    public class CreateSapAdjustRequest
    {
       
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
        public double BudgetCapital => MWOApproved == null ? 0 : MWOApproved.Capital;
        public double PendingCapital => MWOApproved == null ? 0 : MWOApproved.PendingCapital;
        public double ActualSoftware => MWOApproved == null ? 0 : MWOApproved.ActualCapital;
        public double CommitmentSoftware => MWOApproved == null ? 0 : MWOApproved.CommitmentCapital;
        public double PotencialSoftware => MWOApproved == null ? 0 : MWOApproved.PotencialCapital;
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

            if (Validator != null) { await Validator(); }
        }
        public MWOApprovedResponse MWOApproved { get; set; } = null!;
        public string MWOName => MWOApproved == null ? string.Empty : MWOApproved.Name;
        public CreateSapAdjustRequestDto ConvertToDto()

        {
            return new()
            {
                ActualSap = ActualSap,
                ActualSoftware = ActualSoftware,
                CommitmentSap = CommitmentSap,
                CommitmentSoftware = CommitmentSoftware,
                Date = Date,
                ImageData = ImageData,
                ImageTitle = ImageTitle,
                Justification = Justification,
                MWOId = MWOApproved.Id,
                PotencialSap = PotencialSap,
                PotencialSoftware = PotencialSoftware,
                BudgetCapital = BudgetCapital,
            };
        }
    }
}
