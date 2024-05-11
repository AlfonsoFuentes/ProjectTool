using Client.Infrastructure.Managers.PurchaseOrders;
using Shared.NewModels.PurchaseOrders.Request;

namespace Client.Infrastructure.Validators.PurchaseOrder.News
{
    public class NewPurchaseOrderEditReceiveValidator : AbstractValidator<NewPurchaseOrderEditReceiveRequest>
    {
        private INewPurchaseOrderValidator PurchaseOrderValidator { get; set; }
        public NewPurchaseOrderEditReceiveValidator(INewPurchaseOrderValidator purchaseOrderValidator)
        {

            RuleFor(x => x.PurchaseOrder.IsAnyValueNotWellReceived).NotEqual(true).WithMessage("All Item received must have Currency value greater Than zero");

            PurchaseOrderValidator = purchaseOrderValidator;


        }
       
    }

}
