using Client.Infrastructure.Managers;
using Shared.Models.Brands;
using Shared.Models.BudgetItems;

namespace Client.Infrastructure.Managers.BudgetItems
{
    public interface IBudgetItemService : IManager
    {

        Task<IResult> CreateBudgetItem(CreateBudgetItemRequest request);
        Task<IResult> UpdateBudgetItem(UpdateBudgetItemRequest request);
        Task<IResult> UpdateBudgetItemMinimal(UpdateBudgetItemMinimalRequest request);
        Task<IResult<ListBudgetItemResponse>> GetAllBudgetItemByMWO(Guid MWOId);
        Task<IResult<UpdateBudgetItemRequest>> GetBudgetItemById(Guid Id);

        Task<IResult> Delete(BudgetItemResponse request);
        Task<IResult<DataforCreateBudgetItemResponse>> GetDataForCreateBudgetItem(Guid MWOId);
       

    }
    public class BudgetItemService : IBudgetItemService
    {
        private HttpClient Http;

        public BudgetItemService(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }
        public async Task<IResult> CreateBudgetItem(CreateBudgetItemRequest request)
        {
            if (request.IsRegularData)
            {
                var httpresult = await Http.PostAsJsonAsync("BudgetItem/CreateRegularItem", request);
                return await httpresult.ToResult();
            }
            else if (request.IsEquipmentData)
            {
                var httpresult = await Http.PostAsJsonAsync("BudgetItem/CreateEquipmentItem", request);
                return await httpresult.ToResult();
            }
            else if (request.IsTaxesData)
            {
                var httpresult = await Http.PostAsJsonAsync("BudgetItem/CreateTaxItem", request);
                return await httpresult.ToResult();
            }
            else if (request.IsEngContData)
            {
                var httpresult = await Http.PostAsJsonAsync("BudgetItem/CreateEngContItem", request);
                return await httpresult.ToResult();

            }
            else if (request.IsAlteration)
            {
                var httpresult = await Http.PostAsJsonAsync("BudgetItem/CreateAlterationItem", request);
                return await httpresult.ToResult();
            }
            return await Result.FailAsync();
        }

        public async Task<IResult> UpdateBudgetItem(UpdateBudgetItemRequest request)
        {
            if (request.IsRegularData)
            {
                var httpresult = await Http.PostAsJsonAsync("BudgetItem/UpdateRegularItem", request);
                return await httpresult.ToResult();
            }
            else if (request.IsEquipmentData)
            {
                var httpresult = await Http.PostAsJsonAsync("BudgetItem/UpdateEquipmentItem", request);
                return await httpresult.ToResult();
            }
            else if (request.IsTaxesData)
            {
                var httpresult = await Http.PostAsJsonAsync("BudgetItem/UpdateTaxItem", request);
                return await httpresult.ToResult();
            }
            else if (request.IsEngContData)
            {
                var httpresult = await Http.PostAsJsonAsync("BudgetItem/UpdateEngContItem", request);
                return await httpresult.ToResult();

            }
            else if (request.IsAlteration)
            {
                var httpresult = await Http.PostAsJsonAsync("BudgetItem/UpdateAlterationItem", request);
                return await httpresult.ToResult();
            }
            return await Result.FailAsync();
        }

        public async Task<IResult<ListBudgetItemResponse>> GetAllBudgetItemByMWO(Guid MWOId)
        {
            try
            {
                var httpresult = await Http.GetAsync($"BudgetItem/GetAllItems/{MWOId}");
                return await httpresult.ToResult<ListBudgetItemResponse>();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return Result<ListBudgetItemResponse>.Fail();

        }

        public async Task<IResult<UpdateBudgetItemRequest>> GetBudgetItemById(Guid Id)
        {
            var httpresult = await Http.GetAsync($"BudgetItem/{Id}");
            return await httpresult.ToResult<UpdateBudgetItemRequest>();
        }

        public async Task<IResult> Delete(BudgetItemResponse request)
        {
            var httpresult = await Http.PostAsJsonAsync($"BudgetItem/DeleteBudgetItem", request);
            return await httpresult.ToResult();
        }

      

        public async Task<IResult<DataforCreateBudgetItemResponse>> GetDataForCreateBudgetItem(Guid MWOId)
        {
            var httpresult = await Http.GetAsync($"BudgetItem/GetDataForCreateBudget/{MWOId}");
            return await httpresult.ToResult<DataforCreateBudgetItemResponse>();
        }

        public async Task<IResult> UpdateBudgetItemMinimal(UpdateBudgetItemMinimalRequest request)
        {
            var httpresult = await Http.PostAsJsonAsync("BudgetItem/UpdateMinimalItem", request);
            return await httpresult.ToResult();
        }
    }
}
