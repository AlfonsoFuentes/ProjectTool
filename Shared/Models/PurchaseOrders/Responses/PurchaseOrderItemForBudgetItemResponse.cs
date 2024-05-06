using Shared.Enums.PurchaseorderStatus;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class PurchaseOrderItemForBudgetItemResponse
    {
        public double POValueUSD { get; set; }

        public Guid BudgetItemId {  get; set; }
       
        public bool IsAlteration { get; set; } = false;
        public bool IsCapitalizedSalary { get; set; } = false;
        public bool IsTaxNoProductive { get; set; } = false;
        public bool IsTaxEditable { get; set; } = false;
        public Guid PurchaseorderItemId { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public string PurchaseorderName { get; set; } = string.Empty;
        public string PurchaseorderNumber { get; set; } = string.Empty;
        public string PurchaseRequisition { get; set; } = string.Empty;
        public double Actual { get; set; }
        public double Pending => POValueUSD - Actual;
        public double Potencial => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? POValueUSD : 0;
        public string Supplier { get; set; } = string.Empty;

        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;


        public string LabelAction => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ?
            $"Edit {PurchaseRequisition}" : $"Edit {PurchaseorderNumber}";
    }
   
}
