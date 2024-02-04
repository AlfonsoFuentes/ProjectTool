using Application.Interfaces;
using MediatR;
using Shared.Models.Suppliers;

namespace Application.Features.Suppliers.Queries
{
    public record SupplierCreateVendorCodeExistQuery(string VendorCode) : IRequest<bool>;
    public class SupplierCreateVendorCodeExistQueryHandler : IRequestHandler<SupplierCreateVendorCodeExistQuery, bool>
    {
        private readonly ISupplierRepository repository;

        public SupplierCreateVendorCodeExistQueryHandler(ISupplierRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(SupplierCreateVendorCodeExistQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.ReviewVendorCodeExist(request.VendorCode);
            return result;
        }
    }

}
