using Domain.Entities.Data;

namespace Application.Interfaces
{
    public interface IMWORepository : IRepository
    {
        Task UpdateMWO(MWO entity);
        Task AddMWO(MWO mWO);
        Task<bool> ReviewIfNameExist(string name);
        Task<bool> ReviewIfNumberExist(string cecNumber);
        Task<bool> ReviewIfNameExist(Guid id,string name);
               
        Task<MWO> GetMWOById(Guid id);
        Task<MWO> GetMWOWithItemsById(Guid id);
        Task<IQueryable<PurchaseOrder>> GetPurchaseOrdersByMWOId(Guid MWOId);
        Task<IQueryable<BudgetItem>> GetBudgetItemsByMWOId(Guid MWOId);
        Task<IEnumerable<MWO>> GetMWOList();
        Task<BudgetItem> GetBudgetItemsSalary(Guid MWOId);
        Task<BudgetItem> GetBudgetItemsContingency(Guid MWOId);
        Task UpdateBudgetItem(BudgetItem entity);
    }
}
