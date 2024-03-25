namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates
{
    public class EditPurchaseOrderRegularCreatedRequestDto : CreatedRegularPurchaseOrderRequestDto
    {
        public EditPurchaseOrderRegularCreatedRequestDto()
        {

        }
        public void ConvertToDto(EditPurchaseOrderRegularCreatedRequest request)
        {
            this.PurchaseOrderId = request.PurchaseOrderId;
            base.ConvertToDto(request);
        }
        public Guid PurchaseOrderId { get; set; }
    }

}
