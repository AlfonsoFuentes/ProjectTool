namespace Shared.Models.BudgetItems
{
    public class ListBudgetItemResponse
    {
        public List<BudgetItemResponse> BudgetItems { get; set; } = new();
        public string MWOName { get; set; } = string.Empty;
      
    }
    public class BudgetItemResponse
    {
        public Guid Id { get; set; }
        public string Nomenclatore { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double UnitaryCost { get; set; }
        public string Budget { get; set; } = string.Empty;
        public int Order { get; set; }
        public bool Existing { get; set; }
        public double Quantity { get; set; }
        public string? Brand { get; set; }


        public string? Model { get; set; } = string.Empty;
        public string? Reference { get; set; } = string.Empty;
        public double Percentage { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
    }
}
