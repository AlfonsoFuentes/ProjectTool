using Application.Interfaces;
using Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;

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

       
    }
}
