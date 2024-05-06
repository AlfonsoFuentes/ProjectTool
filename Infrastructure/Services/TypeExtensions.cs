using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public static class TypeExtensions
    {
        public static bool HideTenantValidation(this Type type)
        {
            var booleans = new List<bool>()
            {
                type.IsAssignableFrom(typeof(IdentityRole)),
                type.IsAssignableFrom(typeof(IdentityRoleClaim<string>)),
                type.IsAssignableFrom(typeof(AplicationUser)),
                type.IsAssignableFrom(typeof(IdentityUserLogin<string>)),
                type.IsAssignableFrom(typeof(IdentityUserRole<string>)),
                type.IsAssignableFrom(typeof(IdentityUserToken<string>)),
                type.IsAssignableFrom(typeof(IdentityUserClaim<string>)),
                typeof(ITenantCommonEntity).IsAssignableFrom(type)
            };

            var result = booleans.Aggregate((b1, b2) => b1 || b2);
            return result;
        }
    }
}
