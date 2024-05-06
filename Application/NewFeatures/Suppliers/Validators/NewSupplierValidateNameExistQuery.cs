namespace Application.NewFeatures.Suppliers.Validators
{
    public record NewSupplierValidateNameExistQuery(Guid SupplierId, string Name) : IRequest<bool>;
    public class NewSupplierValidateExistingNameExistQueryHandler : IRequestHandler<NewSupplierValidateNameExistQuery, bool>
    {
        private readonly IQueryValidationsRepository repository;

        public NewSupplierValidateExistingNameExistQueryHandler(IQueryValidationsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(NewSupplierValidateNameExistQuery request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfSupplierNameExist(request.SupplierId, request.Name);
        }

    }
}
