namespace Shared.Models.Brands
{
    public class BrandResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
    }
}
