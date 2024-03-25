using Application.Interfaces;
using Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    internal class BrandRepository : IBrandRepository
    {
        public IAppDbContext Context { get; set; }

        public BrandRepository(IAppDbContext context)
        {
            Context = context;
        }

        public Task UpdateBrand(Brand entity)
        {
            Context.Brands.Update(entity);
            return Task.CompletedTask;
        }

        public async Task AddBrand(Brand mWO)
        {
            await Context.Brands.AddAsync(mWO);

        }

        public async Task<bool> ReviewIfNameExist(string name)
        {
            return await Context.Brands.AnyAsync(x => x.Name.ToLower() == name);
        }

        public async Task<bool> ReviewIfNameExist(Guid Id, string name)
        {
            return await Context.Brands.Where(x => x.Id != Id).AnyAsync(x => x.Name.ToLower() == name);
        }

        public Task<IQueryable<Brand>> GetBrandList()
        {
            var mwos = Context.Brands.
               AsNoTracking().
               AsSplitQuery().
               AsQueryable();
            return Task.FromResult(mwos);
        }

        public async Task<Brand> GetBrandById(Guid id)
        {
            return (await Context.Brands.FindAsync(id))!;
        }

        
    }
}
