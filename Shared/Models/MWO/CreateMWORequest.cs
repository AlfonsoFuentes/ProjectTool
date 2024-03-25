using Shared.Models.MWOTypes;

namespace Shared.Models.MWO
{
    public class CreateMWORequest
    {
        public Func<Task<bool>> Validator { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum Type { get; set; } = MWOTypeEnum.None;
        public CreateMWORequestDto ConvertToDto()
        {
            return new()
            {
                IsAssetProductive = IsAssetProductive,
                Name = Name,
                PercentageAssetNoProductive = PercentageAssetNoProductive,
                PercentageContingency = PercentageContingency,
                PercentageEngineering = PercentageEngineering,
                PercentageTaxForAlterations = PercentageTaxForAlterations,
                Type = Type.Id,
            };
        }
        public bool IsAssetProductive { get; set; } = true;
        public double PercentageAssetNoProductive { get; set; } = 19;
        public double PercentageEngineering { get; set; } = 6;
        public double PercentageContingency { get; set; } = 10;
        public double PercentageTaxForAlterations { get; set; } = 19;
        public async Task ChangeName(string name)
        {
            
            Name = name;
            if (Validator != null) await Validator();

        }

        public async Task ChangePercentageTaxes(string stringpercentage)
        {

            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
            {

            }

            PercentageAssetNoProductive = percentage;
            if (Validator != null) await Validator();
        }
        public async Task ChangePercentageEngineering(string stringpercentage)
        {

            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
            {

            }

            PercentageEngineering = percentage;
            if (Validator != null) await Validator();

        }
        public async Task ChangePercentageContingency(string stringpercentage)
        {

            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
            {

            }


            PercentageContingency = percentage;
            if (Validator != null) await Validator();

        }
        public async Task ChangeTaxForAlterations(string stringpercentage)
        {

            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
            {

            }

            PercentageTaxForAlterations = percentage;
            if (Validator != null) await Validator();

        }
        public async Task ChangeType()
        {

            if (Validator != null) await Validator();
        }

    }
}
