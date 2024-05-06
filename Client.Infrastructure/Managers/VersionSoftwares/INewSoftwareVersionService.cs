using Azure.Core;
using Shared.Models.UserManagements;
using Shared.NewModels.SoftwareVersion;

namespace Client.Infrastructure.Managers.VersionSoftwares
{
    public interface INewSoftwareVersionService : IManager
    {
        Task<IResult> CheckVersionForUser(AuthResponseDto request);
        Task<IResult> CreateVersion(NewSoftwareVersionCreateRequest request);
        Task<IResult<NewSoftwareVersionListResponse>> GetAll();
    }
    public class NewSoftwareVersionService : INewSoftwareVersionService
    {
        IHttpClientService http;

        public NewSoftwareVersionService(IHttpClientService http)
        {
            this.http = http;
        }

        public async Task<IResult> CreateVersion(NewSoftwareVersionCreateRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewSoftwareVersion.Create, request);
            return await result.ToResult();
        }

        public async Task<IResult<NewSoftwareVersionListResponse>> GetAll()
        {
            var result = await http.GetAsync(ClientEndPoint.NewSoftwareVersion.GetAll);
            return await result.ToResult<NewSoftwareVersionListResponse>();
        }

        public async Task<IResult> CheckVersionForUser(AuthResponseDto request)
        {
            var result = await http.PostAsJsonAsync("SoftwareVersion/CheckVersionForUser", request);
            return await result.ToResult();
        }
    }
}
