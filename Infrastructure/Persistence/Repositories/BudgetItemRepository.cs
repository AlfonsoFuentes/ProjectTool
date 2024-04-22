using Application.Interfaces;
using Domain.Entities.Data;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
using System.Runtime.InteropServices;
using System.Threading;

namespace Infrastructure.Persistence.Repositories
{
    internal class BudgetItemRepository : IBudgetItemRepository
    {
        public IAppDbContext Context { get; set; }

        public BudgetItemRepository(IAppDbContext appDbContext)
        {
            Context = appDbContext;
        }

        public async Task AddBudgetItem(BudgetItem BudgetItem)
        {
            await Context.BudgetItems.AddAsync(BudgetItem);


        }

        public async Task<bool> ReviewIfNameExist(Guid MWOId, string name)
        {

            return await Context.BudgetItems.AnyAsync(x => x.MWOId == MWOId && x.Name == name);
        }
        public async Task<bool> ReviewIfNameExist(Guid Id, Guid MWOId, string name)
        {

            return await Context.BudgetItems.Where(x => x.Id != Id && x.MWOId == MWOId).AnyAsync(x => x.Name == name);
        }
        public Task<IQueryable<BudgetItem>> GetBudgetItemList(Guid MWOId)
        {
            var BudgetItems = Context.BudgetItems
                .Include(x => x.Brand)
                .AsNoTracking().
                AsSplitQuery().
                AsQueryable()
                .Where(x => x.MWOId == MWOId);
            return Task.FromResult(BudgetItems);
        }
        public Task<IQueryable<BudgetItem>> GetBudgetItemWithMWOList(Guid MWOId)
        {
            var BudgetItems = Context.BudgetItems
                .Include(x => x.Brand)
                .Include(x => x.MWO)
                .AsNoTracking().
                AsSplitQuery().
                AsQueryable()
                .OrderBy(x => x.Type).ThenBy(x => x.Order)
                .Where(x => x.MWOId == MWOId);
            return Task.FromResult(BudgetItems);
        }
        public Task<IQueryable<BudgetItem>> GetBudgetItemWithMWOPurchaseOrderList(Guid MWOId)
        {
            var BudgetItems = Context.BudgetItems
                .Include(x => x.Brand)
                .Include(x => x.MWO)
                .Include(x=>x.PurchaseOrderItems).ThenInclude(x=>x.PurchaseOrder)
                .AsNoTracking().
                AsSplitQuery().
                AsQueryable()
                .OrderBy(x => x.Type).ThenBy(x => x.Order)
                .Where(x => x.MWOId == MWOId);
            return Task.FromResult(BudgetItems);
        }
        public async Task<List<BudgetItem>> GetBudgetItemForTaxesList(Guid MWOId)
        {
            var allitems = await Context.BudgetItems
                .Where(x => x.MWOId == MWOId &&
                !(x.Type == BudgetItemTypeEnum.Engineering.Id
                || x.Type == BudgetItemTypeEnum.Contingency.Id
                || x.Type == BudgetItemTypeEnum.Taxes.Id
                || x.Type == BudgetItemTypeEnum.Alterations.Id))
                .AsNoTracking()
                .AsQueryable()
                .AsSplitQuery().ToListAsync();
            return allitems;
        }
        public async Task<List<BudgetItem>> GetEngContingencyItemsList(Guid MWOId)
        {
            var allitems = await
                Context.BudgetItems
                .Where(x => x.MWOId == MWOId && x.Percentage > 0 &&
                (x.Type == BudgetItemTypeEnum.Engineering.Id
                || x.Type == BudgetItemTypeEnum.Contingency.Id))
                .AsQueryable().ToListAsync();
            return allitems;
        }
        public async Task<List<BudgetItem>> GetBudgetItemForApplyEngContList(Guid MWOId)
        {
            var allitems = await Context.BudgetItems
                .Where(x => x.MWOId == MWOId &&
                !(x.Type == BudgetItemTypeEnum.Alterations.Id
                || x.Type == BudgetItemTypeEnum.Engineering.Id
                || x.Type == BudgetItemTypeEnum.Contingency.Id))
                .AsNoTracking()
                .AsQueryable()
                .AsSplitQuery().ToListAsync();
            return allitems;
        }
        public async Task<List<BudgetItem>> GetBudgetItemEngWithoutPercentageContList(Guid MWOId)
        {
            var allitems = await Context.BudgetItems
                .Where(x => x.MWOId == MWOId &&
                (x.Type == BudgetItemTypeEnum.Engineering.Id && x.Percentage == 0))
                .AsNoTracking()
                .AsQueryable()
                .AsSplitQuery()
                .ToListAsync();
            return allitems;
        }
        public async Task<MWO> GetMWOWithItemsById(Guid MWOId)
        {
            return (await Context.MWOs
                .Include(x => x.BudgetItems)
                .AsNoTracking()
                .AsQueryable()
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == MWOId))!;
        }
        public async Task<MWO> GetMWOById(Guid MWOId)
        {
            return (await Context.MWOs.FindAsync(MWOId))!;
        }
        public async Task<List<PurchaseOrderItem>> GetPurchaseOrderItemsByMWOId(Guid MWOId)
        {
            var result = await Context.PurchaseOrderItems
                .Include(x => x.PurchaseOrder).ThenInclude(x => x.Supplier)

                .AsNoTracking()
                .AsQueryable().AsSplitQuery()
                .Where(x => x.PurchaseOrder.MWOId == MWOId)
                .ToListAsync();
            return result!;
        }

        public Task UpdateBudgetItem(BudgetItem entity)
        {
            Context.BudgetItems.Update(entity);
            return Task.CompletedTask;
        }

        public async Task<BudgetItem> GetBudgetItemById(Guid id)
        {
            return (await Context.BudgetItems.FindAsync(id))!;
        }
        public async Task<BudgetItem> GetBudgetItemWithBrandById(Guid id)
        {
            return (await Context.BudgetItems
                .Include(x => x.MWO)
                .Include(x => x.Brand)
                .AsSplitQuery()
                .SingleOrDefaultAsync(x => x.Id == id))!;
        }
        public async Task<BudgetItem> GetBudgetItemWithTaxesById(Guid id)
        {
            return (await Context.BudgetItems
                .Include(x => x.MWO)
                .Include(x => x.TaxesItems).ThenInclude(x => x.Selected)
                .AsSplitQuery()
                .SingleOrDefaultAsync(x => x.Id == id))!;
        }





        public async Task<double> GetSumEngConPercentage(Guid MWOId)
        {
            var allitems = await GetEngContingencyItemsList(MWOId);
            return allitems.Sum(x => x.Percentage);
        }

        public async Task<double> GetSumBudget(Guid MWOId)
        {
            var allitems = await GetBudgetItemForApplyEngContList(MWOId);
            var itemsEngWithoutPercentage = await GetBudgetItemEngWithoutPercentageContList(MWOId);
            var budget = allitems.Sum(x => x.Budget);
            var EngCost = itemsEngWithoutPercentage.Sum(x => x.Budget);
            var retorno = budget + EngCost;
            return retorno;
        }

        public Task<double> GetSumTaxes(Guid BudgetItemId)
        {
            var allitems = Context.TaxesItems
                .Include(x => x.Selected)
                .Where(x => x.BudgetItemId == BudgetItemId)
                              .AsNoTracking()
                              .AsQueryable()
                              .AsSplitQuery();


            return Task.FromResult(allitems.Sum(x => x.Selected.Budget));
        }

        public async Task<string> GetMWOName(Guid MWOId)
        {
            if (await Context.MWOs.AnyAsync(x => x.Id == MWOId))
            {
                return (await Context.MWOs.FindAsync(MWOId))!.Name;
            }
            return string.Empty;
        }


        public async Task AddTaxSelectedItem(TaxesItem BudgetItem)
        {
            await Context.TaxesItems.AddAsync(BudgetItem);
        }
        public Task UpdateTaxSelectedItem(TaxesItem BudgetItem)
        {
            Context.TaxesItems.Update(BudgetItem);
            return Task.CompletedTask;
        }
        public async Task<TaxesItem> GetTaxesItemById(Guid Id)
        {
            return (await Context.TaxesItems
            .SingleOrDefaultAsync(x => x.SelectedId == Id))!;
        }
        public Task<List<TaxesItem>> GetBudgetItemSelectedTaxesList(Guid Id)
        {
            var result = Context.TaxesItems
                .Include(x => x.Selected)
                .Where(x => x.BudgetItemId == Id)

                .AsNoTracking().
                AsQueryable().
                AsSplitQuery()
                .ToListAsync();
            return result;
        }
        public async Task<List<BudgetItem>> GetItemToApplyTaxes(Guid MWOId)
        {
            var ExtractAlterationAndTaxes = await Context.BudgetItems
                .Where(x => x.MWOId == MWOId &&
                (x.Type != BudgetItemTypeEnum.Alterations.Id && x.Type != BudgetItemTypeEnum.Taxes.Id)).
                AsNoTracking().
                AsQueryable()
                .ToListAsync();

            var ExtractEngineering = ExtractAlterationAndTaxes.
                Where(x => x.Percentage == 0).ToList();

            return ExtractEngineering;

        }
        public async Task<BudgetItem> GetMainBudgetTaxItemByMWO(Guid MWOId)
        {
            return (await Context.BudgetItems.
                Include(x => x.TaxesItems).
                SingleOrDefaultAsync(x => x.IsMainItemTaxesNoProductive == true && x.MWOId == MWOId))!;

        }

        public async Task UpdateEngCostContingency(Guid Mwoid)
        {


            var sumBudget = await GetSumBudget(Mwoid);

            var engContItems = await GetEngContingencyItemsList(Mwoid);

            var sumPercEngCont = engContItems.Sum(x => x.Percentage);

            foreach (var item in engContItems)
            {

                item.Budget = sumBudget * item.Percentage / (100 - sumPercEngCont);
                await UpdateBudgetItem(item);
            }

        }
        public async Task UpdateTaxes(Guid MWOId)
        {
            var allItemsTaxes = await Context.BudgetItems
                .Include(x => x.TaxesItems)
                .ThenInclude(x => x.Selected)
                .Where(x => x.MWOId == MWOId && x.Type == BudgetItemTypeEnum.Taxes.Id).
                ToListAsync();


            foreach (var item in allItemsTaxes)
            {
                var sumbudgetTaxesSelectedItems = item.TaxesItems.Sum(x => x.Selected.Budget);
                item.Budget = item.Percentage / 100 * sumbudgetTaxesSelectedItems;
                await UpdateBudgetItem(item);
            }

        }
        public async Task UpdateTaxesAndEngineeringContingencyItems(Guid MWOId, CancellationToken cancellationToken)
        {
            await UpdateTaxes(MWOId);
            await Context.SaveChangesAsync(cancellationToken);

            await UpdateEngCostContingency(MWOId);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateMWO(MWO entity)
        {
            Context.MWOs.Update(entity);
            return Task.CompletedTask;
        }

        public Task<IQueryable<BudgetItem>> GetBudgetItemsWithPurchaseordersList(Guid MWOId)
        {
            var BudgetItems = Context.BudgetItems

                .Include(x => x.PurchaseOrderItems)
                .ThenInclude(x => x.PurchaseOrder)
                .ThenInclude(x => x.Supplier)

                .AsNoTracking().
                AsSplitQuery().
                AsQueryable()
                .Where(x => x.MWOId == MWOId);
            return Task.FromResult(BudgetItems);
        }
        public Task<IQueryable<PurchaseOrder>> GetPurchaseOrdersByMWOId(Guid MWOId)
        {
            var BudgetItems = Context.PurchaseOrders
                .Include(x => x.PurchaseOrderItems)
                .Include(x => x.Supplier)

                .AsNoTracking().
                AsSplitQuery().
                AsQueryable()
                .Where(x => x.MWOId == MWOId);
            return Task.FromResult(BudgetItems);
        }
    }
}
