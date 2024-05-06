namespace Application.NewFeatures.Suppliers.Validators
{
    public record NewSupplierValidateNameQuery(string Name) : IRequest<bool>;
    public class NewSupplierValidateNameExistQueryHandler : IRequestHandler<NewSupplierValidateNameQuery, bool>
    {
        private readonly IQueryValidationsRepository repository;

        public NewSupplierValidateNameExistQueryHandler(IQueryValidationsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(NewSupplierValidateNameQuery request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfSupplierNameExist(request.Name);
        }
    }

}
