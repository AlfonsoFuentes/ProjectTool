

using Azure.Core;
using Shared.NewModels.PurchaseOrders.Request;
using Shared.NewModels.PurchaseOrders.Responses;

namespace Client.Infrastructure.Managers.PurchaseOrders
{
    public interface INewPurchaseOrderService : IManager
    {
        Task<IResult<NewPriorPurchaseOrderCreatedResponse>> GetAllCreated();
        Task<IResult<NewPriorPurchaseOrderApprovedResponse>> GetAllApproved();
        Task<IResult<NewPriorPurchaseOrderClosedResponse>> GetAllClosed();

        Task<IResult> CreatePurchaseOrderAsync(NewPurchaseOrderCreateRequest request);
        Task<IResult> DeletePurchaseOrderAsync(NewPurchaseOrderDeleteRequest request);
        Task<IResult> ApprovePurchaseOrderAsync(NewPurchaseOrderApproveRequest request);
        Task<IResult> UnApprovePurchaseOrderAsync(NewPurchaseOrderUnApproveRequest request);
        Task<IResult> ReOpenPurchaseOrderAsync(NewPurchaseOrderReOpenRequest request);
        Task<IResult<NewPurchaseOrderApproveRequest>> GetPurchaseOrderToApproved(Guid PurchaseOrderId);
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
            var result = await http.GetAsync($"{ClientEndPoint.NewPurchaseOrder.GetPurchaseOrderToApprove}/{PurchaseOrderId}");
            return await result.ToResult<NewPurchaseOrderApproveRequest>();
        }
    }
}
