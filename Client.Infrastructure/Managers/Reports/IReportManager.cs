namespace Client.Infrastructure.Managers.Reports
{
    public interface IReportManager : IManager
    {
        Task<IResult<NewEBPReportResponse>> GetEBPReport(Guid MWOId);
    }
    public class ReportManager : IReportManager
    {
        IHttpClientService http;

        public ReportManager(IHttpClientService http)
        {
            this.http = http;
        }

        public async Task<IResult<NewEBPReportResponse>> GetEBPReport(Guid MWOId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.Report.GetEBPReport}/{MWOId}");
            return await result.ToResult<NewEBPReportResponse>();
        }
    }
}
