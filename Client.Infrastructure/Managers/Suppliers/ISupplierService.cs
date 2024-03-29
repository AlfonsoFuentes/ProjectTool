﻿using Client.Infrastructure.Managers;
using Shared.Models.Suppliers;

namespace Client.Infrastructure.Managers.Suppliers
{
    public interface ISupplierService : IManager
    {
        Task<IResult> UpdateSupplier(UpdateSupplierRequest request);
        Task<IResult> CreateSupplier(CreateSupplierRequest request);
        Task<bool> ReviewIfNameExist(string name);
        Task<bool> ReviewIfVendorCodeExist(string vendorcode);
        Task<bool> ReviewIfEmailExist(string? email);

        Task<IResult<List<SupplierResponse>>> GetAllSupplier();
        Task<IResult<UpdateSupplierRequest>> GetSupplierById(Guid id);

        Task<IResult> Delete(SupplierResponse request);
        Task<IResult<SupplierResponse>> CreateSupplierForPurchaseOrder(CreateSupplierForPurchaseOrderRequest request);
    }
    public class SupplierService : ISupplierService
    {
        private HttpClient Http;

        public SupplierService(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }

        public async Task<IResult> CreateSupplier(CreateSupplierRequest request)
        {
            try
            {
                var model = new CreateSupplierRequestDto();
                model.ConvertToDto(request);
                var httpresult = await Http.PostAsJsonAsync("Supplier/CreateSupplier", model);

                return await httpresult.ToResult();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return await Result.FailAsync();

        }
        public async Task<IResult<SupplierResponse>> CreateSupplierForPurchaseOrder(CreateSupplierForPurchaseOrderRequest request)
        {
            try
            {
                var model = new CreateSupplierRequestDto();
                model.ConvertToDto(request);
                var httpresult = await Http.PostAsJsonAsync("Supplier/CreateSupplierForPurchaseorder", model);

                return await httpresult.ToResult<SupplierResponse>();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return await Result<SupplierResponse>.FailAsync();

        }
        public async Task<IResult> UpdateSupplier(UpdateSupplierRequest request)
        {
            try
            {
                UpdateSupplierRequestDto model = new();
                model.ConvertToDto(request);
                
                var httpresult = await Http.PostAsJsonAsync("Supplier/UpdateSupplier", model);

                return await httpresult.ToResult();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return await Result.FailAsync();

        }
        public async Task<IResult> Delete(SupplierResponse request)
        {
            var httpresult = await Http.PostAsJsonAsync($"Supplier/DeleteSupplier", request);
            return await httpresult.ToResult();
        }
        public async Task<IResult<UpdateSupplierRequest>> GetSupplierById(Guid id)
        {
            var httpresult = await Http.GetAsync($"Supplier/{id}");
            return await httpresult.ToResult<UpdateSupplierRequest>();
        }
        public async Task<IResult<List<SupplierResponse>>> GetAllSupplier()
        {
            var httpresult = await Http.GetAsync($"Supplier/GetAll");
            return await httpresult.ToResult<List<SupplierResponse>>();
        }
        public async Task<bool> ReviewIfNameExist(string name)
        {
            var httpresult = await Http.GetAsync($"Supplier/CreateNameExist?name={name}");

            return await httpresult.ToObject<bool>();
        }



        public async Task<bool> ReviewIfVendorCodeExist(string vendorcode)
        {
            var httpresult = await Http.GetAsync($"Supplier/CreateVendorCodeExist?vendorcode={vendorcode}");

            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ReviewIfEmailExist(string? email)
        {
            var httpresult = await Http.GetAsync($"Supplier/CreateEmailExist?email={email}");

            return await httpresult.ToObject<bool>();
        }
    }
}
