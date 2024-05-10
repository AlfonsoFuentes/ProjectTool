


namespace Application.Interfaces
{
    public interface IRepository
    {
       
        Task UpdateAsync<T>(T entity) where T : class;
        Task AddAsync<T>(T entity) where T : class;
        Task RemoveAsync<T>(T entity) where T : class;
        Task<T?> GetByIdAsync<T>(Guid Id) where T : class;
        Task<List<BudgetItem>> GetItemsToUpdateVersion1();
        Task<BudgetItem?> GetBudgetItemToUpdate(Guid MWOId);
        Task<MWO?> GetMWOById(Guid MWOId);
        Task UpdateTaxesAndEgineeringItems(MWO mwo,bool updateTaxes,bool updateEgineeringItems,CancellationToken toke);
        Task<List<TaxesItem>> GetTaxesItemsToDeleteBudgetItem(Guid BudgetItemId);
        Task RemoveRangeAsync<T>(List<T> entities) where T : class;
        Task<BudgetItem?> GetBudgetItemToUpdateTaxes(Guid BudgetItemId);
        Task<List<PurchaseOrder>> GetPurchaseOrderToUpdateVersion1();
        Task<List<PurchaseOrderItem>> GetPurchaseOrderItemsToUpdateVersion2();
        Task<PurchaseOrder?> GetPurchaseOrderByIdCreatedAsync(Guid PurchaseOrderId);
        Task<BudgetItem> GetTaxBudgetItemNoProductive(Guid MWOId);
    }
}
