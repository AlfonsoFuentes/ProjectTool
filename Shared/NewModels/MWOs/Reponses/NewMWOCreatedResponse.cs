﻿namespace Shared.NewModels.MWOs.Reponses
{

    public class NewMWOCreatedResponse
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
        
        public double ExpensesUSD { get; set; }
        public double CapitalUSD { get; set; }
        public double AppropiationUSD => ExpensesUSD + CapitalUSD;
        public bool HasExpenses { get; set; }

    }

}
