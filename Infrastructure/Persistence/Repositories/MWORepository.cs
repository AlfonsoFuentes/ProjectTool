using Application.Interfaces;
using Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Models.BudgetItemTypes;

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

        public async Task<bool> ReviewNameExist(string name)
        {

            return await Context.MWOs.AnyAsync(x => x.Name == name);
        }
        public async Task<bool> ReviewNameExist(Guid Id, string name)
        {

            return await Context.MWOs.Where(x => x.Id != Id).AnyAsync(x => x.Name == name);
        }
        public Task<IQueryable<MWO>> GetMWOList()
        {
            var mwos = Context.MWOs.
                Include(x => x.BudgetItems).
                AsNoTracking().
                AsSplitQuery().
                AsQueryable();
            return Task.FromResult(mwos);
        }

        public Task UpdateMWO(MWO entity)
        {
            Context.MWOs.Update(entity);
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
                .AsNoTracking()
                .AsQueryable()
                .AsSplitQuery()
                .SingleOrDefaultAsync(x => x.Id == id))!;
        }




    }
}
