using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;

namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits
{
    public class EditPurchaseOrderRegularApprovedRequestDto : ApprovedRegularPurchaseOrderRequestDto
    {
        public EditPurchaseOrderRegularApprovedRequestDto()
        {

        }

        public void ConvertToDto(EditPurchaseOrderRegularApprovedRequest request)
        {

            base.ConvertToDto(request);
        }
    }
}
