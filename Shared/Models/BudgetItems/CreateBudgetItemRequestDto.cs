namespace Shared.Models.BudgetItems
{
    public class CreateBudgetItemRequestDto
    {
        public CreateBudgetItemRequestDto()
        {

        }

        public Guid MWOId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }

        public bool Existing { get; set; }
        public bool IsRegularData { get; set; }
        public bool IsEquipmentData { get; set; }
        public bool IsEngineeringData { get; set; }
        public bool IsContingencyData { get; set; }
        public bool IsTaxesData { get; set; }
        public bool IsEngContData { get; set; }
        public bool IsAlteration { get; set; }
        public double UnitaryCost { get; set; }
        public double Budget { get; set; }
        public double Quantity { get; set; } = 1;
        public Guid? BrandId { get; set; }
        public string? Model { get; set; } = string.Empty;
        public string? Reference { get; set; } = string.Empty;
        public double SumBudgetItems { get; set; }
        public double SumTaxItems { get; set; }
        public double Percentage { get; set; }
        public double SumPercentage { get; set; }

        public double TaxesNoProductive { get; set; } = 19;
        public List<BudgetItemDto> BudgetItemDtos { get; set; } = new List<BudgetItemDto>();

    }


}
