

using Shared.Models.SapAdjust;

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
        private HttpClient Http;

        public SapAdjustService(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }

        public async Task<IResult> CreateSapAdjust(CreateSapAdjustRequest request)
        {
            try
            {
                CreateSapAdjustRequestDto model = request.ConvertToDto();
                var httpresult = await Http.PostAsJsonAsync("SapAdjust/CreateSapAdjust", model);

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
                UpdateSapAdjustRequestDto model = request.ConvertToDto();
                var httpresult = await Http.PostAsJsonAsync("SapAdjust/UpdateSapAdjust", model);

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
            var httpresult = await Http.GetAsync($"SapAdjust/GetAllByMWO/{MWOId}");
            return await httpresult.ToResult<SapAdjustResponseList>();
        }



        public async Task<IResult<UpdateSapAdjustRequest>> GetSapAdjustById(Guid id)
        {
            var httpresult = await Http.GetAsync($"SapAdjust/{id}");
            return await httpresult.ToResult<UpdateSapAdjustRequest>();
        }

        public async Task<IResult> Delete(SapAdjustResponse request)
        {
            var httpresult = await Http.PostAsJsonAsync($"SapAdjust/DeleteSapAdjust", request);
            return await httpresult.ToResult();
        }

        
    }
}
