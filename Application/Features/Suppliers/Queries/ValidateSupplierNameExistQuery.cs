using Application.Interfaces;
using MediatR;

namespace Application.Features.Suppliers.Queries
{
    public record ValidateSupplierNameExistQuery(string Name) : IRequest<bool>;
    public class ValidateSupplierNameExistQueryHandler : IRequestHandler<ValidateSupplierNameExistQuery, bool>
    {
        private readonly ISupplierRepository repository;

        public ValidateSupplierNameExistQueryHandler(ISupplierRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateSupplierNameExistQuery request, CancellationToken cancellationToken)
        {
            return await repository.ReviewNameExist(request.Name);
        }
    }

}
