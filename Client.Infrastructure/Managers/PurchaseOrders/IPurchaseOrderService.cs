using Azure.Core;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits;
using Shared.Models.PurchaseOrders.Requests.Taxes;
using Shared.Models.PurchaseOrders.Responses;

namespace Client.Infrastructure.Managers.PurchaseOrders
{
    public interface IPurchaseOrderService : IManager
    {
        Task<IResult> ReceivePurchaseOrder(ReceiveRegularPurchaseOrderRequest request);
        Task<IResult> ApproveRegularPurchaseOrder(ApprovedRegularPurchaseOrderRequest request);
        Task<IResult> ApprovePurchaseOrderForAlteration(ApprovedRegularPurchaseOrderRequest request);
        Task<IResult> EditPurchaseOrderCreated(EditPurchaseOrderRegularCreatedRequest request);

       
        Task<IResult> EditPurchaseOrderApproved(EditPurchaseOrderRegularApprovedRequest request);
        Task<IResult> EditPurchaseOrderClosed(EditPurchaseOrderRegularClosedRequest request);
        Task<IResult> EditPurchaseOrderTax(EditTaxPurchaseOrderRequest request);
        Task<IResult> EditPurchaseOrderCapitalizedSalary(EditCapitalizedSalaryPurchaseOrderRequest request);
        Task<IResult> CreateRegularPurchaseOrder(CreatedRegularPurchaseOrderRequest request);
        Task<IResult> CreatePurchaseOrderCapitalizedSalary(CreateCapitalizedSalaryPurchaseOrderRequest request);
        Task<IResult> CreateTaxPurchaseOrder(CreateTaxPurchaseOrderRequest request);
        Task<IResult<BudgetItemsListForPurchaseordersResponse>> GetBudgetItemsToCreatePurchaseOrder(Guid BudgetItemId);
       
        Task<IResult<NewPurchaseOrdersListResponse>> GetAllPurchaseOrders();
       
        Task<IResult<ApprovedRegularPurchaseOrderRequest>> GetPurchaseOrderToApproveById(Guid PurchaseOrderId);
        Task<IResult<ReceiveRegularPurchaseOrderRequest>> GetPurchaseOrderToReceiveById(Guid PurchaseOrderId);

        Task<IResult<EditPurchaseOrderRegularCreatedRequest>> GetPurchaseOrderCreatedToEdit(Guid PurchaseOrderId);
        Task<IResult<EditPurchaseOrderRegularApprovedRequest>> GetPurchaseOrderApprovedToEdit(Guid PurchaseOrderId);
        Task<IResult<EditPurchaseOrderRegularClosedRequest>> GetPurchaseOrderClosedToEdit(Guid PurchaseOrderId);

        Task<IResult<EditTaxPurchaseOrderRequest>> GetPurchaseOrderTaxToEdit(Guid PurchaseOrderId);
        Task<IResult<EditCapitalizedSalaryPurchaseOrderRequest>> GetPurchaseOrderCapitalizedSalarToEdit(Guid PurchaseOrderId);
        Task<IResult> DeletePurchaseOrder(Guid PurchaseOrderId);
        Task<IResult> RecalculatePurchaseOrder();
    }
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private HttpClient Http;
  
        public PurchaseOrderService(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
       
        }
        public async Task<IResult<BudgetItemsListForPurchaseordersResponse>> GetBudgetItemsToCreatePurchaseOrder(Guid BudgetItemId)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetBudgetItemsToCreatePurchaseOrder/{BudgetItemId}");
            return await httpresult.ToResult<BudgetItemsListForPurchaseordersResponse>();
        }
        public async Task<IResult> CreateRegularPurchaseOrder(CreatedRegularPurchaseOrderRequest request)
        {
            
            
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/CreateRegularPurchaseOrder", request);
            return await httpresult.ToResult();
        }
        public async Task<IResult> CreateTaxPurchaseOrder(CreateTaxPurchaseOrderRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/CreateTaxPurchaseOrder", request);
            return await httpresult.ToResult();
        }

        public async Task<IResult<NewPurchaseOrdersListResponse>> GetAllPurchaseOrders()
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetAllPurchaseOrder");
            return await httpresult.ToResult<NewPurchaseOrdersListResponse>();
        }

        public async Task<IResult<ApprovedRegularPurchaseOrderRequest>> GetPurchaseOrderToApproveById(Guid PurchaseOrderId)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetPurchaseOrderToApprove/{PurchaseOrderId}");
            return await httpresult.ToResult<ApprovedRegularPurchaseOrderRequest>();
        }

        public async Task<IResult> EditPurchaseOrderCreated(EditPurchaseOrderRegularCreatedRequest request)
        {
            
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/EditPurchaseOrderCreated", request);
            return await httpresult.ToResult();
        }

       

        public async Task<IResult> ApproveRegularPurchaseOrder(ApprovedRegularPurchaseOrderRequest request)
        {
            

            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/ApproveRegularPurchaseOrder", request);
            return await httpresult.ToResult();
        }
        public async Task<IResult> ApprovePurchaseOrderForAlteration(ApprovedRegularPurchaseOrderRequest request)
        {
            
            var httpresultAlteration = await Http.PostAsJsonAsync($"PurchaseOrder/ApprovePurchaseOrderForAlteration", request);
            return await httpresultAlteration.ToResult();
        }
        public async Task<IResult> EditPurchaseOrderApproved(EditPurchaseOrderRegularApprovedRequest request)
        {
            
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/EditPurchaseOrderApproved", request);
            return await httpresult.ToResult();
        }

        public async Task<IResult<ReceiveRegularPurchaseOrderRequest>> GetPurchaseOrderToReceiveById(Guid PurchaseOrderId)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetReceivePurchaseOrder/{PurchaseOrderId}");
            return await httpresult.ToResult<ReceiveRegularPurchaseOrderRequest>();
        }

        public async Task<IResult> ReceivePurchaseOrder(ReceiveRegularPurchaseOrderRequest request)
        {
            
            
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/ReceivePurchaseOrder", request);
            return await httpresult.ToResult();
        }

        public async Task<IResult<EditPurchaseOrderRegularCreatedRequest>> GetPurchaseOrderCreatedToEdit(Guid PurchaseOrderId)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetPurchaseOrderCreatedToEdit/{PurchaseOrderId}");
            return await httpresult.ToResult<EditPurchaseOrderRegularCreatedRequest>();
        }

        public async Task<IResult<EditPurchaseOrderRegularApprovedRequest>> GetPurchaseOrderApprovedToEdit(Guid PurchaseOrderId)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetPurchaseOrderApprovedToEdit/{PurchaseOrderId}");
            return await httpresult.ToResult<EditPurchaseOrderRegularApprovedRequest>();
        }
        public async Task<IResult<EditPurchaseOrderRegularClosedRequest>> GetPurchaseOrderClosedToEdit(Guid PurchaseOrderId)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetPurchaseOrderClosedToEdit/{PurchaseOrderId}");
            return await httpresult.ToResult<EditPurchaseOrderRegularClosedRequest>();
        }
        public async Task<IResult> EditPurchaseOrderClosed(EditPurchaseOrderRegularClosedRequest request)
        {
            
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/EditPurchaseOrderClosed", request);
            return await httpresult.ToResult();
        }

      

        public async Task<IResult> CreatePurchaseOrderCapitalizedSalary(CreateCapitalizedSalaryPurchaseOrderRequest request)
        {
            
            
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/CreatePurchaseOrderCapitalizedSalary", request);
            return await httpresult.ToResult();
        }

        public async Task<IResult<EditTaxPurchaseOrderRequest>> GetPurchaseOrderTaxToEdit(Guid PurchaseOrderId)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetPurchaseOrderTaxToEdit/{PurchaseOrderId}");
            return await httpresult.ToResult<EditTaxPurchaseOrderRequest>();
        }

        public async Task<IResult<EditCapitalizedSalaryPurchaseOrderRequest>> GetPurchaseOrderCapitalizedSalarToEdit(Guid PurchaseOrderId)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetPurchaseOrderCapitalizedSalaryToEdit/{PurchaseOrderId}");
            return await httpresult.ToResult<EditCapitalizedSalaryPurchaseOrderRequest>();
        }

        public async Task<IResult> EditPurchaseOrderTax(EditTaxPurchaseOrderRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/EditPurchaseOrderTax", request);
            return await httpresult.ToResult();
        }

        public async Task<IResult> EditPurchaseOrderCapitalizedSalary(EditCapitalizedSalaryPurchaseOrderRequest request)
        {
            
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/EditPurchaseOrderCapitalizedSalary", request);
            return await httpresult.ToResult();
        }

        public async Task<IResult> DeletePurchaseOrder(Guid PurchaseOrderId)
        {
            DeletePurchaseorderRequest request = new DeletePurchaseorderRequest() { PurchaseOrderId = PurchaseOrderId };
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/RemovePurchaseOrder", request);
            return await httpresult.ToResult();
        }

        public async Task<IResult> RecalculatePurchaseOrder()
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/RecalculatePurchaseOrder");
            return await httpresult.ToResult();
        }
    }
}
