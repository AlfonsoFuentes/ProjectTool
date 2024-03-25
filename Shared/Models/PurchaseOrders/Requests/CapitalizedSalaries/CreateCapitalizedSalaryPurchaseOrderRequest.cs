using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.Currencies;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using System.Xml.Linq;

namespace Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries
{
    public class CreateCapitalizedSalaryPurchaseOrderRequest
    {

        public Func<Task<bool>> Validator { get; set; } = null!;
        public bool IsCapitalizedSalary { get; set; } = true;


        public string PurchaseOrderName { get; set; } = string.Empty;
        public string PurchaseorderNumber { get; set; } = string.Empty;
        public double TRMUSDCOP { get; set; }
        public double TRMUSDEUR { get; set; }
        public DateTime CurrencyDate { get; set; }
        public string CurrencyDateOnly => CurrencyDate.ToShortDateString();
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.COP;
        public PurchaseOrderItemRequest PurchaseOrderItem { get; set; } = new();
        public async Task ChangeName(string name)
        {
          
            PurchaseOrderName = name;
            PurchaseOrderItem.Name = name;
            if (Validator != null) await Validator();
        }
        public async Task ChangePurchaseorderNumber(string ponumber)
        {
           
            PurchaseorderNumber = ponumber;
            if (Validator != null) await Validator();

        }
        public Guid MWOId { get; set; }
        public string MWOName { get; set; } = string.Empty;
        public string AccountAssignment { get; set; } = string.Empty;
        public string CostCenter { get; set; } = string.Empty;
        public string MWOCECName { get; set; } = string.Empty;


        public BudgetItemApprovedResponse MainBudgetItem { get; set; } = new();
        public void SetMainBudgetItem(BudgetItemApprovedResponse budgetItem)
        {
            MWOId = budgetItem.MWOId;
            MWOName=budgetItem.MWOName;
            CostCenter = budgetItem.CostCenter;
            MWOCECName = budgetItem.MWOCECName;
            AccountAssignment = budgetItem.MWOCECName;
            MainBudgetItem = budgetItem;
            AddBudgetItem(budgetItem);
            PurchaseOrderCurrency = CurrencyEnum.USD;
        }

        public void AddBudgetItem(BudgetItemApprovedResponse response)
        {
            PurchaseOrderItem.SetBudgetItem(response, TRMUSDCOP, TRMUSDEUR);
            PurchaseOrderItem.QuoteCurrency = CurrencyEnum.USD;



        }
        public double SumPOValueUSD => PurchaseOrderItem.POItemValueUSD;
        public double SumPOValueCurrency => PurchaseOrderItem.POItemCurrencyValue;
        public double SumBudget => PurchaseOrderItem.Budget;
        public double SumBudgetAssigned => PurchaseOrderItem.BudgetAssigned;
        public double SumBudgetPotencialAssigned => PurchaseOrderItem.BudgetPotencial;
        public double SumPendingUSD => PurchaseOrderItem.POItemPendingUSD;
        public double SumActualUSD => PurchaseOrderItem.POItemActualUSD;
        public async Task ChangeCurrencyValue(PurchaseOrderItemRequest item, string arg)
        {

            if (string.IsNullOrEmpty(arg))
            {
                return;
            }
            double currencyvalue = item.Quantity;
            if (!double.TryParse(arg, out currencyvalue))
            {

            }
            item.CurrencyUnitaryValue = currencyvalue;
            item.POItemActualUSD = currencyvalue;
      
            if (Validator != null) await Validator();
        }

    }
}
