using Application.Interfaces;
using Domain.Entities.Data;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Models.BudgetItemTypes;
using Shared.Models.PurchaseorderStatus;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    internal class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        public IAppDbContext Context { get; }

        public PurchaseOrderRepository(IAppDbContext context)
        {
            this.Context = context;
        }

        public async Task<MWO> GetMWOWithBudgetItemsAndPurchaseOrderById(Guid MWOId)
        {
            var mwo = await Context.MWOs
                    .Include(x => x.BudgetItems).
                      ThenInclude(x => x.PurchaseOrderItems).
                      ThenInclude(x => x.PurchaseOrder).
                      AsNoTracking().
                      AsQueryable().
                      AsSplitQuery().
                      SingleOrDefaultAsync(x => x.Id == MWOId);
            return mwo!;
        }

        public async Task<BudgetItem> GetBudgetItemWithPurchaseOrdersById(Guid BudgetItemId)
        {
            var budgetItem = await Context.BudgetItems
               .Include(x => x.PurchaseOrderItems)
               .ThenInclude(x => x.PurchaseOrder)
               .AsNoTracking()
               .AsQueryable()
               .FirstOrDefaultAsync(x => x.Id == BudgetItemId);
            return budgetItem!;
        }

        public async Task AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            await Context.PurchaseOrders.AddAsync(purchaseOrder);
        }

        public async Task AddPurchaseorderItem(PurchaseOrderItem purchaseOrderItem)
        {
            await Context.PurchaseOrderItems.AddAsync(purchaseOrderItem);
        }

        public Task<IQueryable<Supplier>> GetSuppliers()
        {
            return Task.FromResult(Context.Suppliers
                .AsNoTracking()
                .AsQueryable());
        }

        public async Task<List<TaxesItem>> GetTaxesItemsByBudgetItemId(Guid BudgetItemId)
        {

            return await Context.TaxesItems
                .Include(x => x.BudgetItem)
                .Where(x => x.SelectedId == BudgetItemId)
                .AsNoTracking()
                .AsQueryable()
                .AsSplitQuery()
                .ToListAsync();
        }



        public Task UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            Context.PurchaseOrders.Update(purchaseOrder);
            return Task.CompletedTask;
        }
        public Task<IQueryable<PurchaseOrder>> GetAllPurchaseorders()
        {
            return Task.FromResult(Context
                .PurchaseOrders
                .Include(x => x.MWO)
                .Include(x => x.PurchaseOrderItems)
                .ThenInclude(x => x.BudgetItem)
                .Include(x => x.Supplier)

                .AsNoTracking()
                .AsQueryable()
                .AsSplitQuery());
        }



        public async Task<PurchaseOrder> GetPurchaseOrderById(Guid PurchaseOrderId)
        {
            return (await Context.PurchaseOrders
                .Include(x => x.PurchaseOrderItems)

                .SingleOrDefaultAsync(x => x.Id == PurchaseOrderId))!;
        }
        public async Task<PurchaseOrder> GetPurchaseOrderClosedById(Guid PurchaseOrderId)
        {
            return (await Context.PurchaseOrders
                .Include(x => x.PurchaseOrderItems)
                

                .SingleOrDefaultAsync(x => x.Id == PurchaseOrderId))!;
        }
        public async Task<PurchaseOrderItem> GetPurchaseOrderItemById(Guid purchaseorderItemId)
        {
            return (await Context.PurchaseOrderItems.FindAsync(purchaseorderItemId))!;
        }
        public Task UpdatePurchaseOrderItem(PurchaseOrderItem purchaseOrderItem)
        {
            Context.PurchaseOrderItems.Update(purchaseOrderItem);
            return Task.CompletedTask;
        }
        public async Task<bool> ReviewIfNameExist(Guid PurchaseorderId, string name)
        {
            Expression<Func<PurchaseOrder, bool>> expresion = PurchaseorderId == Guid.Empty ?
                x => x.PurchaseorderName == name :
                x => x.Id != PurchaseorderId && x.PurchaseorderName == name;
            return await Context.PurchaseOrders.AnyAsync(expresion);
        }
        public async Task<bool> ReviewIfPurchaseRequisitionExist(Guid PurchaseorderId, string pr)
        {
            Expression<Func<PurchaseOrder, bool>> expresion = PurchaseorderId == Guid.Empty ?
                x => x.PurchaseRequisition == pr :
                x => x.Id != PurchaseorderId && x.PurchaseRequisition == pr;
            return await Context.PurchaseOrders.AnyAsync(expresion);
        }
        public async Task<bool> ReviewIfPurchaseOrderExist(Guid PurchaseorderId, string po)
        {
            if (string.IsNullOrEmpty(po)) return false;
            return await Context.PurchaseOrders.AnyAsync(x => x.Id != PurchaseorderId && x.PONumber == po);
        }

        public async Task<MWO> GetMWOById(Guid MWOId)
        {
            return (await Context.MWOs.FindAsync(MWOId))!;
        }
        public async Task<PurchaseOrder> GetPurchaseOrderWithItemsAndSupplierById(Guid PurchaseOrderId)
        {
            return (await Context.PurchaseOrders
                 .Include(x => x.MWO)
                 .Include(x => x.PurchaseOrderItems)
                 .ThenInclude(x => x.BudgetItem)
                 .Include(x => x.Supplier)

                 .AsNoTracking()
                 .AsQueryable().
                 AsSplitQuery().SingleOrDefaultAsync(x => x.Id == PurchaseOrderId))!;
        }
        public async Task<PurchaseOrder> GetPurchaseOrderToEditById(Guid PurchaseOrderId)
        {
            return (await Context.PurchaseOrders
                    .Include(x => x.PurchaseOrderItems)
                 .ThenInclude(x => x.BudgetItem)
               .SingleOrDefaultAsync(x => x.Id == PurchaseOrderId))!;
        }
        public async Task<PurchaseOrder> GetPurchaseOrderToApproveAlterationById(Guid PurchaseOrderId)
        {
            return (await Context.PurchaseOrders
                .Include(x => x.MWO)
                    .Include(x => x.PurchaseOrderItems)
                 .ThenInclude(x => x.BudgetItem)
               .SingleOrDefaultAsync(x => x.Id == PurchaseOrderId))!;
        }
        public async Task<PurchaseOrderItem> GetPurchaseOrderItemForTaxesItemById(Guid purchaseOrderId)
        {
            return (await Context.PurchaseOrderItems
               .Include(x => x.BudgetItem)
                .Where(x => x.PurchaseOrderId == purchaseOrderId
                && x.IsTaxNoProductive == true).SingleOrDefaultAsync())!;
        }
        public async Task<PurchaseOrderItem> GetPurchaseOrderItemForTaxesForAlterationById(Guid PurchaseOrderId, Guid BudgetItemId)
        {
            return (await Context.PurchaseOrderItems
               .Include(x => x.BudgetItem)
                .Where(x => x.PurchaseOrderId == PurchaseOrderId && x.BudgetItemId == BudgetItemId && x.IsAlteration == true).SingleOrDefaultAsync())!;
        }
        public async Task<BudgetItem> GetTaxBudgetItemNoProductive(Guid MWOId)
        {
            var result = await Context.BudgetItems.SingleOrDefaultAsync(x => x.MWOId == MWOId && x.IsMainItemTaxesNoProductive == true);
            return result!;
        }

    }
}
