namespace Domain.Entities.Data
{
    public class SoftwareVersion : BaseEntity, ITenantCommonEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Version { get; set; }
        public static SoftwareVersion Create(string name, int version)
        {
            return new SoftwareVersion() { Id = Guid.NewGuid(), Name = name, Version = version };
        }
    }
}
