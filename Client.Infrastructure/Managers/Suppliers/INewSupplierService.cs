

namespace Client.Infrastructure.Managers.Suppliers
{
    public interface INewSupplierService : IManager

    {
        Task<IResult> CreateSupplier(NewSupplierCreateRequest request);
        Task<IResult<NewSupplierResponse>> CreateSupplierAndReponse(NewSupplierCreateBasicRequest request);
        Task<IResult> Delete(NewSupplierResponse request);
        Task<IResult<NewSupplierListResponse>> GetAllSupplier();
        Task<IResult<NewSupplierUpdateRequest>> GetSupplierToUpdateById(Guid Id);
        Task<IResult> UpdateSupplier(NewSupplierUpdateRequest request);
        Task<IResult<FileResult>> ExporToExcel();
    }
    public class NewSupplierService : INewSupplierService
    {
        IHttpClientService http;

        public NewSupplierService(IHttpClientService http)
        {
            this.http = http;
        }

        public async Task<IResult> UpdateSupplier(NewSupplierUpdateRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewSupplier.Update, request);
            return await result.ToResult();
        }

        public async Task<IResult> CreateSupplier(NewSupplierCreateRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewSupplier.Create, request);
            return await result.ToResult();
        }

        public async Task<IResult<NewSupplierResponse>> CreateSupplierAndReponse(NewSupplierCreateBasicRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewSupplier.CreateAndReponse, request);
            return await result.ToResult<NewSupplierResponse>();
        }

        public async Task<IResult<NewSupplierListResponse>> GetAllSupplier()
        {
            var result = await http.GetAsync(ClientEndPoint.NewSupplier.GetAll);
            return await result.ToResult<NewSupplierListResponse>();
        }

        public async Task<IResult<NewSupplierUpdateRequest>> GetSupplierToUpdateById(Guid Id)
        {

            var result = await http.GetAsync($"{ClientEndPoint.Controller.Supplier}/{Id}");
            return await result.ToResult<NewSupplierUpdateRequest>();
        }

        public async Task<IResult> Delete(NewSupplierResponse request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewSupplier.Delete, request);
            return await result.ToResult();
        }

        public async Task<IResult<FileResult>> ExporToExcel()
        {
            var result = await http.GetAsync(ClientEndPoint.NewSupplier.ExportToExcel);
            return await result.ToResult<FileResult>();
        }
    }
}
