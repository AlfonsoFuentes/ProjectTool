using Application.Interfaces;
using FluentValidation;

namespace Application.Features.PurchaseOrders.Validators
{
    //internal class UpdatePurchaseOrderMinimalValidator : AbstractValidator<UpdatePurchaseOrderMinimalRequest>
    //{
    //    private IPurchaseOrderRepository _purchaseOrderRepository;
    //    public UpdatePurchaseOrderMinimalValidator(IPurchaseOrderRepository purchaseOrderRepository)
    //    {
    //        _purchaseOrderRepository = purchaseOrderRepository;
    //        RuleFor(x => x).MustAsync(ReviewIfNameExist).WithMessage(x => $"{x.PurchaseOrderName} already exist");

    //    }
    //    async Task<bool> ReviewIfNameExist(UpdatePurchaseOrderMinimalRequest PO, CancellationToken cancellationToken)
    //    {
    //        return !(await _purchaseOrderRepository.ReviewIfNameExist(PO.PurchaseOrderId, PO.PurchaseOrderName));
    //    }
    //}
}
