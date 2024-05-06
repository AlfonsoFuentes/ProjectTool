using Shared.Models.BudgetItems;

namespace Shared.NewModels.MWOs.Request
{
    public  class NewMWOCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum Type { get; set; } = MWOTypeEnum.None;
        public FocusEnum Focus { get; set; } = FocusEnum.None;
        public bool IsAssetProductive { get; set; } = true;
        public double PercentageAssetNoProductive { get; set; } = 19;
        public double PercentageEngineering { get; set; } = 6;
        public double PercentageContingency { get; set; } = 10;
        public double PercentageTaxForAlterations { get; set; } = 19;
    }
}
