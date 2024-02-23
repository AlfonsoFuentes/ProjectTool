namespace Shared.Models.BudgetItems
{
    public class BudgetItemDto
    {
        public Guid BudgetItemId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Nomenclatore { get; set; } = string.Empty;
        public double Budget { get; set; }
    }
}
