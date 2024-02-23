using Application.Interfaces;
using FluentValidation;
using Shared.Models.PurchaseOrders.Requests.Create;

namespace Application.Features.PurchaseOrders.Validators
{
    internal class EditPurchaseOrderCreatedValidator : AbstractValidator<EditPurchaseOrderCreatedRequest>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;
        public EditPurchaseOrderCreatedValidator(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            RuleFor(x => x).MustAsync(ReviewIfPRExist).WithMessage(x => $"{x.PurchaseRequisition} already exist");

        }
        async Task<bool> ReviewIfPRExist(EditPurchaseOrderCreatedRequest PO, CancellationToken cancellationToken)
        {
            return !(await _purchaseOrderRepository.ReviewIfPurchaseRequisitionExist(PO.PurchaseorderId,PO.PurchaseRequisition));
        }
    }
}
