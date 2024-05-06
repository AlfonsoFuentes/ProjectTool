using Shared.Enums.BudgetItemTypes;
using Shared.Models.MWO;
using Shared.NewModels.Brands.Reponses;

namespace Shared.Models.BudgetItems
{
    public class CreateBudgetItemRequest
    {
        public CreateBudgetItemRequest()
        {

        }

       
        public MWOResponse MWO { get; set; } = new();
        public Guid MWOId => MWO.Id;
        public string Name { get; set; } = string.Empty;
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;

        public bool Existing { get; set; }
        public bool IsRegularData => Type == BudgetItemTypeEnum.EHS
            || Type == BudgetItemTypeEnum.Structural
            || Type == BudgetItemTypeEnum.Foundations
            || Type == BudgetItemTypeEnum.Electrical
            || Type == BudgetItemTypeEnum.Piping
            || Type == BudgetItemTypeEnum.Insulations
            || Type == BudgetItemTypeEnum.Testing

            || Type == BudgetItemTypeEnum.Painting;
        public bool IsEquipmentData => Type == BudgetItemTypeEnum.Equipments
           || Type == BudgetItemTypeEnum.Instruments;
        public bool IsEngineeringData => Type == BudgetItemTypeEnum.Engineering;
        public bool IsContingencyData => Type == BudgetItemTypeEnum.Contingency;
        public bool IsTaxesData => Type == BudgetItemTypeEnum.Taxes;
        public bool IsEngContData => IsContingencyData || IsEngineeringData;
        public bool IsAlteration => Type == BudgetItemTypeEnum.Alterations;
        public double UnitaryCost { get; set; }
        public double Budget { get; set; }
        public double Quantity { get; set; } = 1;
        public NewBrandResponse? Brand { get; set; }
        public string? Model { get; set; } = string.Empty;
        public string? Reference { get; set; } = string.Empty;
        public double SumBudgetItems { get; set; }
        public double SumTaxItems { get; set; }
        public double Percentage { get; set; }
        public double SumPercentage { get; set; }

        public double TaxesNoProductive { get; set; } = 19;
        public List<BudgetItemDto> BudgetItemDtos { get; set; } = new List<BudgetItemDto>();

        public double SumBudgetTaxes => Math.Round(BudgetItemDtos.Sum(x => x.Budget), 2);
        
    }


}
