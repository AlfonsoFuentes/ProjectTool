using Shared.Models.BudgetItems;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Requests.Create;
using Shared.Models.Suppliers;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class DataForEditPurchaseOrder
    {
        public EditPurchaseOrderCreatedRequest PurchaseOrder { get; set; } = new();
        public MWOResponse MWO { get; set; } = new();
        public BudgetItemResponse BudgetItem { get; set; } = new();
        public List<SupplierResponse> Suppliers { get; set; } = new();
        public List<BudgetItemResponse> OriginalBudgetItems { get; set; } = new();


    }
}
