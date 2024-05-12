namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class NewPurchaseOrderEditCreateValidator : AbstractValidator<NewPurchaseOrderEditCreateRequest>
    {
        private INewPurchaseOrderValidator PurchaseOrderValidator { get; set; }
        public NewPurchaseOrderEditCreateValidator(INewPurchaseOrderValidator purchaseOrderValidator)
        {
            RuleFor(x => x.PurchaseOrder.Supplier).NotNull().WithMessage("Supplier must be defined");


            RuleFor(X => X.PurchaseOrder.PurchaseOrderName).NotEmpty().WithMessage("Purchase order name must be defined");
            RuleFor(X => X.PurchaseOrder.PurchaseOrderName).NotNull().WithMessage("Purchase order name must be defined");

            RuleFor(X => X.PurchaseOrder.QuoteNo).NotEmpty().WithMessage("Quote name must be defined");
            RuleFor(X => X.PurchaseOrder.QuoteNo).NotNull().WithMessage("Quote name must be defined");

            RuleFor(X => X.PurchaseOrder.PurchaseRequisition).NotEmpty().WithMessage("PR must be defined");
            RuleFor(X => X.PurchaseOrder.PurchaseRequisition).NotNull().WithMessage("PR must be defined");

            RuleFor(X => X.PurchaseOrder.PurchaseRequisition)
                 .Must(x => x.StartsWith("PR"))
                 .When(x => !string.IsNullOrEmpty(x.PurchaseOrder.PurchaseRequisition))
                 .WithMessage("PR must include PR letter at start");

            RuleFor(X => X.PurchaseOrder.PurchaseRequisition)
               .Length(8)
               .When(x => !string.IsNullOrEmpty(x.PurchaseOrder.PurchaseRequisition))
               .When(x => x.PurchaseOrder.PurchaseRequisition.StartsWith("PR"))
               .WithMessage("PR must 8 characters");



            RuleFor(x => x.PurchaseOrder.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.PurchaseOrder.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");

            RuleFor(x => x.PurchaseOrder.POValueUSD).GreaterThan(0).WithMessage("PO Value must be defined");

            RuleFor(x => x.PurchaseOrder.IsAnyValueNotDefined).NotEqual(true).WithMessage("All Item must have Currency value greater Than zero");
            RuleFor(x => x.PurchaseOrder.IsAnyNameEmpty).NotEqual(true).WithMessage("All purchase orders items must have a Name");

            RuleFor(x => x.PurchaseOrder.QuoteCurrency).Must(x => x.Id != CurrencyEnum.None.Id).WithMessage("Quote currency must be defined");

            RuleFor(x => x.PurchaseOrder.PurchaseOrderName).MustAsync(ReviewNameExist)
                .When(x => !string.IsNullOrEmpty(x.PurchaseOrder.PurchaseOrderName)).WithMessage(x => $"{x.PurchaseOrder.PurchaseOrderName} already exist"); ;

            RuleFor(x => x.PurchaseOrder.PurchaseRequisition).MustAsync(ReviewPRExist)
                .When(x => !string.IsNullOrEmpty(x.PurchaseOrder.PurchaseRequisition)).WithMessage(x => $"{x.PurchaseOrder.PurchaseRequisition} already exist");


            PurchaseOrderValidator = purchaseOrderValidator;
        }
        async Task<bool> ReviewNameExist(NewPurchaseOrderEditCreateRequest Purchaseorder, string name, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidateNameExistInPurchaseOrder(Purchaseorder.PurchaseOrder.PurchaseOrderId, name);
            return !result;
        }
        async Task<bool> ReviewPRExist(NewPurchaseOrderEditCreateRequest Purchaseorder, string pr, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidatePurchaseRequisitionExistInPurchaseOrder(Purchaseorder.PurchaseOrder.PurchaseOrderId, pr);
            return !result;
        }
    }

}
