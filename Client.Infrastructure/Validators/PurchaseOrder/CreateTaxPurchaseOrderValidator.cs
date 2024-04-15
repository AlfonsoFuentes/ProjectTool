using Client.Infrastructure.Managers.PurchaseOrders;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;
using Shared.Models.PurchaseOrders.Requests.Taxes;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class CreateTaxPurchaseOrderValidator : AbstractValidator<CreateTaxPurchaseOrderRequest>
    {
        IPurchaseOrderValidator PurchaseOrderValidator { get; set; }
        public CreateTaxPurchaseOrderValidator(IPurchaseOrderValidator purchaseOrderValidator)
        {


            RuleFor(X => X.Name).NotEmpty().WithMessage("Purchase order name must be defined");

            RuleFor(X => X.PONumber).Must(x => x.StartsWith("850")).WithMessage("PO must start with 850");
            RuleFor(X => X.PONumber).Length(10).When(x => !string.IsNullOrEmpty(x.PONumber)).WithMessage("PO number must 10 characters");
            RuleFor(customer => customer.PONumber).Matches("^[0-9]*$").When(x => !string.IsNullOrEmpty(x.PONumber)).WithMessage("PO Number must be number!");

            RuleFor(x => x.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.SumPOValueUSD).GreaterThan(0).WithMessage("PO Value must be defined");
            RuleFor(x => x.Name).MustAsync(ReviewNameExist).When(x => !string.IsNullOrEmpty(x.Name)).WithMessage(x => $"{x.Name} already exist");
            RuleFor(x => x.PONumber).MustAsync(ReviewPONumberExist).When(x => !string.IsNullOrEmpty(x.PONumber)).WithMessage(x => $"{x.PONumber} already exist");

            PurchaseOrderValidator = purchaseOrderValidator;
        }
        async Task<bool> ReviewNameExist(CreateTaxPurchaseOrderRequest po, string name, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidateNameExistInPurchaseOrder(po.MWOId, name);
            return !result;
        }
        async Task<bool> ReviewPONumberExist(CreateTaxPurchaseOrderRequest Purchaseorder, string ponumber, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidatePONumberExistInPurchaseOrder(ponumber);
            return !result;
        }
    }
}
