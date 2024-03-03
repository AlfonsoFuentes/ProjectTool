using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.Currencies;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Requests.Create;
using System.Xml.Linq;

namespace Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries
{
    public class CreateCapitalizedSalaryPurchaseOrderRequest
    {
        public List<string> ValidationErrors { get; set; } = new();

        public Guid MWOId { get; set; }
        public bool IsCapitalizedSalary { get; set; } = true;
             
        public Guid MainBudgetItemId { get; set; }
        public string PurchaseOrderName { get; set; } = string.Empty;
        public string PurchaseorderNumber {  get; set; }=string.Empty;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public bool AssetRealProductive { get; set; }
        public string AccountAssigment { get; set; } = string.Empty;
        public string MWOCECName { get; set; } = string.Empty;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.USD;
        public CreatePurchaseOrderItemRequest PurchaseOrderItem { get; set; } = new();
        public void ChangeName(string name)
        {
            ValidationErrors.Clear();
            PurchaseOrderName = name;
            PurchaseOrderItem.Name = name;

        }
        public void ChangePurchaseorderNumber(string ponumber)
        {
            ValidationErrors.Clear();
            PurchaseorderNumber = ponumber;
            

        }
       
        public void SetMWOBudgetItem(MWOResponse mWO, BudgetItemApprovedResponse budgetItem)
        {
            MWOId = mWO.Id;
            MWOCECName = mWO.CECName;
            AddBudgetItem(budgetItem);
            PurchaseOrderCurrency = CurrencyEnum.USD;
        }

        public void AddBudgetItem(BudgetItemApprovedResponse response)
        {
            PurchaseOrderItem.SetBudgetItem(response, USDCOP, USDEUR);
            PurchaseOrderItem.QuoteCurrency = CurrencyEnum.USD;
          
            

        }
        public double SumPOValueUSD => PurchaseOrderItem.TotalValueUSDItem;
        public double SumPOValueCurrency => PurchaseOrderItem.TotalCurrencyValue;
        public double SumBudget => PurchaseOrderItem.Budget;
        public double SumBudgetAssigned => PurchaseOrderItem.BudgetAssigned;
        public double SumBudgetPotencialAssigned => PurchaseOrderItem.BudgetPotencialAssigned;
        public double SumPendingUSD => PurchaseOrderItem.Pending;

        
    }
}
