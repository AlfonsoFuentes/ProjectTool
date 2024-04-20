namespace Domain.Entities.Data
{
    public interface IBaseEntity
    {
         Guid Id { get; set; }
         string CreatedByUserName { get; set; }
      
         DateTime CreatedDate { get; set; }

         DateTime? LastModifiedOn { get; set; }
    }
}
