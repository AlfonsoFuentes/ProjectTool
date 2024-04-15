using Client.Infrastructure.Managers.PurchaseOrders;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class CreateCapitalizedSalaryPurchaseOrderValidator : AbstractValidator<CreateCapitalizedSalaryPurchaseOrderRequest>
    {
        private IPurchaseOrderValidator PurchaseOrderValidator { get; set; }
        public CreateCapitalizedSalaryPurchaseOrderValidator(IPurchaseOrderValidator purchaseOrderValidator)
        {


            RuleFor(X => X.PurchaseOrderName).NotEmpty().WithMessage("Purchase order name must be defined");
            RuleFor(X => X.PurchaseOrderName).NotNull().WithMessage("Purchase order name must be defined");

            RuleFor(X => X.PurchaseorderNumber).Must(x => x.StartsWith("850"))
                .When(x => x.IsCapitalizedSalary == false && !string.IsNullOrEmpty(x.PurchaseorderNumber)).WithMessage("PO must start with 850");
            RuleFor(X => X.PurchaseorderNumber).Length(10)
                .When(x => x.IsCapitalizedSalary == false && !string.IsNullOrEmpty(x.PurchaseorderNumber)).WithMessage("PO number must 10 characters");

            RuleFor(x => x.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.SumPOValueUSD).GreaterThan(0).WithMessage("PO Value must be defined");
            PurchaseOrderValidator = purchaseOrderValidator;
            RuleFor(x => x.PurchaseOrderName).MustAsync(ReviewNameExist).When(x => !string.IsNullOrEmpty(x.PurchaseOrderName))
                .WithMessage(x => $"{x.PurchaseOrderName} already exist"); ;

            RuleFor(x => x.PurchaseorderNumber).MustAsync(ReviewPONumberExist)
                .When(x => x.IsCapitalizedSalary == false && !string.IsNullOrEmpty(x.PurchaseorderNumber)).WithMessage(x => $"{x.PurchaseorderNumber} already exist");
        }
        async Task<bool> ReviewPONumberExist(CreateCapitalizedSalaryPurchaseOrderRequest Purchaseorder, string ponumber, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidatePONumberExistInPurchaseOrder(Guid.Empty, ponumber);
            return !result;
        }
        async Task<bool> ReviewNameExist(CreateCapitalizedSalaryPurchaseOrderRequest Purchaseorder, string name, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidateNameExistInPurchaseOrder(Purchaseorder.MWOId,name);
            return !result;
        }
    }
}
