using Client.Infrastructure.Managers.PurchaseOrders;
using Shared.NewModels.PurchaseOrders.Request;

namespace Client.Infrastructure.Validators.PurchaseOrder
{
    public class NewPurchaseOrderEditReceiveValidator : AbstractValidator<NewPurchaseOrderEditReceiveRequest>
    {
        private INewPurchaseOrderValidator PurchaseOrderValidator { get; set; }
        public NewPurchaseOrderEditReceiveValidator(INewPurchaseOrderValidator purchaseOrderValidator)
        {
           
            
            

           

            RuleFor(x => x.IsAnyValueNotDefined).NotEqual(true).WithMessage("All Item received must have Currency value greater Than zero");
            
            PurchaseOrderValidator = purchaseOrderValidator;

           
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
