namespace Client.Infrastructure.Validators.PurchaseOrder.News
{
    public class NewPurchaseOrderReceiveValidator : AbstractValidator<NewPurchaseOrderReceiveRequest>
    {
        public NewPurchaseOrderReceiveValidator()
        {

            RuleFor(x => x.PurchaseOrder.IsAnyValueNotWellReceived).NotEqual(true).WithMessage("Pending cannot be negative");
            RuleFor(x => x.PurchaseOrder.POCommitmentReceivingCurrency).GreaterThanOrEqualTo(0).WithMessage("Pending cannot be negative");
        }
    }
}
