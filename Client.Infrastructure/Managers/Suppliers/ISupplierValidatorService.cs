using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Infrastructure.Managers.Suppliers
{
    public interface ISupplierValidatorService:IManager
    {
        Task<bool> ReviewIfNameExist(string name);
        Task<bool> ReviewIfNameExist(Guid SupplierId, string Name);
        Task<bool> ReviewIfVendorCodeExist(string vendorcode);
        Task<bool> ReviewIfEmailExist(string? email);
        Task<bool> ReviewIfVendorCodeExist(Guid SupplierId, string vendorcode);
        Task<bool> ReviewIfEmailExist(Guid SupplierId, string? email);
    }
    public class SupplierValidatorService:ISupplierValidatorService
    {
        private HttpClient Http;

        public SupplierValidatorService(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }
        public async Task<bool> ReviewIfNameExist(string name)
        {
            var httpresult = await Http.GetAsync($"SupplierValidator/ValidateNameExist/{name}");

            return await httpresult.ToObject<bool>();
        }

        public async  Task<bool> ReviewIfNameExist(Guid SupplierId, string Name)
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
