using Shared.Models.PurchaseOrders.Requests.Approves;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class ApprovePurchaseOrderValidator : AbstractValidator<ApprovePurchaseOrderRequest>
    {
        public ApprovePurchaseOrderValidator()
        {
            RuleFor(x => x.SupplierId).NotEqual(Guid.Empty).WithMessage("Supplier must be defined");
            RuleFor(X => X.PONumber).Must(x => x.StartsWith("850")).WithMessage("PO must start with 850");
            RuleFor(X => X.PONumber).Length(10).WithMessage("PO number must 10 characters");
            RuleFor(customer => customer.PONumber).Matches("^[0-9]*$").WithMessage("PO Number must be number!");
            RuleFor(X => X.ExpetedOn).NotNull().WithMessage("Expected PO must be defined");
        }
    }

}
