using Shared.Models.PurchaseOrders.Responses;

namespace Shared.Models.MWO
{
    public class MWOEBPResponse
    {
        public Guid MWOId { get; set; }
        public string MWOName { get; set; } = string.Empty;
        public string CECName { get; set; } = string.Empty;
        public List<NewPurchaseOrderResponse> Actual { get; set; } = new();
        public List<NewPurchaseOrderResponse> Commitment { get; set; } = new();
        public List<NewPurchaseOrderResponse> Potential { get; set; } = new();
        public double ActualUSD => Actual.Count == 0 ? 0 : Actual.Sum(x => x.ActualUSD);
        public double CommitmentUSD => Commitment.Count == 0 ? 0 : Commitment.Sum(x => x.CommitmentUSD);
        public double AssignedUSD => Potential.Count == 0 ? 0 : Potential.Sum(x => x.AssignedUSD);
        public List<SummaryTotal> SummaryTotals => new() { SummaryTotal };
        SummaryTotal SummaryTotal => new()
        {
            Actual = ActualUSD,
            Commitment = CommitmentUSD,
            Potential = AssignedUSD,
        };
    }
    public class SummaryTotal
    {
        public double Actual { get; set; }
        public double Commitment { get; set; }
        public double Potential { get; set; }
    }

}
