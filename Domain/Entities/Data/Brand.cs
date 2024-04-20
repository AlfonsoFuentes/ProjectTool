namespace Domain.Entities.Data
{
    public class Brand : BaseEntity, ITenantCommonEntity
    {

        public string Name { get; set; } = string.Empty;
        public static Brand Create(string name)
        {
            return new Brand() { Id = Guid.NewGuid(), Name = name };
        }
        public ICollection<BudgetItem> BudgetItems { get; set; } = new HashSet<BudgetItem>();
    }
}
