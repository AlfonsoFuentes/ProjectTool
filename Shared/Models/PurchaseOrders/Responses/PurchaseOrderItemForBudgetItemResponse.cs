using Shared.Models.PurchaseorderStatus;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class PurchaseOrderItemForBudgetItemResponse
    {
        public double POValueUSD { get; set; }
        public Guid BudgetItemId { get; set; }
        public Guid PurchaseorderItemId { get; set; }
        public string PurchaseorderName { get; set; } = string.Empty;
        public string PurchaseorderNumber { get;set; }= string.Empty;
        public double Actual {  get; set; }
        public double Pending => POValueUSD - Actual;
        public string Supplier {  get; set; }=string.Empty;

        public PurchaseOrderStatusEnum PurchaseOrderStatus {  get; set; }=PurchaseOrderStatusEnum.None;


    }
}
