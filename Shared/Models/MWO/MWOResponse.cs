using Shared.Models.BudgetItems;
using Shared.Models.MWOStatus;
using Shared.Models.MWOTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.MWO
{
    public class MWOResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum MWOType { get; set; } = MWOTypeEnum.None;
        public string Type => MWOType.Name;
        public MWOStatusEnum Status { get; set; } = MWOStatusEnum.None;
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
        public double Capital { get; set; } = 0;
        public double Expenses { get; set; } = 0;
        public double Appropiation => Capital + Expenses;
        public string CECName { get; set; } = string.Empty;
        public string CostCenter { get; set; } = string.Empty;
        public bool IsRealProductive { get; set; } = false;
        public List<BudgetItemResponse> BudgetItems { get; set; } = new();

        public double Budget => Appropiation;
        public double Assigned { get; set; } = 0;//Purchase orders approved
        public double Actual { get; set; } = 0;//Purchase orders money received
        public double Potencial { get; set; } = 0;//Purchase orders not approved
        public double Commitment => Assigned - Actual;


       
        public double Pending => Budget - Assigned - Potencial;
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
