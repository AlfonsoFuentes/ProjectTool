using Domain.Entities.Data;

namespace Application.Interfaces
{
    public interface IBudgetItemRepository : IRepository
    {
        Task UpdateBudgetItem(BudgetItem entity);
        Task AddBudgetItem(BudgetItem BudgetItem);
        Task<bool> ReviewNameExist(string name);
        Task<bool> ReviewNameExist(Guid id, string name);
        Task<IQueryable<BudgetItem>> GetBudgetItemList(Guid MWOId);
        Task<BudgetItem> GetBudgetItemById(Guid id);
        Task<string> GetMWOName(Guid MWOId);
        Task<MWO> GetMWOWithItemsById(Guid MWOId);
        Task UpdateEngCostContingency(Guid MWOId);
        Task<double> GetSumEngConPercentage(Guid MWOId);
        Task<double> GetSumBudget(Guid MWOId);
        Task<double> GetSumTaxes(Guid MWOId);
    }
}
