using Client.Infrastructure.Managers;
using Shared.Models.Brands;
using Shared.Models.BudgetItems;

namespace Client.Infrastructure.Managers.BudgetItems
{
    public interface IBudgetItemService : IManager
    {

        Task<IResult> CreateBudgetItem(CreateBudgetItemRequest request);
        Task<IResult> CreateEngContItem(CreateEngineeringContingencyRequest request);
        Task<IResult<ListBudgetItemResponse>> GetAllBudgetItemByMWO(Guid MWOId);
        Task<IResult<BudgetItemResponse>> GetBudgetItemById(Guid Id);

        Task<IResult> Delete(BudgetItemResponse request);
        Task<IResult<double>> GetSumEngConPercentage(Guid MWOId);
        Task<IResult<double>> GetSumBudget(Guid MWOId);
        Task<IResult<double>> GetSumTaxes(Guid MWOId);
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
            if (request.IsEquipmentData)
            {
                var httpresult = await Http.PostAsJsonAsync("BudgetItem/CreateEquipmentItem", request);
                return await httpresult.ToResult();
            }
            if (request.IsTaxesData)
            {
                var httpresult = await Http.PostAsJsonAsync("BudgetItem/CreateTaxItem", request);
                return await httpresult.ToResult();
            }
            if (request.IsEngContData)
            {
                var httpresult = await Http.PostAsJsonAsync("BudgetItem/CreateEngContItem", request);
                return await httpresult.ToResult();

            }
            return await Result.FailAsync();
        }

        public async Task<IResult> CreateEngContItem(CreateEngineeringContingencyRequest request)
        {
            try
            {
                var httpresult = await Http.PostAsJsonAsync("BudgetItem/CreateEngContItem", request);

                return await httpresult.ToResult();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
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

        public async Task<IResult<BudgetItemResponse>> GetBudgetItemById(Guid Id)
        {
            var httpresult = await Http.GetAsync($"BudgetItem/{Id}");
            return await httpresult.ToResult<BudgetItemResponse>();
        }

        public async Task<IResult> Delete(BudgetItemResponse request)
        {
            var httpresult = await Http.PostAsJsonAsync($"BudgetItem/DeleteBudgetItem", request);
            return await httpresult.ToResult();
        }

        public async Task<IResult<double>> GetSumEngConPercentage(Guid MWOId)
        {
            var httpresult = await Http.GetAsync($"BudgetItem/SumPercEngContItems/{MWOId}");
            return await httpresult.ToResult<double>();
        }

        public async Task<IResult<double>> GetSumBudget(Guid MWOId)
        {
            var httpresult = await Http.GetAsync($"BudgetItem/SumBudgetItems/{MWOId}");
            return await httpresult.ToResult<double>();
        }

        public async Task<IResult<double>> GetSumTaxes(Guid BudgetItemId)
        {
            var httpresult = await Http.GetAsync($"BudgetItem/SumTaxesBudget/{BudgetItemId}");
            return await httpresult.ToResult<double>();
        }
    }
}
