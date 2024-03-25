namespace Shared.Models.Brands
{
    public class UpdateBrandRequestDto
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public void ConvertToDto(UpdateBrandRequest request)
        {
            Id = request.Id;
            Name = request.Name;
        }
    }
}
