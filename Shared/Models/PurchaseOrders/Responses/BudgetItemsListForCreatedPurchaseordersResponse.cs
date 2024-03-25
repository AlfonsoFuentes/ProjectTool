using Shared.Models.BudgetItems;
using Shared.Models.MWO;
using Shared.Models.Suppliers;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class BudgetItemsListForPurchaseordersResponse
    {
        
        public List<BudgetItemApprovedResponse> BudgetItems { get; set; } = new();

    }
}
