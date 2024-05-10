using Shared.NewModels.PurchaseOrders.Request;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class NewPurchaseOrderReceiveValidator : AbstractValidator<NewPurchaseOrderReceiveRequest>
    {
        public NewPurchaseOrderReceiveValidator()
        {
            RuleFor(x => x.Supplier).NotNull().WithMessage("Supplier must be defined");
         
            RuleFor(x => x.PendingCurrency).GreaterThanOrEqualTo(0)
               .WithMessage("Pending cannot be negative");
        }
    }
}
