namespace Client.Infrastructure.Managers.MWOS
{
    public interface IMWOService : IManager
    {
        Task<IResult<ApproveMWORequest>> GetMWOByIdToApprove(Guid MWOId);
    
        Task<IResult> UpdateMWO(UpdateMWORequest request);
        Task<IResult> CreateMWO(CreateMWORequest request);
        Task<IResult> ApproveMWO(ApproveMWORequest request);
 
        Task<IResult<MWOResponseList>> GetAllMWO();
        //Task<IResult<IEnumerable<MWOResponse>>> GetAllMWOCreated();
        //Task<IResult<IEnumerable<MWOResponse>>> GetAllMWOApproved();
        //Task<IResult<IEnumerable<MWOResponse>>> GetAllMWOClosed();
        Task<IResult<UpdateMWORequest>> GetMWOToUpdateById(Guid id);
       
        Task<IResult> Delete(MWOResponse request);
        Task<IResult<MWOApprovedResponse>> GetMWOApprovedById(Guid id);
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
                var model=request.ConvertToDto();
                var httpresult = await Http.PostAsJsonAsync("MWO/createMWO", model);

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
                var model=request.ConvertToDto();
                var httpresult = await Http.PostAsJsonAsync("MWO/updateMWO", model);

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

        public async Task<IResult> Delete(MWOResponse request)
        {
            var httpresult = await Http.PostAsJsonAsync($"mwo/Delete", request);
            return await httpresult.ToResult();
        }

        public async Task<IResult> ApproveMWO(ApproveMWORequest request)
        {
            try
            {
                var model=request.ConvertToDto();   
                var httpresult = await Http.PostAsJsonAsync("MWO/approveMWO", model);

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

      
       
        public async Task<IResult<MWOApprovedResponse>> GetMWOApprovedById(Guid id)
        {
            var httpresult = await Http.GetAsync($"mwo/GetMWOApproved/{id}");
            return await httpresult.ToResult<MWOApprovedResponse>();
        }

        //public async Task<IResult<IEnumerable<MWOResponse>>> GetAllMWOCreated()
        //{
        //    var httpresult = await Http.GetAsync($"mwo/getallCreated");
        //    return await httpresult.ToResult<IEnumerable<MWOResponse>>();
        //}

        //public async Task<IResult<IEnumerable<MWOResponse>>> GetAllMWOApproved()
        //{
        //    var httpresult = await Http.GetAsync($"mwo/getallApproved");
        //    return await httpresult.ToResult<IEnumerable<MWOResponse>>();
        //}

        //public async Task<IResult<IEnumerable<MWOResponse>>> GetAllMWOClosed()
        //{
        //    var httpresult = await Http.GetAsync($"mwo/getallClosed");
        //    return await httpresult.ToResult<IEnumerable<MWOResponse>>();
        //}
    }
}
