using Application.Interfaces;
using MediatR;

namespace Application.Features.Suppliers.Queries
{
    public record ValidateSupplierExistingVendorCodeExistQuery(Guid SupplierId,string VendorCode) : IRequest<bool>;
    public class ValidateSupplierExitingVendorCodeExistQueryHandler : IRequestHandler<ValidateSupplierExistingVendorCodeExistQuery, bool>
    {
        private readonly ISupplierRepository repository;

        public ValidateSupplierExitingVendorCodeExistQueryHandler(ISupplierRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateSupplierExistingVendorCodeExistQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.ReviewVendorCodeExist(request.SupplierId,request.VendorCode);
            return result;
        }
    }

}
