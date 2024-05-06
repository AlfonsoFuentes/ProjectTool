namespace Infrastructure.Configurations
{
    internal class AplicationUserConfiguration : IEntityTypeConfiguration<AplicationUser>
    {
        public void Configure(EntityTypeBuilder<AplicationUser> builder)
        {
            builder.HasKey(ci => ci.Id);
            builder.HasMany(c => c.UpdatedSoftwareVersions).WithOne(t => t.AplicationUser).HasForeignKey(e => e.AplicationUserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            
        }

    }
}
