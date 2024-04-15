using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.CostCenter;
using Shared.Models.MWOTypes;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.MWO
{
    public class ApproveMWORequest
    {
        
      
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MWONumber { get; set; } = string.Empty;
        public CostCenterEnum CostCenter { get; set; } = CostCenterEnum.None;
        public double PercentageTaxForAlterations { get; set; }
        public bool IsAssetProductive { get; set; } = true;
        public double PercentageAssetNoProductive { get; set; } = 19;
        public double PercentageEngineering { get; set; } = 6;
        public double PercentageContingency { get; set; } = 10;

        public MWOTypeEnum Type { get; set; } = MWOTypeEnum.None;
        List<BudgetItemResponse> BudgetItemsDiferentsMandatory => BudgetItems.Count == 0 ? new List<BudgetItemResponse>() :
            BudgetItems.Where(x =>
            !(x.Type.Id == BudgetItemTypeEnum.Alterations.Id ||
            x.Type.Id == BudgetItemTypeEnum.Taxes.Id ||
            x.Type.Id == BudgetItemTypeEnum.Contingency.Id ||
            x.Type.Id == BudgetItemTypeEnum.Engineering.Id)).ToList();
        public bool IsAbleToApproved => BudgetItems.Count == 0 ? false :
            BudgetItemsDiferentsMandatory.Count > 0;
        public List<BudgetItemResponse> BudgetItems { get; set; } = new();
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
