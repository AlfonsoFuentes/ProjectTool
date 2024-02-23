using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class CreatePurchaseOrderValidator : AbstractValidator<CreatePurchaseOrderRequest>
    {
        public CreatePurchaseOrderValidator()
        {
            RuleFor(x => x.SupplierId).NotEqual(Guid.Empty).WithMessage("Supplier must be defined");

            RuleFor(X => X.Name).NotEmpty().WithMessage("Purchase order name must be defined");
            RuleFor(X => X.Name).NotNull().WithMessage("Purchase order name must be defined");

            RuleFor(X => X.QuoteNo).NotEmpty().WithMessage("Quote name must be defined");
            RuleFor(X => X.QuoteNo).NotNull().WithMessage("Quote name must be defined");

            RuleFor(X => X.PurchaseRequisition).NotEmpty().WithMessage("PR must be defined");
            RuleFor(X => X.PurchaseRequisition).NotNull().WithMessage("PR must be defined");

            RuleFor(X => X.PurchaseRequisition).Must(x => x.StartsWith("PR")).WithMessage("PR must include PR letter at start");
            RuleFor(X => X.PurchaseRequisition).Length(8).WithMessage("PR must 8 characters");

           

            RuleFor(x => x.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.SumPOValueUSD).GreaterThan(0).WithMessage("PO Value must be defined");

            RuleFor(x => x.QuoteCurrency).Must(x => x.Id != CurrencyEnum.None.Id).WithMessage("Quote currency must be defined");
        }
    }

}
