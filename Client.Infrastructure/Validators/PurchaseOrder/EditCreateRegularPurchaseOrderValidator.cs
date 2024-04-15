using Client.Infrastructure.Managers.PurchaseOrders;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class EditCreateRegularPurchaseOrderValidator : AbstractValidator<EditPurchaseOrderRegularCreatedRequest>
    {
        private IPurchaseOrderValidator PurchaseOrderValidator { get; set; }
        public EditCreateRegularPurchaseOrderValidator(IPurchaseOrderValidator purchaseOrderValidator)
        {
            RuleFor(x => x.Supplier).NotNull().WithMessage("Supplier must be defined");


            RuleFor(X => X.PurchaseorderName).NotEmpty().WithMessage("Purchase order name must be defined");
            RuleFor(X => X.PurchaseorderName).NotNull().WithMessage("Purchase order name must be defined");

            RuleFor(X => X.QuoteNo).NotEmpty().WithMessage("Quote name must be defined");
            RuleFor(X => X.QuoteNo).NotNull().WithMessage("Quote name must be defined");

            RuleFor(X => X.PurchaseRequisition).NotEmpty().WithMessage("PR must be defined");
            RuleFor(X => X.PurchaseRequisition).NotNull().WithMessage("PR must be defined");

            RuleFor(X => X.PurchaseRequisition).Must(x => x.StartsWith("PR"))
                 .When(x => !string.IsNullOrEmpty(x.PurchaseRequisition)).WithMessage("PR must include PR letter at start");

            RuleFor(X => X.PurchaseRequisition).Length(8)
                 .When(x => !string.IsNullOrEmpty(x.PurchaseRequisition)).WithMessage("PR must 8 characters");



            RuleFor(x => x.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.SumPOValueUSD).GreaterThan(0).WithMessage("PO Value must be defined");

            RuleFor(x => x.IsAnyValueNotDefined).NotEqual(true).WithMessage("All Item must have Currency value greater Than zero");
            RuleFor(x => x.QuoteCurrency).Must(x => x.Id != CurrencyEnum.None.Id).WithMessage("Quote currency must be defined");

            RuleFor(x => x.PurchaseorderName).MustAsync(ReviewNameExist)
                .When(x => !string.IsNullOrEmpty(x.PurchaseorderName)).WithMessage(x => $"{x.PurchaseorderName} already exist"); ;

            RuleFor(x => x.PurchaseRequisition).MustAsync(ReviewPRExist)
                .When(x => !string.IsNullOrEmpty(x.PurchaseRequisition)).WithMessage(x => $"{x.PurchaseRequisition} already exist");


            PurchaseOrderValidator = purchaseOrderValidator;
        }
        async Task<bool> ReviewNameExist(EditPurchaseOrderRegularCreatedRequest Purchaseorder, string name, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidateNameExistInPurchaseOrder(Purchaseorder.PurchaseOrderId, name);
            return !result;
        }
        async Task<bool> ReviewPRExist(EditPurchaseOrderRegularCreatedRequest Purchaseorder, string pr, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidatePurchaseRequisitionExistInPurchaseOrder(Purchaseorder.PurchaseOrderId, pr);
            return !result;
        }
    }

}
