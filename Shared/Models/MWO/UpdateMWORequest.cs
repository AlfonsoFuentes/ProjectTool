using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.MWOTypes;
using System.Reflection;

namespace Shared.Models.MWO
{
    public class UpdateMWORequest
    {
       
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum Type { get; set; } = MWOTypeEnum.Replacement;
        public bool IsAssetProductive { get; set; } = true;
      

        public double PercentageAssetNoProductive { get; set; } = 19;
        public double PercentageEngineering { get; set; } = 6;
        public double PercentageContingency { get; set; } = 10;
        public List<BudgetItemResponse> BudgetItems { get; set; } = new();
        public double PercentageTaxForAlterations { get; set; } = 19;
        public double SumBudgetForTaxesItems => GetItemsForTaxes();
        public double SumAlterations => GetSumAlterations();
        public double SumEngContingency => GetSumEngContingency();
        public double ValueForTaxes => SumBudgetForTaxesItems * PercentageAssetNoProductive / 100.0;
        public double ValueForEngineering => SumEngContingency * PercentageEngineering / (100.0 - PercentageEngineeringContingency);
        public double ValueForContingency => SumEngContingency * PercentageContingency / (100.0 - PercentageEngineeringContingency);
        public double PercentageEngineeringContingency => PercentageContingency + PercentageEngineering;
        double GetSumEngContingency()
        {
            var sumBudget = BudgetItems.
                Where(x => x.Type.Id != BudgetItemTypeEnum.Alterations.Id &&
                x.Type.Id != BudgetItemTypeEnum.Engineering.Id &&
                x.Type.Id != BudgetItemTypeEnum.Contingency.Id).Sum(x => x.Budget);

            var sumDrawings = BudgetItems.Where(x => x.Type.Id == BudgetItemTypeEnum.Engineering.Id && x.Percentage == 0).Sum(x => x.Budget);
            return sumBudget + sumDrawings;
        }
        double GetSumAlterations()
        {
            var sumBudget = BudgetItems.
                Where(x => x.Type.Id == BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget);

            return sumBudget;
        }
        double GetItemsForTaxes()
        {
            var sumBudget = BudgetItems.
                Where(x => x.Type.Id != BudgetItemTypeEnum.Alterations.Id &&
                x.Type.Id != BudgetItemTypeEnum.Taxes.Id &&
                x.Type.Id != BudgetItemTypeEnum.Engineering.Id &&
                x.Type.Id != BudgetItemTypeEnum.Contingency.Id).Sum(x => x.Budget);

            var sumDrawings = BudgetItems.Where(x => x.Type.Id == BudgetItemTypeEnum.Engineering.Id && x.Percentage == 0).Sum(x => x.Budget);

            return sumBudget + sumDrawings;
        }

        
    }
}
