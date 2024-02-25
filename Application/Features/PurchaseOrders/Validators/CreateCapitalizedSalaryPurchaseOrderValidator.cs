using Application.Interfaces;
using FluentValidation;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;

namespace Application.Features.PurchaseOrders.Validators
{
    internal class CreateCapitalizedSalaryPurchaseOrderValidator : AbstractValidator<CreateCapitalizedSalaryPurchaseOrderRequest>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;
        public CreateCapitalizedSalaryPurchaseOrderValidator(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            RuleFor(x => x).MustAsync(ReviewIfPOExist).When(x=>x.IsCapitalizedSalary==false).WithMessage(x => $"{x.PurchaseorderNumber} already exist");
            RuleFor(x => x).MustAsync(ReviewNameExist).WithMessage(x => $"{x.PurchaseOrderName} already exist");
        }
        async Task<bool> ReviewIfPOExist(CreateCapitalizedSalaryPurchaseOrderRequest po, CancellationToken cancellationToken)
        {
            return !(await _purchaseOrderRepository.ReviewIfPurchaseOrderExist(Guid.Empty, po.PurchaseorderNumber));
        }
        async Task<bool> ReviewNameExist(CreateCapitalizedSalaryPurchaseOrderRequest po, CancellationToken cancellationToken)
        {
            return !(await _purchaseOrderRepository.ReviewIfNameExist(Guid.Empty, po.PurchaseOrderName));
        }
    }
}
