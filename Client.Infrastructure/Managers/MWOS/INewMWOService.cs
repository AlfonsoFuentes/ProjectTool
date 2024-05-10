using Shared.NewModels.EBPReport;
using Shared.NewModels.MWOs.Reponses;
using Shared.NewModels.MWOs.Request;

namespace Client.Infrastructure.Managers.MWOS
{
    public interface INewMWOService : IManager
    {
        Task<IResult<NewMWOUpdateRequest>> GetMWOToUpdateById(Guid id);
        Task<IResult> UpdateMWOCreated(NewMWOUpdateRequest request);
        Task<IResult> CreateMWO(NewMWOCreateRequest request);
        Task<IResult<NewMWOCreatedListResponse>> GetAllMWOCreated();
        Task<IResult<NewMWOApprovedListReponse>> GetAllMWOApproved();
        Task<IResult> DeleteMWO(NewMWODeleteRequest request);
        Task<IResult> UnApprovedMWO(NewMWOUnApproveRequest request);
        Task<IResult<NewMWOApproveRequest>> GetMWOByIdToApprove(Guid MWOId);
        Task<IResult<NewEBPReportResponse>> GetMWOEBPReportById(Guid MWOId);
        Task<IResult> ApproveMWO(NewMWOApproveRequest request);
    }
    public class NewMWOService : INewMWOService
    {
        IHttpClientService http;

        public NewMWOService(IHttpClientService http)
        {
            this.http = http;
        }
        public async Task<IResult> CreateMWO(NewMWOCreateRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewMWO.Create, request);
            return await result.ToResult();
        }
        public async Task<IResult<NewMWOCreatedListResponse>> GetAllMWOCreated()
        {
            var result = await http.GetAsync(ClientEndPoint.NewMWO.GetAllCreated);
            return await result.ToResult<NewMWOCreatedListResponse>();
        }

        public async Task<IResult> DeleteMWO(NewMWODeleteRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewMWO.Delete, request);
            return await result.ToResult();
        }

        public async Task<IResult> UpdateMWOCreated(NewMWOUpdateRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewMWO.Update, request);
            return await result.ToResult();
        }

        public async Task<IResult<NewMWOUpdateRequest>> GetMWOToUpdateById(Guid Id)
        {
            var result = await http.GetAsync($"{ClientEndPoint.Controller.MWO}/{Id}");
            return await result.ToResult<NewMWOUpdateRequest>();
        }

        public async Task<IResult<NewMWOApprovedListReponse>> GetAllMWOApproved()
        {
            var result = await http.GetAsync(ClientEndPoint.NewMWO.GetAllApproved);
            return await result.ToResult<NewMWOApprovedListReponse>();
        }

        public async Task<IResult> UnApprovedMWO(NewMWOUnApproveRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewMWO.UnApprove, request);
            return await result.ToResult();
        }

        public async Task<IResult<NewMWOApproveRequest>> GetMWOByIdToApprove(Guid MWOId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.NewMWO.GetToApproved}/{MWOId}");
            return await result.ToResult<NewMWOApproveRequest>();
        }

        public async Task<IResult> ApproveMWO(NewMWOApproveRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewMWO.Approve, request);
            return await result.ToResult();
        }

        public async Task<IResult<NewEBPReportResponse>> GetMWOEBPReportById(Guid MWOId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.NewMWO.GetEBPReport}/{MWOId}");
            return await result.ToResult<NewEBPReportResponse>();
        }
    }
}
