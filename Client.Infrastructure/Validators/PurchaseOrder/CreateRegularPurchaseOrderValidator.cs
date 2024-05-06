using Client.Infrastructure.Managers.PurchaseOrders;
using Shared.Enums.Currencies;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class CreateRegularPurchaseOrderValidator : AbstractValidator<CreatedRegularPurchaseOrderRequest>
    {
        IPurchaseOrderValidator PurchaseOrderValidator { get; set; }
        public CreateRegularPurchaseOrderValidator(IPurchaseOrderValidator purchaseOrderValidator)
        {
            RuleFor(x => x.Supplier).NotNull().WithMessage("Supplier must be defined");


            RuleFor(X => X.PurchaseorderName).NotEmpty().WithMessage("Purchase order name must be defined");
            RuleFor(X => X.PurchaseorderName).NotNull().WithMessage("Purchase order name must be defined");

            RuleFor(X => X.QuoteNo).NotEmpty().WithMessage("Quote name must be defined");
            RuleFor(X => X.QuoteNo).NotNull().WithMessage("Quote name must be defined");

            RuleFor(X => X.PurchaseRequisition).NotEmpty().WithMessage("PR must be defined");
            RuleFor(X => X.PurchaseRequisition).NotNull().WithMessage("PR must be defined");

            RuleFor(X => X.PurchaseRequisition).Must(x => x.StartsWith("PR")).WithMessage("PR must include PR letter at start");
            RuleFor(X => X.PurchaseRequisition).Length(8).When(x => !string.IsNullOrEmpty(x.PurchaseRequisition)).WithMessage("PR must 8 characters");



            RuleFor(x => x.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.SumPOValueUSD).GreaterThan(0).WithMessage("PO Value must be defined");

            RuleFor(x => x.IsAnyValueNotDefined).NotEqual(true).WithMessage("All Item must have Currency value greater Than zero");
            RuleFor(x => x.QuoteCurrency).Must(x => x.Id != CurrencyEnum.None.Id).WithMessage("Quote currency must be defined");
            RuleFor(x => x.PurchaseorderName).MustAsync(ReviewNameExist).When(x => !string.IsNullOrEmpty(x.PurchaseorderName)).WithMessage(x => $"{x.PurchaseorderName} already exist");
            RuleFor(x => x.PurchaseRequisition).MustAsync(ReviewPRExist).When(x => !string.IsNullOrEmpty(x.PurchaseRequisition)).WithMessage(x => $"{x.PurchaseRequisition} already exist");
            PurchaseOrderValidator = purchaseOrderValidator;
        }
        async Task<bool> ReviewNameExist(CreatedRegularPurchaseOrderRequest po,string name, CancellationToken cancellationToken)
        {
       
            var result= await PurchaseOrderValidator.ValidateNameExistInPurchaseOrder(po.MWOId,name);
            return !result;
        }
        async Task<bool> ReviewPRExist(string pr, CancellationToken cancellationToken)
        {
   
            var result = await PurchaseOrderValidator.ValidatePurchaseRequisitionExistInPurchaseOrder(pr);
            return !result;
        }
    }

}
