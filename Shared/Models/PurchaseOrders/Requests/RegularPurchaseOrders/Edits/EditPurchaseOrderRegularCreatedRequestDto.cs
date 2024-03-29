using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;

namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits
{
    public class EditPurchaseOrderRegularCreatedRequestDto : CreatedRegularPurchaseOrderRequestDto
    {
        public EditPurchaseOrderRegularCreatedRequestDto()
        {

        }
        public void ConvertToDto(EditPurchaseOrderRegularCreatedRequest request)
        {
            PurchaseOrderId = request.PurchaseOrderId;
            base.ConvertToDto(request);
        }
        public Guid PurchaseOrderId { get; set; }
    }

}
