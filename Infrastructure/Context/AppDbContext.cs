using Application.Interfaces;
using Domain.Entities.Account;
using Domain.Entities.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.UserManagement;
using System.Reflection;

namespace Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<AplicationUser>,IAppDbContext
    {
        private CurrentUser CurrentUser { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options, CurrentUser currentUser) : base(options)
        {
            CurrentUser = currentUser;
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IBaseEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = CurrentUser.UserId;
                        entry.Entity.CreatedByUserName = CurrentUser.UserName;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = CurrentUser.UserId;
                        //entry.Entity.CreatedBy = CurrentUser.UserId; se agrego esta linea para cambiar de usuario
                        //entry.Entity.CreatedByUserName = CurrentUser.UserName;
                        break;
                }
            }
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return 0;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.Name is "LastModifiedBy" or "CreatedBy"))
            {
                property.SetColumnType("nvarchar(128)");
            }

            base.OnModelCreating(builder);

        }
        public DbSet<MWO> MWOs { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<TaxesItem> TaxesItems { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<DownPayment> DownPayments { get; set; }
    }
}
