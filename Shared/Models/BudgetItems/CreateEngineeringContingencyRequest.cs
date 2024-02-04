using Shared.Models.BudgetItemTypes;

namespace Shared.Models.BudgetItems
{
    public class CreateEngineeringContingencyRequest
    {
        public Guid MWOId { get; set; }
        public string Name { get; set; } = string.Empty;
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;
        
        public int Order { get; set; }
        public double Percentage { get; set; }
    }
}
