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
       
        
        public double CommitmentSap { get; set; }
       
        public double PotencialSap { get; set; }
     
        public double BudgetCapital => MWOApproved == null ? 0 : MWOApproved.Capital.BudgetUSD;
        public double PendingCapital => MWOApproved == null ? 0 : MWOApproved.Capital.PendingToCommitUSD;
        public double ActualSoftware => MWOApproved == null ? 0 : MWOApproved.Capital.ActualUSD;
        public double CommitmentSoftware => MWOApproved == null ? 0 : MWOApproved.Capital.CommitmentUSD;
        public double PotencialSoftware => MWOApproved == null ? 0 : MWOApproved.Capital.PotentialCommitmentUSD;
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
      
        public MWOApprovedWithBudgetItemsResponse MWOApproved { get; set; } = null!;
        public string MWOName => MWOApproved == null ? string.Empty : MWOApproved.Name;
        public string MWOCECName => MWOApproved == null ? string.Empty : MWOApproved.CECName;
        public Guid MWOId => MWOApproved == null ? Guid.Empty : MWOApproved.Id;
    }
}
