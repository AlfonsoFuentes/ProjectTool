

namespace Client.Infrastructure.Managers.Brands
{
    public interface INewBrandService : IManager
    {
        Task<IResult> UpdateBrand(NewBrandUpdateRequest request);
        Task<IResult> CreateBrand(NewBrandCreateRequest request);
        Task<IResult<NewBrandResponse>> CreateBrandForBudgetItem(NewBrandCreateRequest request);

        Task<IResult<NewBrandListResponse>> GetAllBrand();
        Task<IResult<NewBrandUpdateRequest>> GetBrandToUpdateById(Guid id);

        Task<IResult> Delete(NewBrandResponse request);
    }
    public class NewBrandService : INewBrandService
    {
        IHttpClientService http;

        public NewBrandService(IHttpClientService http)
        {
            this.http = http;
        }

        public async Task<IResult> UpdateBrand(NewBrandUpdateRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewBrand.Update, request);
            return await result.ToResult();
        }

        public async Task<IResult> CreateBrand(NewBrandCreateRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewBrand.Create, request);
            return await result.ToResult();
        }

        public async Task<IResult<NewBrandResponse>> CreateBrandForBudgetItem(NewBrandCreateRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewBrand.CreateAndReponse, request);
            return await result.ToResult<NewBrandResponse>();
        }

        public async Task<IResult<NewBrandListResponse>> GetAllBrand()
        {
            var result = await http.GetAsync(ClientEndPoint.NewBrand.GetAll);
            return await result.ToResult<NewBrandListResponse>();
        }

        public async Task<IResult<NewBrandUpdateRequest>> GetBrandToUpdateById(Guid Id)
        {

            var result = await http.GetAsync($"{ClientEndPoint.Controller.Brand}/{Id}");
            return await result.ToResult<NewBrandUpdateRequest>();
        }

        public async Task<IResult> Delete(NewBrandResponse request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewBrand.Delete, request);
            return await result.ToResult();
        }
    }
}
