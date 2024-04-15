using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.Currencies;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries
{
    public class CreateCapitalizedSalaryPurchaseOrderRequest
    {

       
        public bool IsCapitalizedSalary { get; set; } = true;
        public string PurchaseOrderName { get; set; } = string.Empty;
        public string PurchaseorderNumber { get; set; } = string.Empty;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public DateTime CurrencyDate { get; set; }
        public string CurrencyDateOnly => CurrencyDate.ToShortDateString();
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.COP;
        public PurchaseOrderItemRequest PurchaseOrderItem { get; set; } = new();
       
        public Guid MWOId { get; set; }
        public string MWOName { get; set; } = string.Empty;
        public string AccountAssignment { get; set; } = string.Empty;
        public string CostCenter { get; set; } = string.Empty;
        public string MWOCECName { get; set; } = string.Empty;

        public Guid MainBudgetItemId => MainBudgetItem.BudgetItemId;
        public BudgetItemApprovedResponse MainBudgetItem { get; set; } = new();
        public void SetMainBudgetItem(BudgetItemApprovedResponse budgetItem)
        {
            MWOId = budgetItem.MWOId;
            MWOName = budgetItem.MWOName;
            CostCenter = budgetItem.CostCenter;
            MWOCECName = budgetItem.MWOCECName;
            AccountAssignment = budgetItem.MWOCECName;
            MainBudgetItem = budgetItem;
            AddBudgetItem(budgetItem);
            PurchaseOrderCurrency = CurrencyEnum.USD;
        }

        public void AddBudgetItem(BudgetItemApprovedResponse response)
        {
            PurchaseOrderItem.SetBudgetItem(response, USDCOP, USDEUR);
            PurchaseOrderItem.Currency = CurrencyEnum.USD;



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
