using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Account
{
    public class AplicationUser: IdentityUser
    {
        public string InternalRole { get; set; } = string.Empty;
        //public string RefreshToken { get; set; }
        //public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
