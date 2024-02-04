namespace Domain.Entities.Data
{
    public class BudgetItem : BaseEntity
    {
        public MWO MWO { get; set; } = null!;
        public Guid MWOId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; } = 0;
        public double UnitaryCost { get; set; }
        public double Budget { get; set; }
        public int Order { get; set; }
        public bool Existing { get; set; }
        public double Quantity { get; set; }
        public Guid? BrandId { get; set; }
        public Brand? Brand { get; set; } = null!;

        public string? Model { get; set; } = string.Empty;
        public string? Reference { get; set; } = string.Empty;
        public double Percentage { get; set; }

        public ICollection<TaxesItem> TaxesItems { get; set; } = new List<TaxesItem>();
        public ICollection<TaxesItem> Selecteds { get; set; } = new List<TaxesItem>();
    }
}
