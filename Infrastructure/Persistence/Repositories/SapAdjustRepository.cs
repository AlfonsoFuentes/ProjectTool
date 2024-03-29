using Application.Interfaces;
using Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.UserManagement;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    internal class SapAdjustRepository : ISapAdjustRepository
    {
        public IAppDbContext Context { get; set; }


        public SapAdjustRepository(IAppDbContext appDbContext)
        {
            Context = appDbContext;
        }

        public async Task AddSapAdAjust(SapAdjust sapAdjust)
        {
            await Context.SapAdjusts.AddAsync(sapAdjust);
        }

        public Task UpdateSapAdjust(SapAdjust sapAdjust)
        {
            Context.SapAdjusts.Update(sapAdjust);
            return Task.CompletedTask;
        }

        public async Task DeleteSapAdAjust(Guid sapAdAjustId)
        {
            var entity = await Context.SapAdjusts.FindAsync(sapAdAjustId);
            if (entity != null)
            {
                Context.SapAdjusts.Remove(entity);
            }

        }

        public async Task<MWO> GetSapAdjustsByMWOId(Guid MWOId)
        {

            var context = await Context.MWOs
                .Include(x => x.SapAdjusts)
                .AsNoTracking()
                .AsSplitQuery()
                .SingleOrDefaultAsync(x => x.Id == MWOId);



            return context!;

        }
        public async Task<MWO> GetMWOById(Guid MWOId)
        {
            return (await Context.MWOs.Include(x => x.SapAdjusts).FirstOrDefaultAsync(x => x.Id == MWOId))!;
        }

        public async Task<SapAdjust> GetSapAdAjustsById(Guid SapAsjustId)
        {
            return (await Context
                .SapAdjusts.Include(x => x.MWO)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == SapAsjustId))!;
        }
    }
}
