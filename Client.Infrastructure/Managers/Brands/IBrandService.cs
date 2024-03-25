

namespace Client.Infrastructure.Managers.Brands
{
    public interface IBrandService : IManager
    {
        Task<IResult> UpdateBrand(UpdateBrandRequest request);
        Task<IResult> CreateBrand(CreateBrandRequest request);
        Task<IResult<BrandResponse>> CreateBrandForBudgetItem(CreateBrandRequest request);
       
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
                CreateBrandRequestDto model = new CreateBrandRequestDto();
                model.ConvertToDto(request);
                var httpresult = await Http.PostAsJsonAsync("Brand/CreateBrand", model);

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
                UpdateBrandRequestDto model = new();
                model.ConvertToDto(request);
                var httpresult = await Http.PostAsJsonAsync("Brand/UpdateBrand", model);

                return await httpresult.ToResult();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return await Result.FailAsync();

        }

      
        public async Task<IResult<List<BrandResponse>>> GetAllBrand()
        {
            var httpresult = await Http.GetAsync($"Brand/GetAll");
            return await httpresult.ToResult<List<BrandResponse>>();
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

        public async Task<IResult<BrandResponse>> CreateBrandForBudgetItem(CreateBrandRequest request)
        {
            try
            {
                CreateBrandRequestDto model = new CreateBrandRequestDto();
                model.ConvertToDto(request);
                var httpresult = await Http.PostAsJsonAsync("Brand/CreateBrandForBudgetItem", model);

                return await httpresult.ToResult<BrandResponse>();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return await Result<BrandResponse>.FailAsync();
        }
    }
}
