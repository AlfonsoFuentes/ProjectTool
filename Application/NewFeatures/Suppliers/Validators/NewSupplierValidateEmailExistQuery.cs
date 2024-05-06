namespace Application.NewFeatures.Suppliers.Validators
{
    public record NewSupplierValidateEmailQuery(string? Email) : IRequest<bool>;
    public class NewSupplierValidateEmailExistQueryHandler : IRequestHandler<NewSupplierValidateEmailQuery, bool>
    {
        private readonly IQueryValidationsRepository repository;

        public NewSupplierValidateEmailExistQueryHandler(IQueryValidationsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(NewSupplierValidateEmailQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.ReviewSupplierEmailExist(request.Email);
            return result;
        }
    }
}
