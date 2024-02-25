using Azure.Core;
using Shared.Models.PurchaseOrders.Requests.Approves;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;
using Shared.Models.PurchaseOrders.Requests.Closeds;
using Shared.Models.PurchaseOrders.Requests.Create;
using Shared.Models.PurchaseOrders.Requests.Receives;
using Shared.Models.PurchaseOrders.Requests.Taxes;
using Shared.Models.PurchaseOrders.Responses;

namespace Client.Infrastructure.Managers.PurchaseOrders
{
    public interface IPurchaseOrderService : IManager
    {
        Task<IResult> ReceivePurchaseOrder(ReceivePurchaseOrderRequest request);
        Task<IResult> ApprovePurchaseOrder(ApprovePurchaseOrderRequest request);
        Task<IResult> EditPurchaseOrderCreated(EditPurchaseOrderCreatedRequest request);
        Task<IResult> EditPurchaseOrderApproved(ApprovePurchaseOrderRequest request);
        Task<IResult> EditPurchaseOrderClosed(ClosedPurchaseOrderRequest request);
        Task<IResult> CreatePurchaseOrder(CreatePurchaseOrderRequest request);
        Task<IResult> CreatePurchaseOrderCapitalizedSalary(CreateCapitalizedSalaryPurchaseOrderRequest request);
        Task<IResult> CreateTaxPurchaseOrder(CreateTaxPurchaseOrderRequest request);
        Task<IResult<DataForCreatePurchaseOrder>> GetDataForCreatePurchaseOrder(Guid BudgetItemId);
        Task<IResult<DataForEditPurchaseOrder>> GetDataForEditPurchaseOrder(Guid PurchaseOrderId);
        Task<IResult<PurchaseOrdersListResponse>> GetAllPurchaseOrders();
        Task<IResult<ApprovePurchaseOrderRequest>> GetPurchaseOrderToApproveById(Guid PurchaseOrderId);
        Task<IResult<ReceivePurchaseOrderRequest>> GetPurchaseOrderToReceiveById(Guid PurchaseOrderId);
        Task<IResult<ClosedPurchaseOrderRequest>> GetPurchaseOrderClosedById(Guid PurchaseOrderId);
        Task<IResult> ApprovePurchaseOrderForAlteration(ApprovePurchaseOrderRequest request);
    }
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private HttpClient Http;

        public PurchaseOrderService(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }
        public async Task<IResult<DataForCreatePurchaseOrder>> GetDataForCreatePurchaseOrder(Guid BudgetItemId)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetCreatePurchaseOrder/{BudgetItemId}");
            return await httpresult.ToResult<DataForCreatePurchaseOrder>();
        }
        public async Task<IResult> CreatePurchaseOrder(CreatePurchaseOrderRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/CreatePurchaseOrder", request);
            return await httpresult.ToResult();
        }
        public async Task<IResult> CreateTaxPurchaseOrder(CreateTaxPurchaseOrderRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/CreateTaxPurchaseOrder", request);
            return await httpresult.ToResult();
        }

        public async Task<IResult<PurchaseOrdersListResponse>> GetAllPurchaseOrders()
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetAllPurchaseOrder");
            return await httpresult.ToResult<PurchaseOrdersListResponse>();
        }

        public async Task<IResult<ApprovePurchaseOrderRequest>> GetPurchaseOrderToApproveById(Guid PurchaseOrderId)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetApprovePurchaseOrder/{PurchaseOrderId}");
            return await httpresult.ToResult<ApprovePurchaseOrderRequest>();
        }

        public async Task<IResult> EditPurchaseOrderCreated(EditPurchaseOrderCreatedRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/EditPurchaseOrderCreated", request);
            return await httpresult.ToResult();
        }

        public async Task<IResult<DataForEditPurchaseOrder>> GetDataForEditPurchaseOrder(Guid PurchaseOrderId)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetDataForEditPurchaseOrder/{PurchaseOrderId}");
            return await httpresult.ToResult<DataForEditPurchaseOrder>();
        }

        public async Task<IResult> ApprovePurchaseOrder(ApprovePurchaseOrderRequest request)
        {
           
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/ApprovePurchaseOrder", request);
            return await httpresult.ToResult();
        }
        public async Task<IResult> ApprovePurchaseOrderForAlteration(ApprovePurchaseOrderRequest request)
        {
            var httpresultAlteration = await Http.PostAsJsonAsync($"PurchaseOrder/ApprovePurchaseOrderForAlteration", request);
            return await httpresultAlteration.ToResult();
        }
        public async Task<IResult> EditPurchaseOrderApproved(ApprovePurchaseOrderRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/EditPurchaseOrderApproved", request);
            return await httpresult.ToResult();
        }

        public async Task<IResult<ReceivePurchaseOrderRequest>> GetPurchaseOrderToReceiveById(Guid PurchaseOrderId)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetReceivePurchaseOrder/{PurchaseOrderId}");
            return await httpresult.ToResult<ReceivePurchaseOrderRequest>();
        }

        public async Task<IResult> ReceivePurchaseOrder(ReceivePurchaseOrderRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/ReceivePurchaseOrder", request);
            return await httpresult.ToResult();
        }

        public async Task<IResult> EditPurchaseOrderClosed(ClosedPurchaseOrderRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/EditPurchaseOrderClosed", request);
            return await httpresult.ToResult();
        }

        public async Task<IResult<ClosedPurchaseOrderRequest>> GetPurchaseOrderClosedById(Guid PurchaseOrderId)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetClosedPurchaseOrder/{PurchaseOrderId}");
            return await httpresult.ToResult<ClosedPurchaseOrderRequest>();
        }

        public async Task<IResult> CreatePurchaseOrderCapitalizedSalary(CreateCapitalizedSalaryPurchaseOrderRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/CreatePurchaseOrderCapitalizedSalary", request);
            return await httpresult.ToResult();
        }
    }
}
