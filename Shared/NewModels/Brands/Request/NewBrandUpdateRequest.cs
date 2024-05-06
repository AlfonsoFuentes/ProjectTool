namespace Shared.NewModels.Brands.Request
{
    public class NewBrandUpdateRequest
    {
        public Guid BrandId { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}
