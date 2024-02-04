namespace Shared.Models.Brands
{
    public class UpdateBrandRequest
    {
        public Guid Id { get; set; }    
        public string Name { get; set; } = string.Empty;
    }
}
