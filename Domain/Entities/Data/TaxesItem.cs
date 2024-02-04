namespace Domain.Entities.Data
{
    public class TaxesItem : BaseEntity
    {
        public Guid BudgetItemId { get; set; }
        public BudgetItem BudgetItem { get; set; } = null!;

        public Guid SelectedId { get; set; }
        public BudgetItem Selected { get; set; } = null!;

    }
}
