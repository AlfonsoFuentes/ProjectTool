

using Azure.Core;
using Shared.NewModels.BudgetItems.Request;
using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.MWOs.Reponses;
using static System.Net.WebRequestMethods;

namespace Client.Infrastructure.Managers.BudgetItems
{
    public interface INewBudgetItemService : IManager
    {
        Task<IResult> UnApproveMWO(NewMWOUnApproveRequest request);
        Task<IResult> UpdateBudgetItem(NewBudgetItemMWOUpdateRequest request);
        Task<IResult> CreateBudgetItem(NewBudgetItemMWOCreatedRequest request);
        Task<IResult<NewMWOCreatedWithItemsResponse>> GetAllMWOCreatedWithItems(Guid MWOId);
        Task<IResult<NewMWOCreatedWithItemsResponse>> GetAllMWOCreatedWithItemsToApplyTaxes(Guid MWOId);
        Task<IResult> Delete(NewBudgetItemMWOCreatedResponse request);
        Task<IResult<NewBudgetItemMWOUpdateRequest>> GetBudgetItemToUpdate(Guid BudgetItemId);
        Task<IResult<NewMWOApprovedReponse>> GetAllMWOApprovedWithItems(Guid MWOId);
        Task<IResult<NewBudgetItemToCreatePurchaseOrderResponse>> GetBudgetItemMWOApprovedById(Guid BudgetItemId);

        Task<IResult<NewMWOApprovedForCreatePurchaseOrderReponse>> GetAllMWOApprovedForCreatePurchaseOrder(Guid MWOId);
    }
    public class NewBudgetItemService : INewBudgetItemService
    {
        IHttpClientService http;

        public NewBudgetItemService(IHttpClientService http)
        {
            this.http = http;
        }
        public async Task<IResult> CreateBudgetItem(NewBudgetItemMWOCreatedRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewBudgetItem.Create, request);
            return await result.ToResult();
        }
        public async Task<IResult<NewMWOCreatedWithItemsResponse>> GetAllMWOCreatedWithItems(Guid MWOId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.NewBudgetItem.GetAllCreatedWithItems}/{MWOId}");
            return await result.ToResult<NewMWOCreatedWithItemsResponse>();
        }
        public async Task<IResult<NewBudgetItemMWOUpdateRequest>> GetBudgetItemToUpdate(Guid BudgetItemId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.NewBudgetItem.GetByIdToUpdate}/{BudgetItemId}");
            return await result.ToResult<NewBudgetItemMWOUpdateRequest>();
        }
        public async Task<IResult> Delete(NewBudgetItemMWOCreatedResponse request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewBudgetItem.Delete, request);
            return await result.ToResult();
        }

        public async Task<IResult> UpdateBudgetItem(NewBudgetItemMWOUpdateRequest request)
        {
            var result = await http.PostAsJsonAsync(ClientEndPoint.NewBudgetItem.Update, request);
            return await result.ToResult();
        }

        public async Task<IResult<NewMWOCreatedWithItemsResponse>> GetAllMWOCreatedWithItemsToApplyTaxes(Guid MWOId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.NewBudgetItem.GetMWOItemsToApplyTaxes}/{MWOId}");
            return await result.ToResult<NewMWOCreatedWithItemsResponse>();
        }

        public async Task<IResult<NewMWOApprovedReponse>> GetAllMWOApprovedWithItems(Guid MWOId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.NewBudgetItem.GetAllApprovedWithItems}/{MWOId}");
            return await result.ToResult<NewMWOApprovedReponse>();
        }


        public async Task<IResult> UnApproveMWO(NewMWOUnApproveRequest request)
        {
            var result = await http.PostAsJsonAsync($"{ClientEndPoint.NewMWO.UnApprove}", request);
            return await result.ToResult();
        }

        public async Task<IResult<NewBudgetItemToCreatePurchaseOrderResponse>> GetBudgetItemMWOApprovedById(Guid BudgetItemId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.NewBudgetItem.GetBudgetItemByIdMWOApproved}/{BudgetItemId}");
            return await result.ToResult<NewBudgetItemToCreatePurchaseOrderResponse>();
        }

        public async Task<IResult<NewMWOApprovedForCreatePurchaseOrderReponse>> GetAllMWOApprovedForCreatePurchaseOrder(Guid MWOId)
        {
            var result = await http.GetAsync($"{ClientEndPoint.NewBudgetItem.GetAllApprovedForCreatePurchaseOrder}/{MWOId}");
            return await result.ToResult<NewMWOApprovedForCreatePurchaseOrderReponse>();
        }
    }
}
