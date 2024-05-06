namespace Application.NewFeatures.Suppliers.Validators
{
    public record NewSupplierValidateVendorCodeQuery(string VendorCode) : IRequest<bool>;
    public class NewSupplierValidateVendorCodeExistQueryHandler : IRequestHandler<NewSupplierValidateVendorCodeQuery, bool>
    {
        private readonly IQueryValidationsRepository repository;

        public NewSupplierValidateVendorCodeExistQueryHandler(IQueryValidationsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(NewSupplierValidateVendorCodeQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.ReviewSupplierVendorCodeExist(request.VendorCode);
            return result;
        }
    }

}
