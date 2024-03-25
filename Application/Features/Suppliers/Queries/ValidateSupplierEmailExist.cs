using Application.Interfaces;
using MediatR;
using Shared.Models.Suppliers;

namespace Application.Features.Suppliers.Queries
{
    public record ValidateSupplierEmailExist(string? Email) : IRequest<bool>;
    public class SupplierCreateEmailExistQueryHandler : IRequestHandler<ValidateSupplierEmailExist, bool>
    {
        private readonly ISupplierRepository repository;

        public SupplierCreateEmailExistQueryHandler(ISupplierRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateSupplierEmailExist request, CancellationToken cancellationToken)
        {
            var result = await repository.ReviewEmailExist(request.Email);
            return result;
        }
    }
}
