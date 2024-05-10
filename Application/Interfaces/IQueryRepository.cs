using Domain.Entities.Account;

namespace Application.Interfaces
{
    public interface IQueryRepository
    {
        Task<List<Brand>> GetAllBrandsAsync();
        Task<List<Supplier>> GetAllSuppliersAsync();
        Task<Brand?> GetBrandByIdAsyn(Guid id);
        Task<Supplier?> GetSupplierByIdAsync(Guid id);
        Task<BudgetItem?> GetBudgetItemToUpdateByIdAsync(Guid Id);
        Task<List<T>> GetAllAsync<T>() where T : class;
        Task<List<UpdatedSoftwareVersion>> GetVersionsByUserIdAsync(string userId);
        Task<List<MWO>> GetAllMWOsCreatedAsync();
        Task<List<MWO>> GetAllMWOsApprovedAsync();
        Task<MWO?> GetMWOByIdApprovedAsync(Guid MWOId);
        Task<MWO?> GetMWOByIdCreatedAsync(Guid id);
        Task<BudgetItem?> GetBudgetItemMWOApprovedAsync(Guid Id);
        Task<List<PurchaseOrder>> GetAllPurchaseOrderApprovedAsync();
        Task<List<PurchaseOrder>> GetAllPurchaseOrderCreatedAsync();
        Task<List<PurchaseOrder>> GetAllPurchaseOrderClosedAsync();

        Task<PurchaseOrder?> GetPurchaseOrderByIdCreatedAsync(Guid PurchaseOrderId);
        Task<PurchaseOrder?> GetPurchaseOrderByIdToReceiveAsync(Guid PurchaseOrderId);
        Task<MWO?> GetMWOByIdWithPurchaseOrderAsync(Guid MWOId);
    }
}
