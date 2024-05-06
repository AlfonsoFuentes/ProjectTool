using Shared.Models.BudgetItems;

namespace Shared.NewModels.MWOs.Reponses
{
    public class NewMWOCreatedWithItemsResponse
    {
        public Guid MWOId { get; set; }
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum Type { get; set; } = MWOTypeEnum.None;
        public FocusEnum Focus { get; set; } = FocusEnum.None;
        public bool IsAssetProductive { get; set; } = true;
        public double PercentageAssetNoProductive { get; set; } = 19;
        public double PercentageEngineering { get; set; } = 6;
        public double PercentageContingency { get; set; } = 10;
        public double PercentageTaxForAlterations { get; set; } = 19;
        public List<NewBudgetItemMWOCreatedResponse> BudgetItems { get; set; } = new();
        public List<NewBudgetItemMWOCreatedResponse> BudgetItemsExpenses => BudgetItems == null ? new() : BudgetItems.Where(x => x.IsAlteration).ToList();
        public List<NewBudgetItemMWOCreatedResponse> BudgetItemsCapital => BudgetItems == null ? new() : BudgetItems.Where(x => x.IsAlteration == false).ToList();
        public List<NewBudgetItemMWOCreatedResponse> BudgetItemsEngineringContingency => BudgetItemsCapital == null ? new() :
            BudgetItemsCapital.Where(x => (x.Type.Id == BudgetItemTypeEnum.Engineering.Id || x.Type.Id == BudgetItemTypeEnum.Contingency.Id) && x.IsNotAbleToEditDelete).ToList();

        public List<NewBudgetItemMWOCreatedResponse> BudgetItemsForTaxes => BudgetItems == null ? new() :
            BudgetItemsCapital.Where(x => !x.IsTaxesData && !x.IsEngContData).ToList();

        public List<NewBudgetItemMWOCreatedResponse> BudgetItemTaxesItems => BudgetItems == null ? new() : BudgetItems.Where(x => x.Type.Id == BudgetItemTypeEnum.Taxes.Id && x.IsMainItemTaxesNoProductive).ToList();
        public double TaxesNoProductiveUSD => BudgetItemTaxesItems == null || BudgetItemTaxesItems.Count == 0 ? 0 : BudgetItemTaxesItems.Sum(x => x.Budget);
        public double ExpensesUSD => BudgetItemsExpenses.Count == 0 ? 0 : BudgetItemsExpenses.Sum(x => x.Budget);
        public double CapitalUSD => BudgetItemsCapital.Count == 0 ? 0 : BudgetItemsCapital.Sum(x => x.Budget);
        public double EngineeringUSD => BudgetItemsEngineringContingency.Count == 0 ? 0 : BudgetItemsEngineringContingency.Sum(x => x.Budget);

        public double BudgetForCalculateNotProductiveTaxesUSD => BudgetItemsForTaxes.Sum(x => x.Budget);
        public double BudgetUSD => CapitalUSD - EngineeringUSD;
        public double AppropiationUSD => ExpensesUSD + CapitalUSD;
        public bool HasExpenses => BudgetItemsExpenses.Count > 0;
        public double EngContingencyPercentage => BudgetItemsEngineringContingency == null ||
            BudgetItemsEngineringContingency.Count == 0 ? 0 : BudgetItemsEngineringContingency.Sum(x => x.Percentage);

    }
}
