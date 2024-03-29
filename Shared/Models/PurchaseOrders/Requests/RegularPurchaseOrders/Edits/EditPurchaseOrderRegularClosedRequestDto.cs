namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits
{
    public class EditPurchaseOrderRegularClosedRequestDto : EditPurchaseOrderRegularApprovedRequestDto
    {
        public EditPurchaseOrderRegularClosedRequestDto()
        {

        }
        public void ConvertToDto(EditPurchaseOrderRegularClosedRequest request)
        {
            base.ConvertToDto(request);
        }

    }
}
