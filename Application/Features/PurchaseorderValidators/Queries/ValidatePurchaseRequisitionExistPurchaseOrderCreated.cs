using Application.Interfaces;
using MediatR;

namespace Application.Features.PurchaseorderValidators.Queries
{
    public record ValidatePurchaseRequisitionExistPurchaseOrderCreated(Guid PurchaseOrderId,string purchaserequisition) : IRequest<bool>;
    internal class ValidatePurchaseRequisitionExistPurchaseOrderCreatedHandler : IRequestHandler<ValidatePurchaseRequisitionExistPurchaseOrderCreated, bool>
    {
        private IPurchaseOrderValidatorRepository _repository;

        public ValidatePurchaseRequisitionExistPurchaseOrderCreatedHandler(IPurchaseOrderValidatorRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ValidatePurchaseRequisitionExistPurchaseOrderCreated request, CancellationToken cancellationToken)
        {
            return await _repository.ValidatePurchaseRequisition(request.PurchaseOrderId, request.purchaserequisition);
        }
    }
}
