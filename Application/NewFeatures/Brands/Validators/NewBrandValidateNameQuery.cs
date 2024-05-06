namespace Application.NewFeatures.Brands.Validators
{
    public record NewBrandValidateNameQuery(string Name) : IRequest<bool>;
    public class NewBrandValidateNameQueryHandler : IRequestHandler<NewBrandValidateNameQuery, bool>
    {
        private readonly IQueryValidationsRepository repository;

        public NewBrandValidateNameQueryHandler(IQueryValidationsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(NewBrandValidateNameQuery request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfBrandNameExist(request.Name);
        }
    }


}
