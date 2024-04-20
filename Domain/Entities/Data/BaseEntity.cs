namespace Domain.Entities.Data
{
    public class BaseEntity: IBaseEntity
    {
        public Guid Id { get; set; }
        public string CreatedByUserName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
