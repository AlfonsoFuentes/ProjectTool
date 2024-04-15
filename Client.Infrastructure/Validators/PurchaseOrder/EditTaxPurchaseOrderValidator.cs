using Client.Infrastructure.Managers.PurchaseOrders;
using Shared.Models.PurchaseOrders.Requests.Taxes;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class EditTaxPurchaseOrderValidator : AbstractValidator<EditTaxPurchaseOrderRequest>
    {
        IPurchaseOrderValidator PurchaseOrderValidator { get; set; }
        public EditTaxPurchaseOrderValidator(IPurchaseOrderValidator purchaseOrderValidator)
        {


            RuleFor(X => X.PurchaseorderName).NotEmpty().WithMessage("Purchase order name must be defined");

            RuleFor(X => X.PONumber).Must(x => x.StartsWith("850")).WithMessage("PO must start with 850");
            RuleFor(X => X.PONumber).Length(10).When(x => !string.IsNullOrEmpty(x.PONumber)).WithMessage("PO number must 10 characters");
            RuleFor(customer => customer.PONumber).Matches("^[0-9]*$").When(x => !string.IsNullOrEmpty(x.PONumber)).WithMessage("PO Number must be number!");


            RuleFor(x => x.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.SumPOValueUSD).GreaterThan(0).WithMessage("PO Value must be defined");
            RuleFor(x => x.PurchaseorderName).MustAsync(ReviewNameExist).When(x => !string.IsNullOrEmpty(x.PurchaseorderName)).WithMessage(x => $"{x.PurchaseorderName} already exist");
            RuleFor(x => x.PONumber).MustAsync(ReviewPONumberExist).When(x => !string.IsNullOrEmpty(x.PONumber)).WithMessage(x => $"{x.PONumber} already exist");

            PurchaseOrderValidator = purchaseOrderValidator;
        }
        async Task<bool> ReviewNameExist(EditTaxPurchaseOrderRequest po, string name, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidateNameExistInPurchaseOrder(po.MWO.Id, po.PurchaseorderId, name);
            return !result;
        }
        async Task<bool> ReviewPONumberExist(EditTaxPurchaseOrderRequest Purchaseorder, string ponumber, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidatePONumberExistInPurchaseOrder(Purchaseorder.PurchaseorderId, ponumber);
            return !result;
        }
    }
}
