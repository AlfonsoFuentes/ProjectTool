using Shared.Enums.CostCenter;
using Shared.Enums.MWOStatus;
using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Responses;

namespace Shared.NewModels.EBPReport
{
    public class NewEBPReportResponse
    {

        public Guid MWOId { get; set; }
        public CostCenterEnum CostCenter { get; set; } = CostCenterEnum.None;
        public string MWONumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum Type { get; set; } = MWOTypeEnum.None;
        public FocusEnum Focus { get; set; } = FocusEnum.None;
        public MWOStatusEnum Status => MWOStatusEnum.Approved;
        public bool IsAssetProductive { get; set; }
        public double PercentageTaxForAlterations { get; set; }
        public DateTime ApprovedDate { get; set; }
        public List<NewPriorPurchaseOrderResponse> PurchaseOrders { get; set; } = new();
        public List<NewPriorPurchaseOrderResponse> ActualPurchaseOrders =>PurchaseOrders
            .Where(x => 
            x.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Receiving.Id || 
            x.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Closed.Id).ToList();
        public List<NewPriorPurchaseOrderResponse> CommitmentPurchaseOrders => PurchaseOrders
            .Where(x => 
            x.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Receiving.Id || 
            x.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Approved.Id).ToList();
        public List<NewPriorPurchaseOrderResponse> PotentialPurchaseOrders => PurchaseOrders
            .Where(x => x.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id).ToList();
        public double ActualUSD => ActualPurchaseOrders.Count == 0 ? 0 : ActualPurchaseOrders.Sum(x => x.ActualUSD);
        public double CommitmentUSD => CommitmentPurchaseOrders.Count == 0 ? 0 : CommitmentPurchaseOrders.Sum(x => x.CommitmentUSD);
        public double PotentialUSD => PotentialPurchaseOrders.Count == 0 ? 0 : PotentialPurchaseOrders.Sum(x => x.AssignedUSD);
        public List<NewSummaryTotalResponse> SummaryTotals => new() { new NewSummaryTotalResponse(ActualUSD, CommitmentUSD, PotentialUSD) };

    }
    public record NewSummaryTotalResponse(double Actual, double Commitment, double Potential);
}

