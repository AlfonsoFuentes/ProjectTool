using Application.Interfaces;
using Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        public IAppDbContext Context { get; set; }

        public SupplierRepository(IAppDbContext appDbContext)
        {
            Context = appDbContext;
        }

        public Task UpdateSupplier(Supplier entity)
        {
            Context.Suppliers.Update(entity);
            return Task.CompletedTask;
        }

        public async Task AddSupplier(Supplier entity)
        {
            await Context.Suppliers.AddAsync(entity);
        }

        public async Task<bool> ReviewNameExist(string name)
        {
            var retorno = await Context.Suppliers.AnyAsync(x => x.Name == name);
            return retorno;
        }
        public async Task<bool> ReviewNameExist(Guid Id, string name)
        {
            var retorno = await Context.Suppliers.Where(x => x.Id != Id).AnyAsync(x => x.Name == name);
            return retorno;
        }

        public async Task<bool> ReviewVendorCodeExist(string vendorcode)
        {

            var retorno = await Context.Suppliers.AnyAsync(x => x.VendorCode == vendorcode);
            return retorno;
        }
        public async Task<bool> ReviewVendorCodeExist(Guid Id, string vendorcode)
        {
            if (string.IsNullOrEmpty(vendorcode)) return false;
            var retorno = await Context.Suppliers.Where(x => x.Id != Id).AnyAsync(x => x.VendorCode == vendorcode);
            return retorno;
        }

        public async Task<bool> ReviewEmailExist(string? email)
        {
            return await Context.Suppliers.AnyAsync(x => x.ContactEmail == email);
        }
        public async Task<bool> ReviewEmailExist(Guid Id, string? email)
        {
            return await Context.Suppliers.Where(x => x.Id != Id).AnyAsync(x => x.ContactEmail == email);
        }

        public Task<IQueryable<Supplier>?> GetSupplierList()
        {
            var result = Context.Suppliers.
               AsNoTracking().
               AsSplitQuery().
               AsQueryable();
            return Task.FromResult(result)!;
        }

        public async Task<Supplier?> GetSupplierById(Guid id)
        {
            return await Context.Suppliers.FindAsync(id);
        }
    }
}
