namespace Shared.Models.Brands
{
    public class CreateBrandRequestDto
    {

      
        public string Name { get; set; } = string.Empty;
        public void ConvertToDto(CreateBrandRequest request)
        {
            Name=request.Name;

        }
       
    }
}
