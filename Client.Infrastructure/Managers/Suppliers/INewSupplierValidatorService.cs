using Client.Infrastructure.Services;

namespace Client.Infrastructure.Managers.Suppliers
{
    public interface INewSupplierValidatorService : IManager
    {
        Task<bool> ReviewIfNameExist(string name);
        Task<bool> ReviewIfNameExist(Guid SupplierId, string Name);
        Task<bool> ReviewIfVendorCodeExist(string vendorcode);
        Task<bool> ReviewIfEmailExist(string? email);
        Task<bool> ReviewIfVendorCodeExist(Guid SupplierId, string vendorcode);
        Task<bool> ReviewIfEmailExist(Guid SupplierId, string? email);
    }
    public class NewSupplierValidatorService : INewSupplierValidatorService
    {
        IHttpClientService Http;


        public NewSupplierValidatorService(IHttpClientService http)
        {
            this.Http = http;
        }
        public async Task<bool> ReviewIfNameExist(string name)
        {
            var httpresult = await Http.GetAsync($"SupplierValidator/ValidateNameExist/{name}");

            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ReviewIfNameExist(Guid SupplierId, string Name)
        {
            var httpresult = await Http.GetAsync($"SupplierValidator/ValidateNameExist/{SupplierId}/{Name}");

            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ReviewIfVendorCodeExist(string vendorcode)
        {
            var httpresult = await Http.GetAsync($"SupplierValidator/ValidateVendorCodeExist/{vendorcode}");

            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ReviewIfEmailExist(string? email)
        {
            var httpresult = await Http.GetAsync($"SupplierValidator/ValidateEmailExist/{email}");

            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ReviewIfVendorCodeExist(Guid SupplierId, string vendorcode)
        {
            var httpresult = await Http.GetAsync($"SupplierValidator/ValidateVendorCodeExist/{SupplierId}/{vendorcode}");

            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ReviewIfEmailExist(Guid SupplierId, string? email)
        {
            var httpresult = await Http.GetAsync($"SupplierValidator/ValidateEmailExist/{SupplierId}/{email}");

            return await httpresult.ToObject<bool>();
        }
    }
}
