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
        Task<bool> ValidateNameExistInPurchaseOrder(Guid MWOId,string name);
        Task<bool> ValidatePurchaseRequisitionExistInPurchaseOrder(string Purchaserequisition);
        Task<bool> ValidateNameExistInPurchaseOrder(Guid MWOId, Guid PurchaseOrderId, string name);
        Task<bool> ValidatePurchaseRequisitionExistInPurchaseOrder(Guid PurchaseOrderId, string purchaserequisition);
        Task<bool> ValidatePONumberExistInPurchaseOrder(Guid PurchaseOrderId, string ponumber);
        Task<bool> ValidatePONumberExistInPurchaseOrder( string ponumber);
    }
    public class PurchaseOrderValidator : IPurchaseOrderValidator
    {
        private HttpClient Http;
        public PurchaseOrderValidator(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }
        public async Task<bool> ValidateNameExistInPurchaseOrder(Guid MWOId,string name)
        {
          
            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidateNameExist/{MWOId}/{name}");
            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ValidatePurchaseRequisitionExistInPurchaseOrder(string name)
        {
          
            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidatePurchaseRequisitionExist/{name}");
            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ValidateNameExistInPurchaseOrder(Guid MWOId, Guid PurchaseOrderId, string name)
        {
          
            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidateNameExist/{MWOId}/{PurchaseOrderId}/{name}");
            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ValidatePurchaseRequisitionExistInPurchaseOrder(Guid PurchaseOrderId, string purchaserequisition)
        {
         
            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidatePurchaseRequisitionExist/{PurchaseOrderId}/{purchaserequisition}");
            return await httpresult.ToObject<bool>();
        }
        public async Task<bool> ValidatePONumberExistInPurchaseOrder(Guid PurchaseOrderId, string ponumber)
        {
         
            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidatePONumberExist/{PurchaseOrderId}/{ponumber}");
            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ValidatePONumberExistInPurchaseOrder(string ponumber)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidatePONumberExist/{ponumber}");
            return await httpresult.ToObject<bool>();
        }
    }
}
