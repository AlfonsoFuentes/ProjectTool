using Application.Interfaces;
using MediatR;
using Shared.Models.Suppliers;

namespace Application.Features.Suppliers.Queries
{
    public record ValidateSupplierVendorCodeExistQuery(string VendorCode) : IRequest<bool>;
    public class ValidateSupplierVendorCodeExistQueryHandler : IRequestHandler<ValidateSupplierVendorCodeExistQuery, bool>
    {
        private readonly ISupplierRepository repository;

        public ValidateSupplierVendorCodeExistQueryHandler(ISupplierRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateSupplierVendorCodeExistQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.ReviewVendorCodeExist(request.VendorCode);
            return result;
        }
    }

}
