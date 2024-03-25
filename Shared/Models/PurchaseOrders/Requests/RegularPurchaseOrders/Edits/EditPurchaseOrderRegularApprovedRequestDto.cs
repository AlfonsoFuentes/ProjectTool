namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates
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
