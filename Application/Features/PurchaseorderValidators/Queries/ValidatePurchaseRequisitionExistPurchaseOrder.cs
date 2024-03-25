using Application.Interfaces;
using MediatR;

namespace Application.Features.PurchaseorderValidators.Queries
{
    public record ValidatePurchaseRequisitionExistPurchaseOrder(string purchaserequisition) : IRequest<bool>;
    internal class ValidatePurchaseRequisitionExistPurchaseOrderHandler : IRequestHandler<ValidatePurchaseRequisitionExistPurchaseOrder, bool>
    {
        private IPurchaseOrderValidatorRepository _repository;

        public ValidatePurchaseRequisitionExistPurchaseOrderHandler(IPurchaseOrderValidatorRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ValidatePurchaseRequisitionExistPurchaseOrder request, CancellationToken cancellationToken)
        {
            return await _repository.ValidatePurchaseRequisition(request.purchaserequisition);
        }
    }

}
