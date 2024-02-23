using Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal class BudgetItemConfiguration : IEntityTypeConfiguration<BudgetItem>
    {
        public void Configure(EntityTypeBuilder<BudgetItem> builder)
        {
            builder.HasKey(ci => ci.Id);
            builder.HasOne(c => c.Brand).WithMany(t => t.BudgetItems).HasForeignKey(x => x.BrandId).OnDelete(DeleteBehavior.NoAction);
           
            builder.HasMany(x => x.TaxesItems).WithOne(t => t.BudgetItem).HasForeignKey(e => e.BudgetItemId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Selecteds).WithOne(t => t.Selected).HasForeignKey(x => x.SelectedId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(x => x.PurchaseOrderItems).WithOne(t => t.BudgetItem).HasForeignKey(x => x.BudgetItemId).OnDelete(DeleteBehavior.NoAction);

        }

    }
}
