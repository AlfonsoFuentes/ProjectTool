namespace Client.Infrastructure.Managers.MWOS
{
    public interface IMWOValidatorService : IManager
    {
        Task<bool> ValidateMWONameExist(string mwoname);
        Task<bool> ValidateMWONameExist(Guid MWOId, string mwoname);
        Task<bool> ValidateMWONumberExist(Guid MWOId, string mwonumber);
    }
    public class MWOValidatorService : IMWOValidatorService
    {
        private HttpClient Http;
        public MWOValidatorService(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }

        public async Task<bool> ValidateMWONumberExist(Guid MWOId, string mwonumber)
        {
            var httpresult = await Http.GetAsync($"MWOValidator/ValidateMWONumberExist/{MWOId}/{mwonumber}");
            return await httpresult.ToObject<bool>();
        }
        public async Task<bool> ValidateMWONameExist(string mwoname)
        {
            var httpresult = await Http.GetAsync($"MWOValidator/ValidateMWONameExist/{mwoname}");
            return await httpresult.ToObject<bool>();
        }
        public async Task<bool> ValidateMWONameExist(Guid MWOId, string mwoname)
        {
            var httpresult = await Http.GetAsync($"MWOValidator/ValidateMWONameExist/{MWOId}/{mwoname}");
            return await httpresult.ToObject<bool>();
        }
    }
}
