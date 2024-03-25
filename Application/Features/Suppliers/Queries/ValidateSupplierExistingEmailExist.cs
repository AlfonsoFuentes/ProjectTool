using Application.Interfaces;
using MediatR;

namespace Application.Features.Suppliers.Queries
{
    public record ValidateSupplierExistingEmailExist(Guid SupplierId,string? Email) : IRequest<bool>;
    public class ValidateSupplierExistingEmailExistHandler : IRequestHandler<ValidateSupplierExistingEmailExist, bool>
    {
        private readonly ISupplierRepository repository;

        public ValidateSupplierExistingEmailExistHandler(ISupplierRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateSupplierExistingEmailExist request, CancellationToken cancellationToken)
        {
            var result = await repository.ReviewEmailExist(request.SupplierId, request.Email);
            return result;
        }
    }
}
