using Shared.Enums.MWOStatus;
using Shared.Enums.MWOTypes;

namespace Shared.Models.MWO
{
    public class MWOCreatedResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum MWOType { get; set; } = MWOTypeEnum.None;
        public string Type => MWOType.Name;
        public MWOStatusEnum MWOStatus { get; set; } = MWOStatusEnum.None;
        public string Status => MWOStatus.Name;
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
        public double CapitalUSD { get; set; }
        public double ExpensesUSD { get; set; }
        public double AppropiationUSD => CapitalUSD + ExpensesUSD;
        public string CECName { get; set; } = string.Empty;
        public string CostCenter { get; set; } = string.Empty;
        public bool HasExpenses { get; set; }
        public bool IsAssetProductive { get; set; } = true;
        public double PercentageAssetNoProductive { get; set; } = 19;
        public double PercentageEngineering { get; set; } = 6;
        public double PercentageContingency { get; set; } = 10;
        public double PercentageTaxForAlterations { get; set; } = 19;


    }
}
