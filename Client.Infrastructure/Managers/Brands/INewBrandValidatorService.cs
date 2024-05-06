namespace Client.Infrastructure.Managers.Brands
{
    public interface INewBrandValidatorService : IManager
    {
        Task<bool> ReviewIfNameExist(string name);
        Task<bool> ReviewIfNameExist(Guid BranId, string Name);
    }
    public class NewBrandValidatorService : INewBrandValidatorService
    {
        IHttpClientService Http;

        public NewBrandValidatorService(IHttpClientService http)
        {
            this.Http = http;
        }

        public async Task<bool> ReviewIfNameExist(string name)
        {

            var httpresult = await Http.GetAsync($"BrandValidator/ValidateNameExist/{name}");

            return await httpresult.ToObject<bool>();

        }

        public async Task<bool> ReviewIfNameExist(Guid BranId, string Name)
        {
            var httpresult = await Http.GetAsync($"BrandValidator/ValidateNameExist/{BranId}/{Name}");


            return await httpresult.ToObject<bool>();
        }
    }
}
