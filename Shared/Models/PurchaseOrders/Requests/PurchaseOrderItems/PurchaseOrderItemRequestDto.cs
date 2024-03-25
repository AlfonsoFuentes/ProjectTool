namespace Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems
{
    public class PurchaseOrderItemRequestDto
    {
        public PurchaseOrderItemRequestDto() { }

        public void ConvertToDto(PurchaseOrderItemRequest request)
        {
            PurchaseOrderItemId = request.PurchaseOrderItemId;
            PurchaseorderItemName = request.Name;
            BudgetItemId = request.BudgetItemId;
            BudgetItemName = request.BudgetItemName;
            Quantity = request.Quantity;
            POValueUSD = request.POItemValueUSD;
        }


        public Guid PurchaseOrderItemId { get; set; }
        public string PurchaseorderItemName { get; set; } = string.Empty;

        public Guid BudgetItemId { get; set; } = Guid.Empty;
 
        public string BudgetItemName {  get; set; } = string.Empty;
        public double Quantity { get; set; } = 1;
        public double POValueUSD { get; set; } = 1;


    }

}
