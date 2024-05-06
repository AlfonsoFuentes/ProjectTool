using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
           
        }
    }
}
