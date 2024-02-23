using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Account
{
    public class AplicationUser: IdentityUser
    {
        public string InternalRole { get; set; } = string.Empty;
    }
}
