namespace Client.Infrastructure.Managers.MWOS
{
    public interface IMWOService : IManager
    {
        Task<IResult<ApproveMWORequest>> GetMWOByIdToApprove(Guid MWOId);
        Task<IResult<MWOEBPResponse>> GetMWOEBPReport(Guid MWOId);
        Task<IResult> UpdateMWO(UpdateMWORequest request);
        Task<IResult> CreateMWO(CreateMWORequest request);
        Task<IResult> ApproveMWO(ApproveMWORequest request);
        Task<IResult> UnApproveMWO(UnApproveMWORequest request);
        Task<IResult<MWOResponseList>> GetAllMWO();
        Task<IResult<FileResult>> ExportMWOsCreated();
        Task<IResult<UpdateMWORequest>> GetMWOToUpdateById(Guid id);
       
        Task<IResult> Delete(MWOCreatedResponse request);
        Task<IResult<MWOApprovedWithBudgetItemsResponse>> GetMWOApprovedById(Guid id);
        Task<IResult<MWOCreatedResponse>> GetMWOCreatedById(Guid id);
        Task<IResult<FileResult>> ExportMWOsApproved();
        Task<IResult<FileResult>> ExportMWOsClosed();
    }
    public class MWOService : IMWOService
    {
        private HttpClient Http;

        public MWOService(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }

        public async Task<IResult> CreateMWO(CreateMWORequest request)
        {
            try
            {
               
                var httpresult = await Http.PostAsJsonAsync("MWO/createMWO", request);

                return await httpresult.ToResult();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return await Result.FailAsync();

        }
        public async Task<IResult> UpdateMWO(UpdateMWORequest request)
        {
            try
            {
             
                var httpresult = await Http.PostAsJsonAsync("MWO/updateMWO", request);

                return await httpresult.ToResult();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return await Result.FailAsync();

        }

      

        public async Task<IResult<MWOResponseList>> GetAllMWO()
        {
            var httpresult = await Http.GetAsync($"mwo/getall");
            return await httpresult.ToResult<MWOResponseList>();
        }

       

        public async Task<IResult<UpdateMWORequest>> GetMWOToUpdateById(Guid id)
        {
            var httpresult = await Http.GetAsync($"mwo/GetMWOToUpdate/{id}");
            return await httpresult.ToResult<UpdateMWORequest>();
        }

        public async Task<IResult> Delete(MWOCreatedResponse request)
        {
            var httpresult = await Http.PostAsJsonAsync($"mwo/Delete", request);
            return await httpresult.ToResult();
        }

        public async Task<IResult> ApproveMWO(ApproveMWORequest request)
        {
            try
            {
              
                var httpresult = await Http.PostAsJsonAsync("MWO/approveMWO", request);

                return await httpresult.ToResult();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return await Result.FailAsync();
        }
        public async Task<IResult> UnApproveMWO(UnApproveMWORequest request)
        {
            try
            {

                var httpresult = await Http.PostAsJsonAsync("MWO/UnapproveMWO", request);

                return await httpresult.ToResult();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return await Result.FailAsync();
        }

        public async Task<IResult<ApproveMWORequest>> GetMWOByIdToApprove(Guid MWOId)
        {
            var httpresult = await Http.GetAsync($"mwo/GetMWOToApprove/{MWOId}");
            return await httpresult.ToResult<ApproveMWORequest>();

        }

      
       
        public async Task<IResult<MWOApprovedWithBudgetItemsResponse>> GetMWOApprovedById(Guid id)
        {
            var httpresult = await Http.GetAsync($"mwo/GetMWOApproved/{id}");
            return await httpresult.ToResult<MWOApprovedWithBudgetItemsResponse>();
        }
        public async Task<IResult<MWOCreatedResponse>> GetMWOCreatedById(Guid id)
        {
            var httpresult = await Http.GetAsync($"mwo/GetMWOCreated/{id}");
            return await httpresult.ToResult<MWOCreatedResponse>();
        }
        public async Task<IResult<FileResult>> ExportMWOsCreated()
        {
            var httpresult = await Http.GetAsync($"ExportToFile/MWOsCreated");
            return await httpresult.ToResult<FileResult>();
        }
        public async Task<IResult<FileResult>> ExportMWOsApproved()
        {
            var httpresult = await Http.GetAsync($"ExportToFile/MWOsApproved");
            return await httpresult.ToResult<FileResult>();
        }

        public async Task<IResult<FileResult>> ExportMWOsClosed()
        {
            var httpresult = await Http.GetAsync($"ExportToFile/MWOsClosed");
            return await httpresult.ToResult<FileResult>();
        }

        public async Task<IResult<MWOEBPResponse>> GetMWOEBPReport(Guid MWOId)
        {
            var httpresult = await Http.GetAsync($"mwo/GetMWOEBPReport/{MWOId}");
            return await httpresult.ToResult<MWOEBPResponse>();
        }
    }
}
