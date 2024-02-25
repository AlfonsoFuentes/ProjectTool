using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class CreateCapitalizedSalaryPurchaseOrderValidator : AbstractValidator<CreateCapitalizedSalaryPurchaseOrderRequest>
    {
        public CreateCapitalizedSalaryPurchaseOrderValidator()
        {


            RuleFor(X => X.PurchaseOrderName).NotEmpty().WithMessage("Purchase order name must be defined");
            RuleFor(X => X.PurchaseOrderName).NotNull().WithMessage("Purchase order name must be defined");

            RuleFor(X => X.PurchaseorderNumber).Must(x => x.StartsWith("850")).When(x=>x.IsCapitalizedSalary==false).WithMessage("PO must start with 850");
            RuleFor(X => X.PurchaseorderNumber).Length(10).When(x => x.IsCapitalizedSalary == false).WithMessage("PO number must 10 characters");

            RuleFor(x => x.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.SumPOValueUSD).GreaterThan(0).WithMessage("PO Value must be defined");


        }
    }
}
