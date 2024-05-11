using Shared.NewModels.PurchaseOrders.Request;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class OldPurchaseOrderReceiveValidator : AbstractValidator<OldPurchaseOrderReceiveRequest>
    {
        public OldPurchaseOrderReceiveValidator()
        {
            RuleFor(x => x.Supplier).NotNull().WithMessage("Supplier must be defined");

            RuleFor(x => x.PendingCurrency).GreaterThanOrEqualTo(0)
               .WithMessage("Pending cannot be negative");
        }
    }
}
