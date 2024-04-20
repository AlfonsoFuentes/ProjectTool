using Application.Interfaces;
using Domain.Entities.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    internal class MWORepository : IMWORepository
    {
        public IAppDbContext Context { get; set; }
     
        public MWORepository(IAppDbContext appDbContext)
        {
            Context = appDbContext;
           
        }

        public async Task AddMWO(MWO mWO)
        {
            await Context.MWOs.AddAsync(mWO);


        }

        public async Task<bool> ReviewIfNameExist(string name)
        {

            return await Context.MWOs.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        }
        public async Task<bool> ReviewIfNameExist(Guid Id, string name)
        {

            return await Context.MWOs.Where(x => x.Id != Id).AnyAsync(x => x.Name.ToLower() == name.ToLower());
        }
       

        public Task UpdateMWO(MWO entity)
        {
            Context.MWOs.Update(entity);
            return Task.CompletedTask;
        }
        public Task UpdateBudgetItem(BudgetItem entity)
        {
            Context.BudgetItems.Update(entity);
            return Task.CompletedTask;
        }
        public async Task<MWO> GetMWOById(Guid id)
        {
            return (await Context.MWOs.FindAsync(id))!;
        }

        public async Task<bool> ReviewIfNumberExist(string cecNumber)
        {
            return await Context.MWOs.AnyAsync(x => x.MWONumber == cecNumber);
        }
        public async Task<MWO> GetMWOWithItemsById(Guid id)
        {
            return (await Context.MWOs
                .Include(x => x.BudgetItems)
                .Include(x => x.PurchaseOrders)
                .AsNoTracking()
                .AsQueryable()
                .AsSplitQuery()
                .SingleOrDefaultAsync(x => x.Id == id))!;
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
        public Task<IQueryable<BudgetItem>> GetBudgetItemsByMWOId(Guid MWOId)
        {
            var BudgetItems = Context.BudgetItems
                .AsNoTracking().
                AsSplitQuery().
                AsQueryable()
                .Where(x => x.MWOId == MWOId);
            return Task.FromResult(BudgetItems);
        }
        public Task<IEnumerable<MWO>> GetMWOList()
        {
            Stopwatch sw = Stopwatch.StartNew();
           
            var mwos = Context.MWOs
                .Include(x => x.BudgetItems)
                .Include(x => x.PurchaseOrders)
                .ThenInclude(x => x.PurchaseOrderItems)
                .AsNoTracking()
              .AsSplitQuery()
              .AsEnumerable();
            sw.Stop();
            var elapse = sw.ElapsedMilliseconds;
            return Task.FromResult(mwos);
        }

        public async Task<BudgetItem> GetBudgetItemsSalary(Guid MWOId)
        {
            var result = await Context.BudgetItems.FirstOrDefaultAsync(x => x.MWOId == MWOId
            && x.Percentage > 0
            && x.Type == BudgetItemTypeEnum.Engineering.Id);
            return result!;
        }

        public async Task<BudgetItem> GetBudgetItemsContingency(Guid MWOId)
        {
            var result = await Context.BudgetItems.FirstOrDefaultAsync(x => x.MWOId == MWOId

            && x.Type == BudgetItemTypeEnum.Contingency.Id);
            return result!;
        }
    }
}
