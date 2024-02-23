using Domain.Entities.Data;

namespace Application.Interfaces
{
    public interface IPurchaseOrderRepository:IRepository
    {
        Task<MWO> GetMWOById(Guid MWOId);
        Task<MWO> GetMWOWithBudgetItemsAndPurchaseOrderById(Guid MWOId);
        Task<BudgetItem> GetBudgetItemWithPurchaseOrdersById(Guid BudgetItemId);
        Task AddPurchaseOrder(PurchaseOrder purchaseOrder);
        Task AddPurchaseorderItem(PurchaseOrderItem purchaseOrderItem);
        Task<IQueryable<Supplier>> GetSuppliers();
        Task<List<TaxesItem>> GetTaxesItemsByBudgetItemId(Guid BudgetItemId);
        Task<PurchaseOrder> GetPurchaseOrderWithItemsAndSupplierById(Guid PurchaseOrderId);

        Task<BudgetItem> GetTaxBudgetItemNoProductive(Guid MWOId);
        Task<PurchaseOrder> GetPurchaseOrderById(Guid PurchaseOrderId);
        Task UpdatePurchaseOrder(PurchaseOrder purchaseOrder);
        Task<IQueryable<PurchaseOrder>> GetAllPurchaseorders();
        Task<PurchaseOrderItem> GetPurchaseOrderItemById(Guid purchaseorderItemId);
        Task UpdatePurchaseOrderItem(PurchaseOrderItem purchaseOrderItem);
        Task<bool> ReviewIfPurchaseRequisitionExist(Guid PurchaseorderId,string pr); 
        Task<bool> ReviewIfPurchaseOrderExist(Guid PurchaseorderId, string po);

        Task<PurchaseOrderItem> GetPurchaseOrderItemForTaxesItemById(Guid purchaseOrderId);
        Task<PurchaseOrder> GetPurchaseOrderToEditById(Guid PurchaseOrderId);
    }
}
