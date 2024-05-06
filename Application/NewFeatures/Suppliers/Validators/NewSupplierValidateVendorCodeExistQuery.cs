namespace Application.NewFeatures.Suppliers.Validators
{
    public record NewSupplierValidateVendorCodeExistQuery(Guid SupplierId, string VendorCode) : IRequest<bool>;
    public class NewSupplierValidateExistingVendorCodeExistQueryHandler : IRequestHandler<NewSupplierValidateVendorCodeExistQuery, bool>
    {
        private readonly IQueryValidationsRepository repository;

        public NewSupplierValidateExistingVendorCodeExistQueryHandler(IQueryValidationsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(NewSupplierValidateVendorCodeExistQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.ReviewSupplierVendorCodeExist(request.SupplierId, request.VendorCode);
            return result;
        }
    }

}
