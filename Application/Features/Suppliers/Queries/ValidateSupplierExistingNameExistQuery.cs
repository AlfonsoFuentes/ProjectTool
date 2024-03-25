using Application.Interfaces;
using MediatR;

namespace Application.Features.Suppliers.Queries
{
    public record ValidateSupplierExistingNameExistQuery(Guid SupplierId, string Name) : IRequest<bool>;
    public class ValidateSupplierExistingNameExistQueryHandler : IRequestHandler<ValidateSupplierExistingNameExistQuery, bool>
    {
        private readonly ISupplierRepository repository;

        public ValidateSupplierExistingNameExistQueryHandler(ISupplierRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateSupplierExistingNameExistQuery request, CancellationToken cancellationToken)
        {
            return await repository.ReviewNameExist(request.SupplierId, request.Name);
        }

    }
}
