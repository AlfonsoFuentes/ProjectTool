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
       
        Task<IResult<PurchaseOrdersListResponse>> GetAllPurchaseOrders();
       
        Task<IResult<ApprovedRegularPurchaseOrderRequest>> GetPurchaseOrderToApproveById(Guid PurchaseOrderId);
        Task<IResult<ReceiveRegularPurchaseOrderRequest>> GetPurchaseOrderToReceiveById(Guid PurchaseOrderId);

        Task<IResult<EditPurchaseOrderRegularCreatedRequest>> GetPurchaseOrderCreatedToEdit(Guid PurchaseOrderId);
        Task<IResult<EditPurchaseOrderRegularApprovedRequest>> GetPurchaseOrderApprovedToEdit(Guid PurchaseOrderId);
        Task<IResult<EditPurchaseOrderRegularClosedRequest>> GetPurchaseOrderClosedToEdit(Guid PurchaseOrderId);

        Task<IResult<EditTaxPurchaseOrderRequest>> GetPurchaseOrderTaxToEdit(Guid PurchaseOrderId);
        Task<IResult<EditCapitalizedSalaryPurchaseOrderRequest>> GetPurchaseOrderCapitalizedSalarToEdit(Guid PurchaseOrderId);
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
            CreatedRegularPurchaseOrderRequestDto model = new ();
            model.ConvertToDto(request);
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/CreateRegularPurchaseOrder", model);
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

        public async Task<IResult<ApprovedRegularPurchaseOrderRequest>> GetPurchaseOrderToApproveById(Guid PurchaseOrderId)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetPurchaseOrderToApprove/{PurchaseOrderId}");
            return await httpresult.ToResult<ApprovedRegularPurchaseOrderRequest>();
        }

        public async Task<IResult> EditPurchaseOrderCreated(EditPurchaseOrderRegularCreatedRequest request)
        {
            EditPurchaseOrderRegularCreatedRequestDto model = new();
            model.ConvertToDto(request);
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/EditPurchaseOrderCreated", model);
            return await httpresult.ToResult();
        }

       

        public async Task<IResult> ApproveRegularPurchaseOrder(ApprovedRegularPurchaseOrderRequest request)
        {
            ApprovedRegularPurchaseOrderRequestDto model = new();
            model.ConvertToDto(request);

            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/ApproveRegularPurchaseOrder", model);
            return await httpresult.ToResult();
        }
        public async Task<IResult> ApprovePurchaseOrderForAlteration(ApprovedRegularPurchaseOrderRequest request)
        {
            ApprovedRegularPurchaseOrderRequestDto model = new();
            model.ConvertToDto(request);
            var httpresultAlteration = await Http.PostAsJsonAsync($"PurchaseOrder/ApprovePurchaseOrderForAlteration", model);
            return await httpresultAlteration.ToResult();
        }
        public async Task<IResult> EditPurchaseOrderApproved(EditPurchaseOrderRegularApprovedRequest request)
        {
            EditPurchaseOrderRegularApprovedRequestDto model= new();
            model.ConvertToDto(request);
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/EditPurchaseOrderApproved", model);
            return await httpresult.ToResult();
        }

        public async Task<IResult<ReceiveRegularPurchaseOrderRequest>> GetPurchaseOrderToReceiveById(Guid PurchaseOrderId)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrder/GetReceivePurchaseOrder/{PurchaseOrderId}");
            return await httpresult.ToResult<ReceiveRegularPurchaseOrderRequest>();
        }

        public async Task<IResult> ReceivePurchaseOrder(ReceiveRegularPurchaseOrderRequest request)
        {
            ReceiveRegularPurchaseOrderRequestDto model = new();
            model.ConvertToDto(request);
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/ReceivePurchaseOrder", model);
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
            EditPurchaseOrderRegularClosedRequestDto model = new();
            model.ConvertToDto(request);
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/EditPurchaseOrderClosed", model);
            return await httpresult.ToResult();
        }

        //public async Task<IResult<ClosedPurchaseOrderRequest>> GetPurchaseOrderClosedById(Guid PurchaseOrderId)
        //{
        //    var httpresult = await Http.GetAsync($"PurchaseOrder/GetClosedPurchaseOrder/{PurchaseOrderId}");
        //    return await httpresult.ToResult<ClosedPurchaseOrderRequest>();
        //}

        public async Task<IResult> CreatePurchaseOrderCapitalizedSalary(CreateCapitalizedSalaryPurchaseOrderRequest request)
        {
            CreateCapitalizedSalaryPurchaseOrderRequestDto model = new();
            model.ConverToDto(request);
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/CreatePurchaseOrderCapitalizedSalary", model);
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
            EditCapitalizedSalaryPurchaseOrderRequestDto model = new();
            model.ConverToDto(request);
            var httpresult = await Http.PostAsJsonAsync($"PurchaseOrder/EditPurchaseOrderCapitalizedSalary", model);
            return await httpresult.ToResult();
        }

       
    }
}
