namespace Infrastructure.Configurations
{
    internal class MWOConfiguration : IEntityTypeConfiguration<MWO>
    {
        public void Configure(EntityTypeBuilder<MWO> builder)
        {
            builder.HasKey(ci => ci.Id);
            builder.HasMany(c => c.BudgetItems).WithOne(t => t.MWO).HasForeignKey(e => e.MWOId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.PurchaseOrders).WithOne(t => t.MWO).HasForeignKey(e => e.MWOId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.SapAdjusts).WithOne(t => t.MWO).HasForeignKey(e => e.MWOId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }

    }
}
