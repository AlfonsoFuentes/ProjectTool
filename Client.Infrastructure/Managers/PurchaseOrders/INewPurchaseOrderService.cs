using Shared.NewModels.PurchaseOrders.Responses;

namespace Client.Infrastructure.Managers.PurchaseOrders
{
    public interface INewPurchaseOrderService : IManager
    {
        Task<IResult<NewPriorPurchaseOrderCreatedResponse>> GetAllCreated();
        Task<IResult<NewPriorPurchaseOrderApprovedResponse>> GetAllApproved();
        Task<IResult<NewPriorPurchaseOrderClosedResponse>> GetAllClosed();
        Task<IResult> CreatePurchaseOrderSalaryAsync(NewPurchaseOrderCreateSalaryRequest request);
        Task<IResult> CreatePurchaseOrderAsync(NewPurchaseOrderCreateRequest request);
        Task<IResult> EditCreatedPurchaseOrderAsync(NewPurchaseOrderEditCreateRequest request);
 
        Task<IResult> DeletePurchaseOrderAsync(NewPurchaseOrderDeleteRequest request);
        Task<IResult> ApprovePurchaseOrderAsync(NewPurchaseOrderApproveRequest request);
        Task<IResult> UnApprovePurchaseOrderAsync(NewPurchaseOrderUnApproveRequest request);
        Task<IResult> ReOpenPurchaseOrderAsync(NewPurchaseOrderReOpenRequest request);
        
       
        Task<IResult> EditApprovedPurchaseOrderAsync(NewPurchaseOrderEditApproveRequest request);
        Task<IResult> EditSalaryPurchaseOrderAsync(NewPurchaseOrderEditSalaryRequest request);
        Task<IResult<NewPurchaseOrderApproveRequest>> GetPurchaseOrderToApproved(Guid PurchaseOrderId);

        Task<IResult<NewPurchaseOrderEditCreateRequest>> GetPurchaseOrderCreatedToEdit(Guid PurchaseOrderId);
        Task<IResult> ReceivePurchaseOrderAsync(NewPurchaseOrderReceiveRequest request);
        Task<IResult<NewPurchaseOrderReceiveRequest>> GetPurchaseOrderToReceive(Guid PurchaseOrderId);
      
        Task<IResult<NewPurchaseOrderEditReceiveRequest>> GetPurchaseOrderToEditeReceive(Guid PurchaseOrderId);
        Task<IResult> EditReceivePurchaseOrderAsync(NewPurchaseOrderEditReceiveRequest request);
      
        Task<IResult<NewPurchaseOrderEditApproveRequest>> GetPurchaseOrderToEditApproved(Guid PurchaseOrderId);
        Task<IResult<NewPurchaseOrderEditSalaryRequest>> GetPurchaseOrderToEditSalary(Guid PurchaseOrderId);
    }
    public class NewPurchaseOrderService : INewPurchaseOrderService
    {
        IHttpClientService http;

        public NewPurchaseOrderService(IHttpClientService http)
        {
            this.http = http;
        }


        public async Task<IResult> CreatePurchaseOrderAsync(NewPurchaseOrderCreateRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewPurchaseOrder.Create, request);
            return await result.ToResult();
        }
        public async Task<IResult> DeletePurchaseOrderAsync(NewPurchaseOrderDeleteRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewPurchaseOrder.Delete, request);
            return await result.ToResult();
        }

        public async Task<IResult<NewPriorPurchaseOrderCreatedResponse>> GetAllCreated()
        {
            var result = await http.GetAsync(ClientEndPoint.NewPurchaseOrder.GetAllCreated);
            return await result.ToResult<NewPriorPurchaseOrderCreatedResponse>();
        }

        public async Task<IResult<NewPriorPurchaseOrderApprovedResponse>> GetAllApproved()
        {
            var result = await http.GetAsync(ClientEndPoint.NewPurchaseOrder.GetAllApproved);
            return await result.ToResult<NewPriorPurchaseOrderApprovedResponse>();
        }

        public async Task<IResult<NewPriorPurchaseOrderClosedResponse>> GetAllClosed()
        {
            var result = await http.GetAsync(ClientEndPoint.NewPurchaseOrder.GetAllClosed);
            return await result.ToResult<NewPriorPurchaseOrderClosedResponse>();
        }

        public async Task<IResult> ApprovePurchaseOrderAsync(NewPurchaseOrderApproveRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewPurchaseOrder.Approve, request);
            return await result.ToResult();
        }

        public async Task<IResult> UnApprovePurchaseOrderAsync(NewPurchaseOrderUnApproveRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewPurchaseOrder.UnApprove, request);
            return await result.ToResult();
        }

        public async Task<IResult> ReOpenPurchaseOrderAsync(NewPurchaseOrderReOpenRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewPurchaseOrder.ReOpen, request);
            return await result.ToResult();
        }

        public async Task<IResult<NewPurchaseOrderApproveRequest>> GetPurchaseOrderToApproved(Guid PurchaseOrderId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.NewPurchaseOrder.GetCreateToApprove}/{PurchaseOrderId}");
            return await result.ToResult<NewPurchaseOrderApproveRequest>();
        }

       
        public async Task<IResult> CreatePurchaseOrderSalaryAsync(NewPurchaseOrderCreateSalaryRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewPurchaseOrder.CreateSalary, request);
            return await result.ToResult();
        }

        public async Task<IResult> EditApprovedPurchaseOrderAsync(NewPurchaseOrderEditApproveRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewPurchaseOrder.EditApproved, request);
            return await result.ToResult();
        }

        public async Task<IResult<NewPurchaseOrderEditApproveRequest>> GetPurchaseOrderToEditApproved(Guid PurchaseOrderId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.NewPurchaseOrder.GetApprovedToEdit}/{PurchaseOrderId}");
            return await result.ToResult<NewPurchaseOrderEditApproveRequest>();
        }

        public async Task<IResult> EditSalaryPurchaseOrderAsync(NewPurchaseOrderEditSalaryRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewPurchaseOrder.EditSalary, request);
            return await result.ToResult();
        }

        public async Task<IResult<NewPurchaseOrderEditSalaryRequest>> GetPurchaseOrderToEditSalary(Guid PurchaseOrderId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.NewPurchaseOrder.GetSalaryToEdit}/{PurchaseOrderId}");
            return await result.ToResult<NewPurchaseOrderEditSalaryRequest>();
        }

        public async Task<IResult<NewPurchaseOrderEditCreateRequest>> GetPurchaseOrderCreatedToEdit(Guid PurchaseOrderId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.NewPurchaseOrder.GetCreateToEdit}/{PurchaseOrderId}");
            return await result.ToResult<NewPurchaseOrderEditCreateRequest>();
        }

        public async Task<IResult> EditCreatedPurchaseOrderAsync(NewPurchaseOrderEditCreateRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewPurchaseOrder.EditCreate, request);
            return await result.ToResult();
        }

        public async Task<IResult> ReceivePurchaseOrderAsync(NewPurchaseOrderReceiveRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewPurchaseOrder.Receive, request);
            return await result.ToResult();
        }

        public async Task<IResult<NewPurchaseOrderReceiveRequest>> GetPurchaseOrderToReceive(Guid PurchaseOrderId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.NewPurchaseOrder.GetApprovedToReceive}/{PurchaseOrderId}");
            return await result.ToResult<NewPurchaseOrderReceiveRequest>();
        }

        public async Task<IResult<NewPurchaseOrderEditReceiveRequest>> GetPurchaseOrderToEditeReceive(Guid PurchaseOrderId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.NewPurchaseOrder.GetReceivedToEdit}/{PurchaseOrderId}");
            return await result.ToResult<NewPurchaseOrderEditReceiveRequest>();
        }

        public async Task<IResult> EditReceivePurchaseOrderAsync(NewPurchaseOrderEditReceiveRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewPurchaseOrder.EditReceive, request);
            return await result.ToResult();
        }
    }

}
