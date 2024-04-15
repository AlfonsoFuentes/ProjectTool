using Shared.Models.PurchaseorderStatus;
using System.Net.Http.Headers;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class PurchaseOrderResponse
    {

        public List<PurchaseorderItemsResponse> PurchaseOrderItems { get; set; } = new();
        public Guid PurchaseOrderId { get; set; }
        public string PurchaseorderName { get; set; } = string.Empty;
        public string PurchaseRequisition { get; set; } = string.Empty;
        public string MWOName { get; set; } = string.Empty;
        public string PONumber { get; set; } = string.Empty;

        public Guid MWOId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedOn { get; set; }=string.Empty;
        public string ExpectedOn { get; set; } = string.Empty;
        public string ReceivedOn { get; set; } = string.Empty;
        public string SupplierNickName { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string QuoteNo { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public string AccountAssigment { get; set; } = string.Empty;
        public string CECName { get; set; } = string.Empty;
        public string TaxCode { get; set; } = string.Empty;
        public Guid? SupplierId { get; set; }
        public double POValueUSD => PurchaseOrderItems.Sum(x => x.ValueUSD);
        public double Assigned => Actual + Commitment;
        public double Actual => PurchaseOrderItems.Sum(x => x.ActualUSD);
        public double Commitment => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? 0 : POValueUSD - Actual- Potencial;
        public double Potencial=> PurchaseOrderItems.Sum(x => x.PotencialUSD);
        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;
        public string PurchaseOrderStatusName => PurchaseOrderStatus.Name;
        public bool IsAlteration { get; set; } = false;
        public bool IsCapitalizedSalary { get; set; } = false;
        public bool IsTaxNoProductive { get; set; } = false;
        public bool IsTaxEditable { get; set; } = false;
        public string LabelAction => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ?
          $"Edit {PurchaseRequisition}" : $"Edit {PONumber}";

        public double GetPOValueByItem(Guid BudgetItemId)
        {
            return PurchaseOrderItems.Where(x => x.BudgetItemId == BudgetItemId).Sum(x => x.ValueUSD);
        }
        public double GetActualByItem(Guid BudgetItemId)
        {
            return PurchaseOrderItems.Where(x => x.BudgetItemId == BudgetItemId).Sum(x => x.ActualUSD);
        }
        public double GetCommitmentByItem(Guid BudgetItemId)
        {
            return PurchaseOrderItems.Where(x => x.BudgetItemId == BudgetItemId).Sum(x => x.CommitmmentUSD);
        }
        public double GetPotentialByItem(Guid BudgetItemId)
        {
            return PurchaseOrderItems.Where(x => x.BudgetItemId == BudgetItemId).Sum(x => x.PotencialUSD);
        }
        public double GetAssignedByItem(Guid BudgetItemId)
        {
            return PurchaseOrderItems.Where(x => x.BudgetItemId == BudgetItemId).Sum(x => x.AssignedUSD);
        }
    }
}
