using Client.Infrastructure.Managers.PurchaseOrders;
using FluentValidation;
using Shared.Enums.Currencies;
using Shared.NewModels.PurchaseOrders.Request;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class NewPurchaseOrderCreateValidator : AbstractValidator<NewPurchaseOrderCreateRequest>
    {
        INewPurchaseOrderValidator PurchaseOrderValidator { get; set; }
        public NewPurchaseOrderCreateValidator(INewPurchaseOrderValidator purchaseOrderValidator)
        {
            RuleFor(x => x.Supplier).NotNull().When(x => !x.IsCapitalizedSalary || !x.IsTaxEditable)
                .WithMessage("Supplier must be defined");

            RuleFor(X => X.PurchaseorderName).NotEmpty().WithMessage("Purchase order name must be defined");
            RuleFor(X => X.PurchaseorderName).NotNull().WithMessage("Purchase order name must be defined");

            RuleFor(X => X.QuoteNo).NotEmpty().WithMessage("Quote name must be defined");
            RuleFor(X => X.QuoteNo).NotNull().WithMessage("Quote name must be defined");

            RuleFor(X => X.PurchaseRequisition).NotEmpty().WithMessage("PR must be defined");
            RuleFor(X => X.PurchaseRequisition).NotNull().WithMessage("PR must be defined");

            RuleFor(X => X.PurchaseRequisition).Must(x => x.StartsWith("PR")).When(x=> !string.IsNullOrEmpty(x.PurchaseRequisition)).WithMessage("PR must include PR letter at start");
            RuleFor(X => X.PurchaseRequisition).Length(8).When(x => !string.IsNullOrEmpty(x.PurchaseRequisition)).WithMessage("PR must 8 characters");

            //Revisar cuando sea salarios capitalizables
            //RuleFor(x => x.PurchaseOrderNumber).NotNull().When(x => !x.IsCapitalizedSalary || !x.IsTaxEditable)
            //    .WithMessage("PO must be defined");

            //RuleFor(X => X.PurchaseOrderNumber).Must(x => x.StartsWith("850"))
            //   .When(x => x.IsCapitalizedSalary == false && !string.IsNullOrEmpty(x.PurchaseOrderNumber)).WithMessage("PO must start with 850");
            //RuleFor(X => X.PurchaseOrderNumber).Length(10)
            //    .When(x => x.IsCapitalizedSalary == false && !string.IsNullOrEmpty(x.PurchaseOrderNumber)).WithMessage("PO number must 10 characters");

            //RuleFor(customer => customer.PurchaseOrderNumber).Matches("^[0-9]*$").When(x => !string.IsNullOrEmpty(x.PONumber)).WithMessage("PO Number must be number!");


            RuleFor(x => x.USDEUR).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.USDCOP).GreaterThan(0).WithMessage("TRM must be defined");
            RuleFor(x => x.IsAnyValueNotDefined).NotEqual(true).WithMessage("All purchase orders items Currency value greater Than zero");
            RuleFor(x => x.IsAnyNameEmpty).NotEqual(true).WithMessage("All purchase orders items must have a Name");
            RuleFor(x => x.QuoteCurrency).Must(x => x.Id != CurrencyEnum.None.Id).WithMessage("Quote currency must be defined");
            RuleFor(x => x.PurchaseorderName).MustAsync(ReviewNameExist).When(x => !string.IsNullOrEmpty(x.PurchaseorderName)).WithMessage(x => $"{x.PurchaseorderName} already exist");
            RuleFor(x => x.PurchaseRequisition).MustAsync(ReviewPRExist).When(x => !string.IsNullOrEmpty(x.PurchaseRequisition)).WithMessage(x => $"{x.PurchaseRequisition} already exist");
            PurchaseOrderValidator = purchaseOrderValidator;
        }
        async Task<bool> ReviewNameExist(NewPurchaseOrderCreateRequest po, string name, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidateNameExistInPurchaseOrder(po.MWOId, name);
            return !result;
        }
        async Task<bool> ReviewPRExist(string pr, CancellationToken cancellationToken)
        {

            var result = await PurchaseOrderValidator.ValidatePurchaseRequisitionExistInPurchaseOrder(pr);
            return !result;
        }
    }

}
