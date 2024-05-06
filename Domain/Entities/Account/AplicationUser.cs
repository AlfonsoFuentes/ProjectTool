using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace Domain.Entities.Account
{
    public class AplicationUser : IdentityUser
    {

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
        public List<UpdatedSoftwareVersion> UpdatedSoftwareVersions { get; set; } = new();
        public UpdatedSoftwareVersion AddUpdatedSoftwarerVersion(Guid SoftwareVersionId)
        {
            return new UpdatedSoftwareVersion()
            {
                SoftwareVersionId = SoftwareVersionId,
                Id = Guid.NewGuid(),
                AplicationUserId =Id ,
            };

        }
    }
}
