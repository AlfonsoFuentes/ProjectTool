using Client.Infrastructure.Managers.PurchaseOrders;
using Shared.NewModels.PurchaseOrders.Request;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class NewPurchaseOrderCreateSalaryValidator : AbstractValidator<NewPurchaseOrderCreateSalaryRequest>
    {
        private INewPurchaseOrderValidator PurchaseOrderValidator { get; set; }
        public NewPurchaseOrderCreateSalaryValidator(INewPurchaseOrderValidator purchaseOrderValidator)
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
            PurchaseOrderValidator = purchaseOrderValidator;
            RuleFor(x => x.PurchaseorderName).MustAsync(ReviewNameExist).When(x => !string.IsNullOrEmpty(x.PurchaseorderName))
                .WithMessage(x => $"{x.PurchaseorderName} already exist"); ;

            RuleFor(x => x.PurchaseOrderNumber).MustAsync(ReviewPONumberExist)
                .When(x => x.CreatePurchaseOrderNumber == false && !string.IsNullOrEmpty(x.PurchaseOrderNumber)).WithMessage(x => $"{x.PurchaseOrderNumber} already exist");
        }
        async Task<bool> ReviewPONumberExist(NewPurchaseOrderCreateSalaryRequest Purchaseorder, string ponumber, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidatePONumberExistInPurchaseOrder(Guid.Empty, ponumber);
            return !result;
        }
        async Task<bool> ReviewNameExist(NewPurchaseOrderCreateSalaryRequest Purchaseorder, string name, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidateNameExistInPurchaseOrder(Purchaseorder.MWOId, name);
            return !result;
        }
    }
}
