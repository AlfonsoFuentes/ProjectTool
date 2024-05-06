namespace Client.Infrastructure.Managers.VersionSoftwares
{
    public interface INewVersionSoftwareValidatorService : IManager
    {
        Task<bool> ReviewIfNameExist(string name);
        Task<bool> ReviewIfNameExist(Guid SoftwareVersionId, string Name);
    }
    public class NewVersionSoftwareValidatorService : INewVersionSoftwareValidatorService
    {
        IHttpClientService Http;

        public NewVersionSoftwareValidatorService(IHttpClientService http)
        {
            Http = http;
        }

        public async Task<bool> ReviewIfNameExist(string name)
        {

            var httpresult = await Http.GetAsync($"SoftwareVersionValidator/ValidateNameExist/{name}");

            return await httpresult.ToObject<bool>();

        }

        public async Task<bool> ReviewIfNameExist(Guid BranId, string Name)
        {
            var httpresult = await Http.GetAsync($"SoftwareVersionValidator/ValidateNameExist/{BranId}/{Name}");


            return await httpresult.ToObject<bool>();
        }
    }
}
