namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates
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
