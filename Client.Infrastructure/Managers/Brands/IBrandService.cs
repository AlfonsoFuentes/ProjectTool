

namespace Client.Infrastructure.Managers.Brands
{
    public interface IBrandService : IManager
    {
        Task<IResult> UpdateBrand(UpdateBrandRequest request);
        Task<IResult> CreateBrand(CreateBrandRequest request);
        Task<bool> ReviewIfNameExist(string name);
        Task<bool> ReviewIfNameExist(UpdateBrandRequest name);
        Task<IResult<List<BrandResponse>>> GetAllBrand();
        Task<IResult<UpdateBrandRequest>> GetBrandById(Guid id);

        Task<IResult> Delete(BrandResponse request);
    }
    public class BrandService : IBrandService
    {
        private HttpClient Http;

        public BrandService(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }

        public async Task<IResult> CreateBrand(CreateBrandRequest request)
        {
            try
            {
                var httpresult = await Http.PostAsJsonAsync("Brand/CreateBrand", request);

                return await httpresult.ToResult();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return await Result.FailAsync();

        }
        public async Task<IResult> UpdateBrand(UpdateBrandRequest request)
        {
            try
            {
                var httpresult = await Http.PostAsJsonAsync("Brand/UpdateBrand", request);

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
            var httpresult = await Http.GetAsync($"Brand/CreateNameExist?name={name}");

            return await httpresult.ToObject<bool>();
        }

        public async Task<IResult<List<BrandResponse>>> GetAllBrand()
        {
            var httpresult = await Http.GetAsync($"Brand/GetAll");
            return await httpresult.ToResult<List<BrandResponse>>();
        }

        public async Task<bool> ReviewIfNameExist(UpdateBrandRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync($"Brand/UpdateNameExist", request);

            return await httpresult.ToObject<bool>();
        }

        public async Task<IResult<UpdateBrandRequest>> GetBrandById(Guid id)
        {
            var httpresult = await Http.GetAsync($"Brand/{id}");
            return await httpresult.ToResult<UpdateBrandRequest>();
        }

        public async Task<IResult> Delete(BrandResponse request)
        {
            var httpresult = await Http.PostAsJsonAsync($"Brand/DeleteBrand", request);
            return await httpresult.ToResult();
        }
    }
}
