

namespace Application.Mappers.Brands
{
    public static class BrandMapper
    {

        public static void FromRequest(this NewBrandUpdateRequest request, Brand brand)
        {
            brand.Name = request.Name;
            
        }

        public static NewBrandResponse ToResponse(this Brand brand)
        {
            return new NewBrandResponse()
            {
                BrandId = brand.Id,
                Name = brand.Name,
            };
        }
        public static NewBrandUpdateRequest ToUpdateRequest(this Brand brand)
        {
            return new NewBrandUpdateRequest()
            {
                BrandId = brand.Id,
                Name = brand.Name,
            };
        }
    }
}
