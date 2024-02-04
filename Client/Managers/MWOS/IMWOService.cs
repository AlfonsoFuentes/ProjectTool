using Shared.Commons.Results;
using Shared.Models.MWO;
using System.Net.Http.Json;
using System.Xml.Linq;

namespace Client.Managers.MWOS
{
    public interface IMWOService:IManager
    {
        Task<IResult> UpdateMWO(MWOUpdateRequest request);
        Task<IResult> CreateMWO(MWOCreateRequest request);
        Task<bool> ReviewIfNameExist(string name);
        Task<bool> ReviewIfNameExist(MWOUpdateRequest name);
        Task<IResult<List<MWOResponse>>> GetAllMWO();
    }
    public class MWOService:IMWOService
    {
        private HttpClient Http;

        public MWOService(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }

        public async Task<IResult> CreateMWO(MWOCreateRequest request)
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
        public async Task<IResult> UpdateMWO(MWOUpdateRequest request)
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

        public async Task<bool> ReviewIfNameExist(string name)
        {
            var httpresult = await Http.GetAsync($"mwo/CreateNameExist?name={name}");

            return await httpresult.ToObject<bool>();
        }

        public async Task<IResult<List<MWOResponse>>> GetAllMWO()
        {
            var httpresult = await Http.GetAsync($"mwo/getall");
            return await httpresult.ToResult<List<MWOResponse>>();
        }

        public async Task<bool> ReviewIfNameExist(MWOUpdateRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync($"mwo/UpdateNameExist",request);

            return await httpresult.ToObject<bool>();
        }
    }
}
