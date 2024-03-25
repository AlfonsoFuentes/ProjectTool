using Application.Interfaces;
using FluentValidation;
using Shared.Models.PurchaseOrders.Requests.Taxes;

namespace Application.Features.PurchaseOrders.Validators.RegularPurchaseOrders
{
    internal class CreateTaxPurchaseOrderValidator : AbstractValidator<CreateTaxPurchaseOrderRequest>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;
        public CreateTaxPurchaseOrderValidator(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            RuleFor(x => x).MustAsync(ReviewNameExist).WithMessage(x => $"{x.Name} already exist");

        }
        async Task<bool> ReviewNameExist(CreateTaxPurchaseOrderRequest po, CancellationToken cancellationToken)
        {
            return !await _purchaseOrderRepository.ReviewIfNameExist(Guid.Empty, po.Name);
        }
    }
}
