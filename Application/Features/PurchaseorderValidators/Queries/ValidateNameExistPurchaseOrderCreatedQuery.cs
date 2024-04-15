using Application.Interfaces;
using MediatR;

namespace Application.Features.PurchaseorderValidators.Queries
{
    public record ValidateNameExistPurchaseOrderCreatedQuery(Guid MWOId,Guid PurchaseOrderId, string name) : IRequest<bool>;

    internal class ValidateNameExistPurchaseOrderCreatedQueryHandler : IRequestHandler<ValidateNameExistPurchaseOrderCreatedQuery, bool>
    {
        private IPurchaseOrderValidatorRepository _repository;

        public ValidateNameExistPurchaseOrderCreatedQueryHandler(IPurchaseOrderValidatorRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ValidateNameExistPurchaseOrderCreatedQuery request, CancellationToken cancellationToken)
        {
            return await _repository.ValidateNameExist(request.MWOId, request.PurchaseOrderId, request.name);
        }
    }

}
