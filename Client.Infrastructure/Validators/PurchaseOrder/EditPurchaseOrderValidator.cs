using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.Create;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class EditPurchaseOrderValidator : AbstractValidator<EditPurchaseOrderCreatedRequest>
    {
        public EditPurchaseOrderValidator()
        {
            RuleFor(x => x.SupplierId).NotEqual(Guid.Empty).WithMessage("Supplier must be defined");

            RuleFor(X => X.PurchaseOrderName).NotEmpty().WithMessage("Purchase order name must be defined");
            RuleFor(X => X.PurchaseOrderName).NotNull().WithMessage("Purchase order name must be defined");

            RuleFor(X => X.QuoteNo).NotEmpty().WithMessage("Quote name must be defined");
            RuleFor(X => X.QuoteNo).NotNull().WithMessage("Quote name must be defined");
            
            RuleFor(x => x.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.SumPOValueUSD).GreaterThan(0).WithMessage("PO Value must be defined");

            RuleFor(x => x.QuoteCurrency).Must(x => x.Id != CurrencyEnum.None.Id).WithMessage("Quote currency must be defined");
        }
    }
}
