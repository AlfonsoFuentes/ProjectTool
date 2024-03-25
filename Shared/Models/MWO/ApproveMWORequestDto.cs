namespace Shared.Models.MWO
{
    public class ApproveMWORequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MWONumber { get; set; } = string.Empty;
        public int CostCenter { get; set; }
        public double PercentageTaxForAlterations { get; set; }
        public bool IsAssetProductive { get; set; } = true;
        public double PercentageAssetNoProductive { get; set; } = 19;
        public double PercentageEngineering { get; set; } = 6;
        public double PercentageContingency { get; set; } = 10;


    }
}
