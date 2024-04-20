using Domain.Entities.Data;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IPurchaseOrderRepository : IRepository
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
        Task<IQueryable<PurchaseOrder>> GetAllPurchaseorders(Expression<Func<PurchaseOrder, bool>> filteruser);
        Task<PurchaseOrderItem> GetPurchaseOrderItemById(Guid purchaseorderItemId);
        Task<PurchaseOrderItem> GetPurchaseOrderMainTaxItemById(Guid TaxBudgetItemId, Guid MWOId);
        Task UpdatePurchaseOrderItem(PurchaseOrderItem purchaseOrderItem);
        Task<bool> ReviewIfPurchaseRequisitionExist(Guid PurchaseorderId, string pr);
        Task<bool> ReviewIfPurchaseOrderExist(Guid PurchaseorderId, string po);
        Task<PurchaseOrderItem> GetPurchaseOrderItemForTaxesForAlterationById(Guid PurchaseorderId, Guid BudgetItemId);
        Task<PurchaseOrderItem> GetPurchaseOrderItemForTaxesItemById(Guid purchaseOrderId);
        Task<PurchaseOrder> GetPurchaseOrderToEditById(Guid PurchaseOrderId);
        Task<bool> ReviewIfNameExist(Guid PurchaseorderId, string name);
        Task<PurchaseOrder> GetPurchaseOrderToApproveAlterationById(Guid PurchaseOrderId);
        Task<PurchaseOrder> GetPurchaseOrderClosedById(Guid PurchaseOrderId);
        Task<BudgetItem> GetBudgetItemById(Guid BudgetItemId);
        Task<BudgetItem> GetBudgetItemWithMWOById(Guid BudgetItemId);
        Task<PurchaseOrderItem> GetPurchaseOrderItemsAlterationsById(Guid PurchaseOrderId,Guid BudgetItemId);
        Task<IQueryable<PurchaseOrder>> GetPurchaseorderByBudgetItem(Guid BudgetItemId);
        Task<IQueryable<PurchaseOrder>> GetAllPurchaseordersClosed();
        Task<IQueryable<PurchaseOrder>> GetAllPurchaseordersToReceive();
        Task<IQueryable<PurchaseOrder>> GetAllPurchaseordersCreated();
        Task<PurchaseOrder> GetPurchaseOrderToDeleteById(Guid PurchaseOrderId);
        Task RemovePurchaseOrder(PurchaseOrder item);
    }
}
