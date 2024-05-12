using FluentValidation;

namespace Client.Infrastructure.Validators.PurchaseOrder.News
{
    public class NewPurchaseOrderCreateSalaryValidator : AbstractValidator<NewPurchaseOrderCreateSalaryRequest>
    {
        INewPurchaseOrderValidator PurchaseOrderValidator { get; set; }
        public NewPurchaseOrderCreateSalaryValidator(INewPurchaseOrderValidator purchaseOrderValidator)
        {
            PurchaseOrderValidator = purchaseOrderValidator;
            RuleFor(X => X.PurchaseOrder.PurchaseOrderName).NotEmpty().WithMessage("Purchase order name must be defined");
            RuleFor(X => X.PurchaseOrder.PurchaseOrderName).NotNull().WithMessage("Purchase order name must be defined");

            RuleFor(X => X.PurchaseOrder.PurchaseOrderNumber).Must(x => x.StartsWith("850"))
                .When(x => x.CreatePurchaseOrderNumber == false && !string.IsNullOrEmpty(x.PurchaseOrder.PurchaseOrderNumber)).WithMessage("PO must start with 850");
            RuleFor(X => X.PurchaseOrder.PurchaseOrderNumber).Length(10)
                .When(x => x.CreatePurchaseOrderNumber == false && !string.IsNullOrEmpty(x.PurchaseOrder.PurchaseOrderNumber)).WithMessage("PO number must 10 characters");

            RuleFor(x => x.PurchaseOrder.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.PurchaseOrder.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.PurchaseOrder.POValueUSD).GreaterThan(0).WithMessage("PO Value must be defined");
            PurchaseOrderValidator = purchaseOrderValidator;
            RuleFor(x => x.PurchaseOrder.PurchaseOrderName).MustAsync(ReviewNameExist).When(x => !string.IsNullOrEmpty(x.PurchaseOrder.PurchaseOrderName))
                .WithMessage(x => $"{x.PurchaseOrder.PurchaseOrderName} already exist"); ;

            RuleFor(x => x.PurchaseOrder.PurchaseOrderNumber).MustAsync(ReviewPONumberExist)
                .When(x => x.CreatePurchaseOrderNumber == false && !string.IsNullOrEmpty(x.PurchaseOrder.PurchaseOrderNumber))
                .WithMessage(x => $"{x.PurchaseOrder.PurchaseOrderNumber} already exist");
        }
        async Task<bool> ReviewPONumberExist(NewPurchaseOrderCreateSalaryRequest Purchaseorder, string ponumber, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidatePONumberExistInPurchaseOrder(Guid.Empty, ponumber);
            return !result;
        }
        async Task<bool> ReviewNameExist(NewPurchaseOrderCreateSalaryRequest Purchaseorder, string name, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidateNameExistInPurchaseOrder(Purchaseorder.PurchaseOrder.MWOId, name);
            return !result;
        }
    }
}
