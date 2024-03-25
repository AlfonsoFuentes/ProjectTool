using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;
using Shared.Models.PurchaseOrders.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Infrastructure.Managers.PurchaseOrders
{

    public interface IPurchaseOrderValidator : IManager
    {
        Task<bool> ValidateNameExistInPurchaseOrder(string name);
        Task<bool> ValidatePurchaseRequisitionExistInPurchaseOrder(string Purchaserequisition);
        Task<bool> ValidateNameExistInPurchaseOrder(Guid PurchaseOrderId, string name);
        Task<bool> ValidatePurchaseRequisitionExistInPurchaseOrder(Guid PurchaseOrderId, string purchaserequisition);
        Task<bool> ValidatePONumberExistInPurchaseOrder(Guid PurchaseOrderId, string ponumber);
    }
    public class PurchaseOrderValidator : IPurchaseOrderValidator
    {
        private HttpClient Http;
        public PurchaseOrderValidator(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }
        public async Task<bool> ValidateNameExistInPurchaseOrder(string name)
        {
          
            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidateNameExist/{name}");
            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ValidatePurchaseRequisitionExistInPurchaseOrder(string name)
        {
          
            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidatePurchaseRequisitionExist/{name}");
            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ValidateNameExistInPurchaseOrder(Guid PurchaseOrderId, string name)
        {
          
            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidateNameExist/{name}/{PurchaseOrderId}");
            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ValidatePurchaseRequisitionExistInPurchaseOrder(Guid PurchaseOrderId, string purchaserequisition)
        {
         
            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidatePurchaseRequisitionExist/{purchaserequisition}/{PurchaseOrderId}");
            return await httpresult.ToObject<bool>();
        }
        public async Task<bool> ValidatePONumberExistInPurchaseOrder(Guid PurchaseOrderId, string ponumber)
        {
         
            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidatePONumberExist/{ponumber}/{PurchaseOrderId}");
            return await httpresult.ToObject<bool>();
        }
    }
}
