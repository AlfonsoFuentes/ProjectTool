using Client.Infrastructure.Managers.PurchaseOrders;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits;
using Shared.Models.PurchaseorderStatus;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class EditClosedRegularPurchaseOrderValidator : AbstractValidator<EditPurchaseOrderRegularClosedRequest>
    {
        private IPurchaseOrderValidator PurchaseOrderValidator { get; set; }
        public EditClosedRegularPurchaseOrderValidator(IPurchaseOrderValidator purchaseOrderValidator)
        {
            RuleFor(x => x.Supplier).NotNull().WithMessage("Supplier must be defined");
            RuleFor(X => X.PONumber).Must(x => x.StartsWith("850"))
                 .When(x => !string.IsNullOrEmpty(x.PurchaseRequisition)).WithMessage("PO must start with 850");

            RuleFor(X => X.PONumber).Length(10)
                .When(x => !string.IsNullOrEmpty(x.PurchaseRequisition)).WithMessage("PO number must 10 characters");

            RuleFor(customer => customer.PONumber).Matches("^[0-9]*$")
                 .When(x => !string.IsNullOrEmpty(x.PurchaseRequisition)).WithMessage("PO Number must be number!");

            RuleFor(X => X.ExpectedDate).NotNull().WithMessage("Expected PO must be defined");

            RuleFor(X => X.PurchaseorderName).NotEmpty().WithMessage("Purchase order name must be defined");
            RuleFor(X => X.PurchaseorderName).NotNull().WithMessage("Purchase order name must be defined");

            RuleFor(X => X.QuoteNo).NotEmpty().WithMessage("Quote name must be defined");
            RuleFor(X => X.QuoteNo).NotNull().WithMessage("Quote name must be defined");

            RuleFor(X => X.PurchaseRequisition).NotEmpty().WithMessage("PR must be defined");
            RuleFor(X => X.PurchaseRequisition).NotNull().WithMessage("PR must be defined");

            RuleFor(X => X.PurchaseRequisition).Must(x => x.StartsWith("PR")).WithMessage("PR must include PR letter at start");
            RuleFor(X => X.PurchaseRequisition).Length(8).WithMessage("PR must 8 characters");

            //RuleFor(x => x.SumPOValueUSD).GreaterThan(x => x.POValueUSDOriginal)
            //    .When(x => x.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Closed.Id)
            //    .WithMessage(x => $"PO new Value $USD{x.SumPOValueUSD} can not be less than $USD{x.POValueUSDOriginal}");

            RuleFor(x => x.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.SumPOValueUSD).GreaterThan(0).WithMessage("PO Value must be defined");

            RuleFor(x => x.IsAnyValueNotDefined).NotEqual(true).WithMessage("All Item must have Currency value greater Than zero");
            RuleFor(x => x.QuoteCurrency).Must(x => x.Id != CurrencyEnum.None.Id).WithMessage("Quote currency must be defined");
            PurchaseOrderValidator = purchaseOrderValidator;
            
            RuleFor(x => x.PurchaseorderName).MustAsync(ReviewNameExist)
                .When(x => !string.IsNullOrEmpty(x.PurchaseorderName)).WithMessage(x => $"{x.PurchaseorderName} already exist"); ;

            RuleFor(x => x.PurchaseRequisition).MustAsync(ReviewPRExist)
                .When(x => !string.IsNullOrEmpty(x.PurchaseRequisition)).WithMessage(x => $"{x.PurchaseRequisition} already exist");

            RuleFor(x => x.PONumber).MustAsync(ReviewPONumberExist)
                .When(x => !string.IsNullOrEmpty(x.PONumber)).WithMessage(x => $"{x.PONumber} already exist");

        }
        async Task<bool> ReviewNameExist(EditPurchaseOrderRegularClosedRequest Purchaseorder, string name, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidateNameExistInPurchaseOrder(Purchaseorder.PurchaseOrderId, name);
            return !result;
        }
        async Task<bool> ReviewPRExist(EditPurchaseOrderRegularClosedRequest Purchaseorder, string pr, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidatePurchaseRequisitionExistInPurchaseOrder(Purchaseorder.PurchaseOrderId, pr);
            return !result;
        }
        async Task<bool> ReviewPONumberExist(EditPurchaseOrderRegularClosedRequest Purchaseorder, string ponumber, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidatePONumberExistInPurchaseOrder(Purchaseorder.PurchaseOrderId, ponumber);
            return !result;
        }
    }

}
