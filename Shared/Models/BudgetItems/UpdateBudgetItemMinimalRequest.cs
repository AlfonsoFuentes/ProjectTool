using Shared.Models.BudgetItemTypes;

namespace Shared.Models.BudgetItems
{
    public class UpdateBudgetItemMinimalRequest
    {
        public Guid MWOId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public double UnitaryCost { get; set; }
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;
        public string MWOName { get; set; } = string.Empty;

        public double Percentage { get; set; }
        public double Budget { get; set; }
    }
}
