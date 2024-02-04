using Application.Interfaces;
using Domain.Entities.Data;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
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

        public async Task<bool> ReviewNameExist(string name)
        {

            return await Context.BudgetItems.AnyAsync(x => x.Name == name);
        }
        public async Task<bool> ReviewNameExist(Guid Id, string name)
        {

            return await Context.BudgetItems.Where(x => x.Id != Id).AnyAsync(x => x.Name == name);
        }
        public Task<IQueryable<BudgetItem>> GetBudgetItemList(Guid MWOId)
        {
            var BudgetItems = Context.BudgetItems.
                AsNoTracking().
                AsSplitQuery().
                AsQueryable()
                .Where(x => x.MWOId == MWOId);
            return Task.FromResult(BudgetItems);
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

        public async Task<MWO> GetMWOWithItemsById(Guid MWOId)
        {
            return (await Context.MWOs
                .Include(x => x.BudgetItems)
                .AsNoTracking()
                .AsQueryable()
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == MWOId))!;
        }

        public async Task UpdateEngCostContingency(Guid Mwoid)
        {
            var allitems = await Context.BudgetItems
                 .Where(x => x.MWOId == Mwoid &&
                 (x.Type != BudgetItemTypeEnum.Engineering.Id
                 || x.Type != BudgetItemTypeEnum.Contingency.Id
                 || x.Type != BudgetItemTypeEnum.Alterations.Id))
                 .AsNoTracking()
                 .AsQueryable()
                 .AsSplitQuery()
                 .ToListAsync();

            var sumBudget = allitems.Sum(x => x.Budget);

            var engContItems = await Context.BudgetItems
                .Where(x => x.MWOId == Mwoid &&
                (x.Type == BudgetItemTypeEnum.Engineering.Id
                || x.Type == BudgetItemTypeEnum.Contingency.Id))
                .ToListAsync();

            var sumPercEngCont = engContItems.Sum(x => x.Percentage);

            foreach (var item in engContItems)
            {
                item.Budget = sumBudget * item.Percentage / (100-sumPercEngCont);
                await UpdateBudgetItem(item);
            }
          
        }
        public async Task CalculateTaxes(Guid Mwoid)
        {
            var allitems = await Context.BudgetItems.
                Include(x => x.Selecteds)
                 .Where(x => x.MWOId == Mwoid &&
                 (x.Type == BudgetItemTypeEnum.Taxes.Id))
                 .AsNoTracking()
                 .AsQueryable()
                 .AsSplitQuery()
                 .ToListAsync();

            var sum = 0;
        }

        public async Task<double> GetSumEngConPercentage(Guid MWOId)
        {
            var allitems = await Context.BudgetItems
                 .Where(x => x.MWOId == MWOId &&
                 (x.Type == BudgetItemTypeEnum.Engineering.Id
                 || x.Type != BudgetItemTypeEnum.Contingency.Id))
                 .AsNoTracking()
                 .AsQueryable()
                 .AsSplitQuery()
                 .ToListAsync();
            return allitems.Sum(x => x.Percentage);
        }

        public async Task<double> GetSumBudget(Guid MWOId)
        {
            var allitems = await Context.BudgetItems
                             .Where(x => x.MWOId == MWOId &&
                             (x.Type != BudgetItemTypeEnum.Engineering.Id
                             || x.Type != BudgetItemTypeEnum.Contingency.Id
                             || x.Type != BudgetItemTypeEnum.Alterations.Id))
                             .AsNoTracking()
                             .AsQueryable()
                             .AsSplitQuery()
                             .ToListAsync();

            return allitems.Sum(x => x.Budget);
        }

        public async Task<double> GetSumTaxes(Guid BudgetItemId)
        {
            var allitems = await Context.TaxesItems
                .Include(x => x.Selected)
                .Where(x => x.BudgetItemId == BudgetItemId)
                              .AsNoTracking()
                              .AsQueryable()
                              .AsSplitQuery()
                              .ToListAsync();

            return allitems.Sum(x => x.Selected.Budget);
        }

        public async Task<string> GetMWOName(Guid MWOId)
        {
            if(await Context.MWOs.AnyAsync(x=>x.Id==MWOId))
            {
                return (await Context.MWOs.FindAsync(MWOId))!.Name;
            }
            return string.Empty;
        }
    }
}
