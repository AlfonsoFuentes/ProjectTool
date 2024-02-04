namespace Domain.Entities.Data
{
    public interface IBaseEntity
    {
         Guid Id { get; set; }
         string CreatedByUserName { get; set; }
         string CreatedBy { get; set; } 
         DateTime CreatedDate { get; set; }

         string? LastModifiedBy { get; set; }

         DateTime? LastModifiedOn { get; set; }
    }
    public class BaseEntity: IBaseEntity
    {
        public Guid Id { get; set; }
        public string CreatedByUserName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        public string? LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }
    }
}
