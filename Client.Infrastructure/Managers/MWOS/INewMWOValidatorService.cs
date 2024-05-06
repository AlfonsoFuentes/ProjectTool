namespace Client.Infrastructure.Managers.MWOS
{
    public interface INewMWOValidatorService : IManager
    {
        Task<bool> ValidateMWONameExist(string mwoname);
        Task<bool> ValidateMWONameExist(Guid MWOId, string mwoname);
        Task<bool> ValidateMWONumberExist(Guid MWOId, string mwonumber);
    }
    public class NewMWOValidatorService : INewMWOValidatorService
    {
        private IHttpClientService Http;
        public NewMWOValidatorService(IHttpClientService httpClientFactory)
        {
            Http = httpClientFactory;
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
