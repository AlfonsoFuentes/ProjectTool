using Application.Interfaces;
using MediatR;

namespace Application.Features.Suppliers.Queries
{
    public record SupplierCreateNameExistQuery(string Name) : IRequest<bool>;
    public class SupplierNameExistQueryHandler : IRequestHandler<SupplierCreateNameExistQuery, bool>
    {
        private readonly ISupplierRepository repository;

        public SupplierNameExistQueryHandler(ISupplierRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(SupplierCreateNameExistQuery request, CancellationToken cancellationToken)
        {
            return await repository.ReviewNameExist(request.Name);
        }
    }

}
