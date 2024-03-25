namespace Shared.Models.BudgetItems
{
    public class BudgetItemDto
    {
        public Guid SelectedItemId { get; set; }
        public Guid BudgetItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Nomenclatore { get; set; } = string.Empty;
        public double Budget { get; set; }
    }
}
