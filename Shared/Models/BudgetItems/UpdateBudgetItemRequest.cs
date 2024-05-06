using Shared.Enums.BudgetItemTypes;
using Shared.NewModels.Brands.Reponses;

namespace Shared.Models.BudgetItems
{
    public class UpdateBudgetItemRequest
    {
       
       
        public Guid MWOId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MWOName { get; set; } = string.Empty;
        public string Nomenclatore { get; set; } = string.Empty;
        public BudgetItemTypeEnum Type { get; set; } = BudgetItemTypeEnum.None;
        public double UnitaryCost { get; set; }
        public double Budget { get; set; }

        public bool Existing { get; set; }
        public double Quantity { get; set; }

        public string? Model { get; set; } = string.Empty;
        public string? Reference { get; set; } = string.Empty;
        public double Percentage { get; set; }
        public List<BudgetItemDto> SelectedBudgetItemDtos { get; set; } = new List<BudgetItemDto>();

        public List<Guid> SelectedIdBudgetItemDtos { get; set; } = new List<Guid>();
        public List<BudgetItemDto> BudgetItemDtos { get; set; } = new List<BudgetItemDto>();
        public double SumBudgetTaxes => Math.Round(SelectedBudgetItemDtos.Sum(x => x.Budget), 2);


        public bool IsRegularData => Type.Id == BudgetItemTypeEnum.EHS.Id
            || Type.Id == BudgetItemTypeEnum.Structural.Id
            || Type.Id == BudgetItemTypeEnum.Foundations.Id
            || Type.Id == BudgetItemTypeEnum.Electrical.Id
            || Type.Id == BudgetItemTypeEnum.Piping.Id
            || Type.Id == BudgetItemTypeEnum.Insulations.Id
            || Type.Id == BudgetItemTypeEnum.Testing.Id

            || Type.Id == BudgetItemTypeEnum.Painting.Id;
        public bool IsEquipmentData => Type.Id == BudgetItemTypeEnum.Equipments.Id
           || Type.Id == BudgetItemTypeEnum.Instruments.Id;
        public bool IsEngineeringData => Type.Id == BudgetItemTypeEnum.Engineering.Id;
        public bool IsContingencyData => Type.Id == BudgetItemTypeEnum.Contingency.Id;
        public bool IsTaxesData => Type.Id == BudgetItemTypeEnum.Taxes.Id;
        public bool IsEngContData => IsContingencyData || IsEngineeringData;
        public bool IsAlteration => Type.Id == BudgetItemTypeEnum.Alterations.Id;
        public bool IsNotAbleToEditDelete { get; set; }
        public double SumPercentage { get; set; }
        public double SumBudgetItems { get; set; }
        public NewBrandResponse? Brand { get; set; }
       
    }
    
}
