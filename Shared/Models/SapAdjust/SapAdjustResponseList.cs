using Shared.Models.ChartSeries;

namespace Shared.Models.SapAdjust
{
    public class SapAdjustResponseList
    {
        public IEnumerable<SapAdjustResponse> Adjustments { get; set; } = new List<SapAdjustResponse>();
        public string MWOName { get; set; } = string.Empty;
        public string MWOCECName { get; set; } = string.Empty;
        public DateTime MWOApprovedDate { get; set; }
        public List<ChartSeries<DateTime, double>> Series => GetSeries();
        List<ChartSeries<DateTime, double>> GetSeries()
        {
            List<ChartSeries<DateTime, double>> result = new();

            ChartSeries<DateTime, double> Actual = new()
            {
                Name = "Actual",
                DataCollection = Adjustments.Select(x => new ChartSeriesData<DateTime, double>
                {
                    XValue = x.Date.Date,
                    YValue = x.ActualSap,

                }).ToList(),
            };
            ChartSeries<DateTime, double> Commitment = new()
            {
                Name = "Commiment",
                DataCollection = Adjustments.Select(x => new ChartSeriesData<DateTime, double>
                {
                    XValue = x.Date.Date,
                    YValue = x.CommitmentSap,

                }).ToList(),
            };
            ChartSeries<DateTime, double> Assigned = new()
            {
                Name = "Assigned",
                DataCollection = Adjustments.Select(x => new ChartSeriesData<DateTime, double>
                {
                    XValue = x.Date.Date,
                    YValue = x.AssignedSap,

                }).ToList(),
            };
            ChartSeries<DateTime, double> Pending = new()
            {
                Name = "Pending",
                DataCollection = Adjustments.Select(x => new ChartSeriesData<DateTime, double>
                {
                    XValue = x.Date.Date,
                    YValue = x.PendingSap,

                }).ToList(),
            };
            result.Add(Actual);
            result.Add(Commitment);
            result.Add(Assigned);
            result.Add(Pending);
            return result;
        }
        public string[] XAxisLabels => Adjustments.Select(x => x.Date.ToShortDateString()).ToArray();
    }
}
