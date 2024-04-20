using Application.Interfaces;
using Domain.Entities.Account;
using Domain.Entities.Data;
using Domain.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<AplicationUser>,IAppDbContext
    {
        private string CurrentTenant { get; set; }
      
        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantService TenantService) : base(options)
        {
            CurrentTenant=TenantService.GetTenantId();  

           
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
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var type = entity.ClrType;
                if (typeof(ITenantEntity).IsAssignableFrom(type))
                {
                    //Build filter
                    var method = typeof(AppDbContext)
                        .GetMethod(nameof(BuildTenantGlobalFilter),
                    BindingFlags.NonPublic | BindingFlags.Static)?
                    .MakeGenericMethod(type);
                    var filter = method?.Invoke(null, new object[] { this })!;
                    entity.SetQueryFilter((LambdaExpression)filter);
                    entity.AddIndex(entity.FindProperty(nameof(ITenantEntity.TenantId))!);
                }
                else if (type.HideTenantValidation())
                {
                    continue;
                }
                else
                {
                    throw new Exception($"Entity {entity} is not marked as Tenant");
                }

            }
            base.OnModelCreating(builder);

        }
        private static LambdaExpression BuildTenantGlobalFilter<TEntity>(
           AppDbContext context) where TEntity : class, ITenantEntity

        {
            Expression<Func<TEntity, bool>> filter = x => x.TenantId == context.CurrentTenant;
            return filter;
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var AddedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added && e.Entity is ITenantEntity);
            foreach (var item in AddedEntities)
            {
                if (string.IsNullOrEmpty(CurrentTenant))
                {
                    throw new Exception("TenantId not found");
                }

                var entity = item.Entity as ITenantEntity;
                entity!.TenantId = CurrentTenant;

            }
            var ModifyEntities = ChangeTracker.Entries<IBaseEntity>().Where(e => e.State == EntityState.Modified && e.Entity is ITenantEntity);
            foreach (var entry in ModifyEntities)
            {
                entry.Entity.LastModifiedOn = DateTime.UtcNow;
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
        public DbSet<MWO> MWOs { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<TaxesItem> TaxesItems { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<DownPayment> DownPayments { get; set; }
        public DbSet<SapAdjust> SapAdjusts { get; set; }
       
    }
}
