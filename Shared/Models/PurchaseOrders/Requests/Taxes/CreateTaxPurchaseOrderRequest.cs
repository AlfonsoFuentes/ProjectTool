using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.Currencies;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;

namespace Shared.Models.PurchaseOrders.Requests.Taxes
{
    public class CreateTaxPurchaseOrderRequest
    {
        public CreateTaxPurchaseOrderRequest()
        {

        }

        public BudgetItemApprovedResponse MainBudgetItem { get; set; } = new();

        public void SetMainBudgetItem(BudgetItemApprovedResponse budgetItem)
        {
            MWOId=budgetItem.MWOId;
            MWOName=budgetItem.MWOName;
            MWOCECName = budgetItem.MWOCECName;
            CostCenter = budgetItem.CostCenter;
            MainBudgetItem = budgetItem;
            AddBudgetItem(budgetItem);

        }
        public string Name { get; set; } = string.Empty;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public DateTime CurrencyDate { get; set; } = DateTime.UtcNow;
        public string CurrencyDateOnly => CurrencyDate.ToShortDateString();
        public string PONumber { get; set; } = string.Empty;
        public Guid MWOId { get; set; }
        public string MWOName { get; set; } = string.Empty;

        public string CostCenter { get; set; } = string.Empty;
        public string MWOCECName { get; set; } = string.Empty;

        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.COP;


        public PurchaseOrderItemRequest PurchaseOrderItem { get; set; } = new();

       
        public void AddBudgetItem(BudgetItemApprovedResponse response)
        {
            PurchaseOrderItem.SetBudgetItem(response, USDCOP, USDEUR);


        }
        public double SumPOValueUSD => PurchaseOrderItem.POValueUSD;
        public double SumPOValueCurrency => PurchaseOrderItem.TotalCurrencyValue;
        public double SumBudget => PurchaseOrderItem.Budget;
        public double SumBudgetAssigned => PurchaseOrderItem.AssignedUSD;
        public double SumBudgetPotencialAssigned => PurchaseOrderItem.PotencialUSD;
        public double SumPendingUSD => PurchaseOrderItem.POItemPendingUSD;
        public double SumActualUSD => PurchaseOrderItem.ActualUSD;
    }

}
