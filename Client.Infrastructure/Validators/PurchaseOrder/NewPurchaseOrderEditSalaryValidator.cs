using Client.Infrastructure.Managers.PurchaseOrders;
using Shared.NewModels.PurchaseOrders.Request;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class NewPurchaseOrderEditSalaryValidator : AbstractValidator<NewPurchaseOrderEditSalaryRequest>
    {
        private INewPurchaseOrderValidator PurchaseOrderValidator { get; set; }
        public NewPurchaseOrderEditSalaryValidator(INewPurchaseOrderValidator purchaseOrderValidator)
        {


            RuleFor(X => X.PurchaseorderName).NotEmpty().WithMessage("Purchase order name must be defined");
            RuleFor(X => X.PurchaseorderName).NotNull().WithMessage("Purchase order name must be defined");

            RuleFor(X => X.PurchaseOrderNumber).Must(x => x.StartsWith("850"))
                .When(x => x.CreatePurchaseOrderNumber == false && !string.IsNullOrEmpty(x.PurchaseOrderNumber)).WithMessage("PO must start with 850");

            RuleFor(X => X.PurchaseOrderNumber).Length(10)
                .When(x => x.CreatePurchaseOrderNumber == false && !string.IsNullOrEmpty(x.PurchaseOrderNumber)).WithMessage("PO number must 10 characters");

            RuleFor(x => x.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.POValueUSD).GreaterThan(0).WithMessage("PO Value must be defined");
            RuleFor(x => x.PurchaseorderName).MustAsync(ReviewNameExist).When(x =>
            !string.IsNullOrEmpty(x.PurchaseorderName)).WithMessage(x => $"{x.PurchaseorderName} already exist"); ;

            RuleFor(x => x.PurchaseOrderNumber).MustAsync(ReviewIfPOExist)
                .When(x => x.CreatePurchaseOrderNumber == false && !string.IsNullOrEmpty(x.PurchaseOrderNumber)).WithMessage(x => $"{x.PurchaseOrderNumber} already exist");
            PurchaseOrderValidator = purchaseOrderValidator;
        }
        async Task<bool> ReviewIfPOExist(NewPurchaseOrderEditSalaryRequest po, string por, CancellationToken cancellationToken)
        {
            return !await PurchaseOrderValidator.ValidatePONumberExistInPurchaseOrder(po.PurchaseOrderId, por);
        }
        async Task<bool> ReviewNameExist(NewPurchaseOrderEditSalaryRequest po, string name, CancellationToken cancellationToken)
        {
            return !await PurchaseOrderValidator.ValidateNameExistInPurchaseOrder(po.MWOId, po.PurchaseOrderId, name);
        }
    }
}
