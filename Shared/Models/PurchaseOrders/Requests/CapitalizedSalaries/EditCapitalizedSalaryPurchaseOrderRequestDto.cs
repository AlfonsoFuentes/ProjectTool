namespace Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries
{
    public class EditCapitalizedSalaryPurchaseOrderRequestDto : CreateCapitalizedSalaryPurchaseOrderRequestDto
    {

        public void ConvertToDto(EditCapitalizedSalaryPurchaseOrderRequest request)
        {
            this.PurchaseOrderId = request.PurchaseOrderId;
            base.ConverToDto(request);
        }
        public Guid PurchaseOrderId { get; set; }

    }
}
