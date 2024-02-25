using Shared.Models.PurchaseOrders.Requests.Taxes;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class CreateTaxPurchaseOrderValidator : AbstractValidator<CreateTaxPurchaseOrderRequest>
    {
        public CreateTaxPurchaseOrderValidator()
        {
           

            RuleFor(X => X.Name).NotEmpty().WithMessage("Purchase order name must be defined");
  
            RuleFor(X => X.PONumber).Must(x => x.StartsWith("850")).WithMessage("PO must start with 850");
            RuleFor(X => X.PONumber).Length(10).WithMessage("PO number must 10 characters");

            RuleFor(x => x.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.SumPOValueUSD).GreaterThan(0).WithMessage("PO Value must be defined");

           
        }
    }
}
