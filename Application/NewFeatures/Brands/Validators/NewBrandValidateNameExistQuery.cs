namespace Application.NewFeatures.Brands.Validators
{
    public record NewBrandValidateNameExistQuery(Guid BrandId, string Name) : IRequest<bool>;
    public class NewBrandValidateNameExistingNameExistQueryHandler : IRequestHandler<NewBrandValidateNameExistQuery, bool>
    {
        private readonly IQueryValidationsRepository repository;

        public NewBrandValidateNameExistingNameExistQueryHandler(IQueryValidationsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(NewBrandValidateNameExistQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.ReviewIfBrandNameExist(request.BrandId, request.Name);
            return result;
        }
    }
}
