namespace Client.Infrastructure.Managers.BudgetItems
{
    public interface IBudgetItemValidatorService : IManager
    {
        Task<bool> ValidateNameExist(Guid BudgetItemId, Guid MWOId, string mwoname);
        Task<bool> ValidateNameExist(Guid MWOId, string mwoname);
    }
    public class BudgetItemValidatorService: IBudgetItemValidatorService
    {
        private HttpClient Http;
        public BudgetItemValidatorService(IHttpClientFactory httpClientFactory)
        {
            Http = httpClientFactory.CreateClient("Auth");
        }
       
        public async Task<bool> ValidateNameExist(Guid MWOId, string mwoname)
        {
            var httpresult = await Http.GetAsync($"BudgetItemValidator/ValidateNameExist/{MWOId}/{mwoname}");
            return await httpresult.ToObject<bool>();
        }
        public async Task<bool> ValidateNameExist(Guid BudgetItemId, Guid MWOId, string mwoname)
        {
            var httpresult = await Http.GetAsync($"BudgetItemValidator/ValidateNameExist/{MWOId}/{BudgetItemId}/{mwoname}");
            return await httpresult.ToObject<bool>();
        }
    }
}
