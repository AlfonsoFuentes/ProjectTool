using Application.Interfaces;
using MediatR;
using Shared.Models.Suppliers;

namespace Application.Features.Suppliers.Queries
{
    public record SupplierCreateEmailExistQuery(string? Email) : IRequest<bool>;
    public class SupplierCreateEmailExistQueryHandler : IRequestHandler<SupplierCreateEmailExistQuery, bool>
    {
        private readonly ISupplierRepository repository;

        public SupplierCreateEmailExistQueryHandler(ISupplierRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(SupplierCreateEmailExistQuery request, CancellationToken cancellationToken)
        {
            var result = await repository.ReviewEmailExist(request.Email);
            return result;
        }
    }
}
