using Shared.Enums.MWOStatus;
using Shared.Enums.MWOTypes;

namespace Shared.Models.MWO
{
    public class MWOResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum MWOType { get; set; } = MWOTypeEnum.None;
        public string Type => MWOType.Name;
        public MWOStatusEnum Status { get; set; } = MWOStatusEnum.None;
        public string sStatus => Status.Name;
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
        public double Capital { get; set; }
        public double Expenses { get; set; }
        public double Appropiation => Capital + Expenses;
        public string CECName { get; set; } = string.Empty;
        public string CostCenter { get; set; } = string.Empty;


        public double AssignedCapital => ActualCapital + CommitmentCapital + PotencialCapital;
        public double ActualCapital { get; set; }
        public double PotencialCapital { get; set; }
        public double CommitmentCapital => POValueCapital - ActualCapital - PotencialCapital;
        public double PendingCapital => Capital - AssignedCapital;
        public double POValueCapital { get; set; }

        public double AssignedExpenses => ActualExpenses + CommitmentExpenses + PotencialExpenses;
        public double ActualExpenses { get; set; }
        public double PotencialExpenses { get; set; }
        public double CommitmentExpenses => POValueExpenses - ActualExpenses - PotencialExpenses;
        public double PendingExpenses => Expenses - AssignedExpenses;
        public double POValueExpenses { get; set; }
        public bool HasAlterations => Expenses > 0;
        public bool IsAssetProductive { get; set; } = true;

        public double PercentageAssetNoProductive { get; set; } = 19;
        public double PercentageEngineering { get; set; } = 6;
        public double PercentageContingency { get; set; } = 10;
        public double PercentageTaxForAlterations { get; set; } = 19;


    }
}
