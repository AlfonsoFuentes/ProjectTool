using Application.Interfaces;
using FluentValidation;
using Shared.Models.PurchaseOrders.Requests.Taxes;

namespace Application.Features.PurchaseOrders.Validators.RegularPurchaseOrders
{
    internal class EditTaxPurchaseOrderValidator : AbstractValidator<EditTaxPurchaseOrderRequest>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;
        public EditTaxPurchaseOrderValidator(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            RuleFor(x => x).MustAsync(ReviewNameExist).WithMessage(x => $"{x.PurchaseorderName} already exist");

        }
        async Task<bool> ReviewNameExist(EditTaxPurchaseOrderRequest po, CancellationToken cancellationToken)
        {
            return !await _purchaseOrderRepository.ReviewIfNameExist(po.PurchaseorderId, po.PurchaseorderName);
        }
    }
}
