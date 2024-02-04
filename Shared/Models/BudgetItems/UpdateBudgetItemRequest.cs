using Shared.Models.BudgetItemTypes;

namespace Shared.Models.BudgetItems
{
    public class UpdateBudgetItemRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;
        public double UnitaryCost { get; set; }
        public double Budget { get; set; }
        public int Order { get; set; }
        public bool Existing { get; set; }
        public double Quantity { get; set; }
        public Guid? BrandId { get; set; }


        public string? Model { get; set; } = string.Empty;
        public string? Reference { get; set; } = string.Empty;
        public double Percentage { get; set; }
    }
}
