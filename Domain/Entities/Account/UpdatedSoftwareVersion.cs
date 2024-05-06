using Domain.Entities.Data;

namespace Domain.Entities.Account
{
    public class UpdatedSoftwareVersion : BaseEntity, ITenantEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public Guid SoftwareVersionId { get; set; }
        public AplicationUser AplicationUser { get; set; } = null!;
        public string AplicationUserId { get; set; } = string.Empty;

    }
}
