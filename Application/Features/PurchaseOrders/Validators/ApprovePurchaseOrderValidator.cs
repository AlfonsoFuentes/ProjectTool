using Application.Interfaces;
using FluentValidation;
using Shared.Models.PurchaseOrders.Requests.Approves;

namespace Application.Features.PurchaseOrders.Validators
{
    internal class ApprovePurchaseOrderValidator : AbstractValidator<ApprovePurchaseOrderRequest>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;
        public ApprovePurchaseOrderValidator(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            RuleFor(x => x).MustAsync(ReviewIfPOExist).WithMessage(x => $"{x.PONumber} already exist");

        }
        async Task<bool> ReviewIfPOExist(ApprovePurchaseOrderRequest PO, CancellationToken cancellationToken)
        {
            return !(await _purchaseOrderRepository.ReviewIfPurchaseOrderExist(PO.PurchaseorderId,PO.PONumber));
        }
    }
}
