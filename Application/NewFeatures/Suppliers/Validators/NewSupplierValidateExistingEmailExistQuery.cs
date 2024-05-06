namespace Application.NewFeatures.Suppliers.Validators
{
    public record NewSupplierValidateExistingEmailExistQuery(Guid SupplierId, string? Email) : IRequest<bool>;
    public class NewSupplierValidateExistingEmailExistQueryHandler : IRequestHandler<NewSupplierValidateExistingEmailExistQuery, bool>
    {
        private readonly IQueryValidationsRepository repository;

        public NewSupplierValidateExistingEmailExistQueryHandler(IQueryValidationsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(NewSupplierValidateExistingEmailExistQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.ReviewSupplierEmailExist(request.SupplierId, request.Email);
            return result;
        }
    }
}
