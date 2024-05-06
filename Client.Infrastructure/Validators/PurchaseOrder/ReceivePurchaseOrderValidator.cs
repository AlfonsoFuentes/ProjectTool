using Shared.Enums.WayToReceivePurchaseOrdersEnums;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class ReceivePurchaseOrderValidator : AbstractValidator<ReceiveRegularPurchaseOrderRequest>
    {
        public ReceivePurchaseOrderValidator()
        {
            RuleFor(x => x.Supplier).NotNull().WithMessage("Supplier must be defined");
            RuleFor(x => x.TRMUSDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.TRMUSDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.PercentageToReceive).GreaterThan(0)
                .When(x=>x.WayToReceivePurchaseOrder.Id== WayToReceivePurchaseorderEnum.PercentageOrder.Id).WithMessage("Percentage must be greater than zero");
            RuleFor(x => x.PercentageToReceive).LessThanOrEqualTo(100)
               .When(x => x.WayToReceivePurchaseOrder.Id == WayToReceivePurchaseorderEnum.PercentageOrder.Id).WithMessage("Percentage must be greater than 100");
            RuleFor(x => x.SumPOPendingUSD).GreaterThanOrEqualTo(0)
               .WithMessage("Pending dont be negative");
        }
    }
}
