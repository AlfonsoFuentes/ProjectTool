using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Account
{
    public class AplicationUser: IdentityUser
    {

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
