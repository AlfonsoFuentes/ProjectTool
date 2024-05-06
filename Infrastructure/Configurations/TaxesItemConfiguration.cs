namespace Infrastructure.Configurations
{
    internal class TaxesItemConfiguration : IEntityTypeConfiguration<TaxesItem>
    {
        public void Configure(EntityTypeBuilder<TaxesItem> builder)
        {
            builder.HasKey(ci => ci.Id);
           

        }

    }
}
