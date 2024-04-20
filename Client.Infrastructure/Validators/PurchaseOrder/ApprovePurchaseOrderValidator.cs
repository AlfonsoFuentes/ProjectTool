using Client.Infrastructure.Managers.PurchaseOrders;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class ApprovePurchaseOrderValidator : AbstractValidator<ApprovedRegularPurchaseOrderRequest>
    {
        private IPurchaseOrderValidator PurchaseOrderValidator { get; set; }
        public ApprovePurchaseOrderValidator(IPurchaseOrderValidator purchaseOrderValidator)
        {
           
            RuleFor(X => X.PONumber).Must(x => x.StartsWith("850")).WithMessage("PO must start with 850");
            RuleFor(X => X.PONumber).Length(10).When(x => !string.IsNullOrEmpty(x.PONumber)).WithMessage("PO number must 10 characters");
            RuleFor(customer => customer.PONumber).Matches("^[0-9]*$").When(x => !string.IsNullOrEmpty(x.PONumber)).WithMessage("PO Number must be number!");
            RuleFor(X => X.ExpectedDate).NotNull().WithMessage("Expected PO must be defined");
            RuleFor(x => x.QuoteCurrency).Must(x => x.Id != CurrencyEnum.None.Id).WithMessage("Quote currency must be defined");
            RuleFor(x => x.PONumber).MustAsync(ReviewPONumberExist).When(x => !string.IsNullOrEmpty(x.PONumber)).WithMessage(x => $"{x.PONumber} already exist");
            RuleFor(x => x.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            PurchaseOrderValidator = purchaseOrderValidator;
        }
        async Task<bool> ReviewPONumberExist(ApprovedRegularPurchaseOrderRequest Purchaseorder, string ponumber, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidatePONumberExistInPurchaseOrder(Purchaseorder.PurchaseOrderId, ponumber);
            return !result;
        }
    }
}
