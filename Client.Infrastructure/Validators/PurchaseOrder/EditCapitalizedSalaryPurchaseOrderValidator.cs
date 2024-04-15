using Client.Infrastructure.Managers.PurchaseOrders;
using FluentValidation;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class EditCapitalizedSalaryPurchaseOrderValidator : AbstractValidator<EditCapitalizedSalaryPurchaseOrderRequest>
    {
        private IPurchaseOrderValidator PurchaseOrderValidator { get; set; }
        public EditCapitalizedSalaryPurchaseOrderValidator(IPurchaseOrderValidator purchaseOrderValidator)
        {


            RuleFor(X => X.PurchaseOrderName).NotEmpty().WithMessage("Purchase order name must be defined");
            RuleFor(X => X.PurchaseOrderName).NotNull().WithMessage("Purchase order name must be defined");

            RuleFor(X => X.PurchaseorderNumber).Must(x => x.StartsWith("850"))
                .When(x => x.IsCapitalizedSalary == false && !string.IsNullOrEmpty(x.PurchaseorderNumber)).WithMessage("PO must start with 850");

            RuleFor(X => X.PurchaseorderNumber).Length(10)
                .When(x => x.IsCapitalizedSalary == false&& !string.IsNullOrEmpty(x.PurchaseorderNumber)).WithMessage("PO number must 10 characters");

            RuleFor(x => x.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.SumPOValueUSD).GreaterThan(0).WithMessage("PO Value must be defined");
            RuleFor(x => x.PurchaseOrderName).MustAsync(ReviewNameExist).When(x =>
            !string.IsNullOrEmpty(x.PurchaseOrderName)).WithMessage(x => $"{x.PurchaseOrderName} already exist"); ;

            RuleFor(x => x.PurchaseorderNumber).MustAsync(ReviewIfPOExist)
                .When(x => x.IsCapitalizedSalary == false && !string.IsNullOrEmpty(x.PurchaseorderNumber)).WithMessage(x => $"{x.PurchaseorderNumber} already exist");
            PurchaseOrderValidator = purchaseOrderValidator;
        }
        async Task<bool> ReviewIfPOExist(EditCapitalizedSalaryPurchaseOrderRequest po,string por, CancellationToken cancellationToken)
        {
            return !await PurchaseOrderValidator.ValidatePONumberExistInPurchaseOrder(po.PurchaseOrderId, por);
        }
        async Task<bool> ReviewNameExist(EditCapitalizedSalaryPurchaseOrderRequest po,string name, CancellationToken cancellationToken)
        {
            return !await PurchaseOrderValidator.ValidateNameExistInPurchaseOrder(po.MWOId,po.PurchaseOrderId, name);
        }
    }
}
