using Shared.Models.MWOTypes;

namespace Shared.Models.MWO
{
    public class CreateMWORequest
    {
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum Type { get; set; } = MWOTypeEnum.None;
        public List<string> ValidationErrors { get; set; } = new();
        public bool IsAssetProductive { get; set; } = true;
        public double PercentageAssetNoProductive { get; set; } = 19;
        public double PercentageEngineering { get; set; } = 6;
        public double PercentageContingency { get; set; } = 10;
        public double PercentageTaxForAlterations {  get; set; } = 19;
        public void ChangeName(string name)
        {
            ValidationErrors.Clear();
            Name = name;


        }

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
