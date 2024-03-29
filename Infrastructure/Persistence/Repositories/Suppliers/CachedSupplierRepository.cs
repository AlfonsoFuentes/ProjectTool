using Application.Interfaces;
using Domain.Entities.Data;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Persistence.Repositories.Suppliers
{
    public class CachedSupplierRepository : ISupplierRepository
    {
        private SupplierRepository _decorated {  get; set; }
        public IAppDbContext Context { get; set; }
        private IMemoryCache memoryCache { get; set; }
        public CachedSupplierRepository(SupplierRepository decorated, IAppDbContext context, IMemoryCache memoryCache)
        {
            _decorated = decorated;
            Context = context;
            this.memoryCache = memoryCache;
        }

        public Task UpdateSupplier(Supplier entity)=>_decorated.UpdateSupplier(entity);

        public Task AddSupplier(Supplier mWO)=>_decorated.AddSupplier(mWO);

        public Task<bool> ReviewNameExist(string name)
        {
            string Key = $"supplier-ReviewNameExist-{name}";

            return memoryCache.GetOrCreateAsync(Key,

                entry=>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    return _decorated.ReviewNameExist(name);
                }
                );
            

            
        }

        public Task<bool> ReviewVendorCodeExist(string vendorcode)
        {
            string Key = $"supplier-ReviewVendorCodeExist-{vendorcode}";

            return memoryCache.GetOrCreateAsync(Key,

                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    return _decorated.ReviewVendorCodeExist(vendorcode);
                }
                );
           
        }

        public Task<bool> ReviewEmailExist(string? email)
        {
            string Key = $"supplier-ReviewEmailExist-{email}";

            return memoryCache.GetOrCreateAsync(Key,

                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    return _decorated.ReviewEmailExist(email);
                }
                );
           
        }

        public  Task<IQueryable<Supplier>?> GetSupplierList()
        {
            string Key = $"supplier-GetSupplierList";

            var result= memoryCache.GetOrCreateAsync(Key,

                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    return _decorated.GetSupplierList();
                }
                );
            return result;


        }

        public Task<Supplier?> GetSupplierById(Guid id)
        {
            string Key = $"supplier-GetSupplierById-{id}";

            return memoryCache.GetOrCreateAsync(Key,

                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    return _decorated.GetSupplierById(id);
                }
                );
           
        }

        public Task<bool> ReviewNameExist(Guid Id, string name)
        {
            string Key = $"supplier-ReviewNameExist-{Id}-{name}";

            return memoryCache.GetOrCreateAsync(Key,

                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    return _decorated.ReviewNameExist(Id, name);
                }
                );
           
        }

        public Task<bool> ReviewVendorCodeExist(Guid Id, string vendorcode)
        {
            string Key = $"supplier-ReviewVendorCodeExist-{Id}-{vendorcode}";

            return memoryCache.GetOrCreateAsync(Key,

                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    return _decorated.ReviewVendorCodeExist(Id, vendorcode);
                }
                );
           
        }

        public Task<bool> ReviewEmailExist(Guid Id, string? email)
        {
            string Key = $"supplier-ReviewEmailExist-{Id}-{email}";

            return memoryCache.GetOrCreateAsync(Key,

                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromDays(1));
                    return _decorated.ReviewEmailExist(Id, email);
                }
                );
           
        }

       
    }
}
