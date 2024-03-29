using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits;

namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates
{
    public class ApprovedRegularPurchaseOrderRequestDto: EditPurchaseOrderRegularCreatedRequestDto
    {
        public DateTime? ExpectedDate { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public void ConvertToDto(ApprovedRegularPurchaseOrderRequest request)
        {
            this.PONumber = request.PONumber;
            this.ExpectedDate = request.ExpectedDate;
            base.ConvertToDto(request);
        }

    }
}
