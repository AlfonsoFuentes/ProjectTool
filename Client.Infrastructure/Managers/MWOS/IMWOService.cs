using Client.Infrastructure.Managers;
using Shared.Commons.Results;
using Shared.Models.MWO;
using System.Net.Http.Json;
using System.Xml.Linq;

namespace Client.Infrastructure.Managers.MWOS
{
    public interface IMWOService : IManager
    {
        Task<IResult> UpdateMWO(UpdateMWORequest request);
        Task<IResult> CreateMWO(CreateMWORequest request);
        Task<bool> ReviewIfNameExist(string name);
        Task<bool> ReviewIfNameExist(UpdateMWORequest name);
        Task<IResult<List<MWOResponse>>> GetAllMWO();
        Task<IResult<UpdateMWORequest>> GetMWOById(Guid id);

        Task<IResult> Delete(MWOResponse request);
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

        public async Task<bool> ReviewIfNameExist(UpdateMWORequest request)
        {
            var httpresult = await Http.PostAsJsonAsync($"mwo/UpdateNameExist", request);

            return await httpresult.ToObject<bool>();
        }

        public async Task<IResult<UpdateMWORequest>> GetMWOById(Guid id)
        {
            var httpresult = await Http.GetAsync($"mwo/{id}");
            return await httpresult.ToResult<UpdateMWORequest>();
        }

        public async Task<IResult> Delete(MWOResponse request)
        {
            var httpresult = await Http.PostAsJsonAsync($"mwo/Delete", request);
            return await httpresult.ToResult();
        }
    }
}
