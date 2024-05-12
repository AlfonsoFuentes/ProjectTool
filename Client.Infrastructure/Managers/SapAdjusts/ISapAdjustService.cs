



namespace Client.Infrastructure.Managers.SapAdjusts
{
    public interface ISapAdjustService : IManager
    {
        Task<IResult> UpdateSapAdjust(UpdateSapAdjustRequest request);
        Task<IResult> CreateSapAdjust(CreateSapAdjustRequest request);
        Task<IResult<SapAdjustResponseList>> GetAllSapAdjustByMWO(Guid MWOId);
        Task<IResult<UpdateSapAdjustRequest>> GetSapAdjustById(Guid id);

       

        Task<IResult> Delete(SapAdjustResponse request);
    }
    public class SapAdjustService : ISapAdjustService
    {
        IHttpClientService http;

        public SapAdjustService(IHttpClientService http)
        {
            this.http = http;
        }

        public async Task<IResult> CreateSapAdjust(CreateSapAdjustRequest request)
        {
            try
            {
             
                var httpresult = await http.PostAsJsonAsync(ClientEndPoint.SapAdjust.Create, request);

                return await httpresult.ToResult();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return await Result.FailAsync();

        }
        public async Task<IResult> UpdateSapAdjust(UpdateSapAdjustRequest request)
        {
            try
            {

                var httpresult = await http.PostAsJsonAsync(ClientEndPoint.SapAdjust.Update, request);

                return await httpresult.ToResult();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return await Result.FailAsync();

        }


        public async Task<IResult<SapAdjustResponseList>> GetAllSapAdjustByMWO(Guid MWOId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.SapAdjust.GetByIdMWOApproved}/{MWOId}");
            return await result.ToResult<SapAdjustResponseList>();
        }



        public async Task<IResult<UpdateSapAdjustRequest>> GetSapAdjustById(Guid id)
        {
            var httpresult = await http.GetAsync($"{ClientEndPoint.Controller.SapAdjust}/{id}");
            return await httpresult.ToResult<UpdateSapAdjustRequest>();
        }

        public async Task<IResult> Delete(SapAdjustResponse request)
        {
            var httpresult = await http.PostAsJsonAsync(ClientEndPoint.SapAdjust.Delete, request);
            return await httpresult.ToResult();
        }

        
    }
}
