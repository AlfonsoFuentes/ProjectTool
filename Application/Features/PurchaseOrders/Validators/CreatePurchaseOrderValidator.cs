using Application.Interfaces;
using FluentValidation;
using Shared.Models.PurchaseOrders.Requests.Create;

namespace Application.Features.PurchaseOrders.Validators
{
    internal class CreatePurchaseOrderValidator : AbstractValidator<CreatePurchaseOrderRequest>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;
        public CreatePurchaseOrderValidator(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            RuleFor(x => x).MustAsync(ReviewIfPRExist).WithMessage(x => $"{x.PurchaseRequisition} already exist");

        }
        async Task<bool> ReviewIfPRExist(CreatePurchaseOrderRequest po, CancellationToken cancellationToken)
        {
            return !(await _purchaseOrderRepository.ReviewIfPurchaseRequisitionExist(Guid.Empty, po.PurchaseRequisition));
        }
    }
}
