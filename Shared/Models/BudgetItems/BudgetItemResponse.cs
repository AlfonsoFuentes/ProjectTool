using Shared.Models.BudgetItemTypes;

namespace Shared.Models.BudgetItems
{
    public class BudgetItemResponse
    {
        public Guid MWOId { get; set; }
        public Guid Id { get; set; }
        public string Nomenclatore { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string NomenclatoreName=>$"{Nomenclatore} - {Name}";
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;
        public bool IsNotAbleToEditDelete { get; set; }
        public bool IsMainItemTaxesNoProductive { get; set; }
        public double UnitaryCost { get; set; }
        public double Budget { get; set; }
        public double Assigned { get; set; }
        public double PotencialAssigned { get; set; }
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
