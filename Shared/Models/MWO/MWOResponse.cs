using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.MWOStatus;
using Shared.Models.MWOTypes;

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
        public double Capital { get; set; } = 0;
        public double Expenses { get; set; } = 0;
        public double Appropiation => Capital + Expenses;
        public string CECName { get; set; } = string.Empty;
        public string CostCenter { get; set; } = string.Empty;
       
        public List<BudgetItemResponse> BudgetItems { get; set; } = new();

        public double Budget => Appropiation;
        public double AssignedCapital { get; set; }
        public double ActualCapital { get; set; } = 0;//Purchase orders money received
        public double PotencialCapital { get; set; } = 0;//Purchase orders not approved
        public double CommitmentCapital => AssignedCapital - ActualCapital - PotencialCapital;
        public double PendingCapital => Capital - AssignedCapital;


        public double AssignedExpenses { get; set; }
        public double ActualExpenses { get; set; } = 0;//Purchase orders money receivedActualExpenses
        public double PotencialExpenses { get; set; } = 0;//Purchase orders not approved
        public double CommitmentExpenses => AssignedExpenses - ActualExpenses - PotencialExpenses;
        public double PendingExpenses => Expenses - AssignedExpenses;

        public bool HasAlterations => BudgetItems.Any(x => x.Type.Id == BudgetItemTypeEnum.Alterations.Id);
        public bool IsAssetProductive { get; set; } = true;
        public List<string> ValidationErrors { get; set; } = new();
        public double PercentageAssetNoProductive { get; set; } = 19;
        public double PercentageEngineering { get; set; } = 6;
        public double PercentageContingency { get; set; } = 10;
        public double PercentageTaxForAlterations { get; set; } = 19;
        public void ChangePercentageTaxes(string stringpercentage)
        {
            ValidationErrors.Clear();
            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
                return;

            PercentageAssetNoProductive = percentage;
        }
        public void ChangePercentageEngineering(string stringpercentage)
        {
            ValidationErrors.Clear();
            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
                return;

            PercentageEngineering = percentage;

        }
        public void ChangePercentageContingency(string stringpercentage)
        {
            ValidationErrors.Clear();
            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
                return;


            PercentageContingency = percentage;

        }
        public void ChangeTaxForAlterations(string stringpercentage)
        {
            ValidationErrors.Clear();
            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
                return;

            PercentageTaxForAlterations = percentage;

        }
        public void ChangeType()
        {
            ValidationErrors.Clear();

        }

    }
}
