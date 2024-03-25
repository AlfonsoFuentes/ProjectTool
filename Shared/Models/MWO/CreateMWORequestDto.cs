namespace Shared.Models.MWO
{
    public class CreateMWORequestDto
    {

        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }

        public bool IsAssetProductive { get; set; } = true;
        public double PercentageAssetNoProductive { get; set; } = 19;
        public double PercentageEngineering { get; set; } = 6;
        public double PercentageContingency { get; set; } = 10;
        public double PercentageTaxForAlterations { get; set; } = 19;


    }
}
