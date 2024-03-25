using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Infrastructure.Managers.Brands
{
    public interface IBrandValidatorService:IManager
    {
        Task<bool> ReviewIfNameExist(string name);
        Task<bool> ReviewIfNameExist(Guid BranId, string Name);
    }
    public class BrandValidatorService: IBrandValidatorService
    {
        private HttpClient Http;

        public BrandValidatorService(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }

        public async Task<bool> ReviewIfNameExist(string name)
        {
           
            var httpresult = await Http.GetAsync($"BrandValidator/ValidateNameExist/{name}");

            return await httpresult.ToObject<bool>();
 
        }

        public async Task<bool> ReviewIfNameExist(Guid BranId,string Name)
        {
            var httpresult = await Http.GetAsync($"BrandValidator/ValidateNameExist/{BranId}/{Name}");
            

            return await httpresult.ToObject<bool>();
        }
    }
}
