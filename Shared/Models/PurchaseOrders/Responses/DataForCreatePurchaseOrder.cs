using Shared.Models.BudgetItems;
using Shared.Models.MWO;
using Shared.Models.Suppliers;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class DataForCreatePurchaseOrder
    {
        public MWOResponse MWO { get; set; } = new();
        public BudgetItemResponse BudgetItem { get; set; } = new();
        public List<SupplierResponse> Suppliers { get; set; } = new();
        public List<BudgetItemResponse> OriginalBudgetItems { get; set; } = new();
        public List<BudgetItemResponse> BudgetItems { get; set; } = new();

    }
}
