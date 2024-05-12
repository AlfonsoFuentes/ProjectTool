



namespace Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet<MWO> MWOs { get; set; }
        DbSet<Brand> Brands { get; set; }
        DbSet<Supplier> Suppliers { get; set; }
        DbSet<BudgetItem> BudgetItems { get; set; }
        DbSet<TaxesItem> TaxesItems { get; set; }
        DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        DbSet<SoftwareVersion> SoftwareVersions { get; set; }
        DbSet<DownPayment> DownPayments { get; set; }
        DbSet<SapAdjust> SapAdjusts { get; set; }
        DbSet<UpdatedSoftwareVersion> UpdatedSoftwareVersions { get; set; }
        
        DbSet<PurchaseOrderItemReceived> PurchaseOrderItemReceiveds { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAndRemoveCacheAsync(CancellationToken cancellationToken, params string[] cacheKeys);
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> addItemFactory);
    }
}
