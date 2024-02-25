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
            RuleFor(x => x).MustAsync(ReviewNameExist).WithMessage(x => $"{x.Name} already exist");
        }
        async Task<bool> ReviewIfPRExist(CreatePurchaseOrderRequest po, CancellationToken cancellationToken)
        {
            return !(await _purchaseOrderRepository.ReviewIfPurchaseRequisitionExist(Guid.Empty, po.PurchaseRequisition));
        }
        async Task<bool> ReviewNameExist(CreatePurchaseOrderRequest po, CancellationToken cancellationToken)
        {
            return !(await _purchaseOrderRepository.ReviewIfNameExist(Guid.Empty, po.Name));
        }
    }
}
