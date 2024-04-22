using Domain.Entities.Data;

namespace Application.Interfaces
{
    public interface IBudgetItemRepository : IRepository
    {
        Task UpdateMWO(MWO entity);
        Task UpdateBudgetItem(BudgetItem entity);
        Task AddBudgetItem(BudgetItem BudgetItem);
        Task AddTaxSelectedItem(TaxesItem BudgetItem);
        Task<bool> ReviewIfNameExist(Guid MWOId, string name);
        Task<bool> ReviewIfNameExist(Guid id, Guid MWOId, string name);
        Task<IQueryable<BudgetItem>> GetBudgetItemList(Guid MWOId);

        Task<IQueryable<BudgetItem>> GetBudgetItemsWithPurchaseordersList(Guid MWOId);
        Task<BudgetItem> GetBudgetItemById(Guid id);
        Task<string> GetMWOName(Guid MWOId);
        Task<MWO> GetMWOWithItemsById(Guid MWOId);
      
        Task<double> GetSumEngConPercentage(Guid MWOId);
        Task<double> GetSumBudget(Guid MWOId);
        Task<double> GetSumTaxes(Guid MWOId);
        Task<List<BudgetItem>> GetBudgetItemForTaxesList(Guid MWOId);
        Task<List<BudgetItem>> GetBudgetItemForApplyEngContList(Guid MWOId);
        Task<List<TaxesItem>> GetBudgetItemSelectedTaxesList(Guid Id);
        Task<BudgetItem> GetBudgetItemWithTaxesById(Guid id);
      
        Task<TaxesItem> GetTaxesItemById(Guid Id);
        Task<BudgetItem> GetBudgetItemWithBrandById(Guid id);
        Task<MWO> GetMWOById(Guid MWOId);

        Task<List<BudgetItem>> GetItemToApplyTaxes(Guid MWOId);
        Task<BudgetItem> GetMainBudgetTaxItemByMWO(Guid MWOId);
        
        Task UpdateTaxesAndEngineeringContingencyItems(Guid MWOId, CancellationToken cancellationToken);
        Task<List<PurchaseOrderItem>> GetPurchaseOrderItemsByMWOId(Guid MWOId);
        Task<IQueryable<PurchaseOrder>> GetPurchaseOrdersByMWOId(Guid MWOId);
        Task<IQueryable<BudgetItem>> GetBudgetItemWithMWOList(Guid MWOId);
        Task<IQueryable<BudgetItem>> GetBudgetItemWithMWOPurchaseOrderList(Guid MWOId);
    }
}
