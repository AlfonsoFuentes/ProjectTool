using Shared.Models.BudgetItems;
using Shared.Models.Currencies;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;

namespace Shared.Models.PurchaseOrders.Requests.Taxes
{
    public class EditTaxPurchaseOrderRequest
    {
        public EditTaxPurchaseOrderRequest()
        {

        }
        public Guid PurchaseorderId { get; set; }
        public MWOApprovedResponse MWO { get; set; } = new();
        public BudgetItemApprovedResponse MainBudgetItem { get; set; } = new();

        public void SetMainBudgetItem(BudgetItemApprovedResponse budgetItem, MWOApprovedResponse mWO)
        {
            MWO = mWO;
            MainBudgetItem = budgetItem;
            AddBudgetItem(budgetItem);

        }
      
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public DateTime CurrencyDate {  get; set; }
        public string CurrencyDateOnly => CurrencyDate.ToShortDateString();
        public string PONumber { get; set; } = string.Empty;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.COP;
        public PurchaseOrderItemRequest PurchaseOrderItem { get; set; } = new();
        public string PurchaseorderName {  get; set; } = string.Empty;
        public List<string> ValidationErrors { get; set; } = new();


        public void ChangeName(string name)
        {
            ValidationErrors.Clear();
            PurchaseorderName = name;
            PurchaseOrderItem.Name = PurchaseorderName;

        }
        public void ChangePOnumber(string ponumber)
        {
            ValidationErrors.Clear();
            PONumber = ponumber;

        }
        public void ChangeName(PurchaseOrderItemRequest model, string name)
        {
            ValidationErrors.Clear();
            model.Name = name;
            PurchaseorderName = name;

        }


        public void AddBudgetItem(BudgetItemApprovedResponse response)
        {
            PurchaseOrderItem.SetBudgetItem(response, USDCOP, USDEUR);


        }
        public double SumPOValueUSD => PurchaseOrderItem.POItemValueUSD;
        public double SumPOValueCurrency => PurchaseOrderItem.POItemCurrencyValue;
        public double SumBudget => PurchaseOrderItem.Budget;
        public double SumBudgetAssigned => PurchaseOrderItem.BudgetAssigned;
        public double SumBudgetPotencialAssigned => PurchaseOrderItem.BudgetPotencial;
        public double SumPendingUSD => PurchaseOrderItem.POItemPendingUSD;
    }

}
