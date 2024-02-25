using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.Currencies;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Requests.Create;

namespace Shared.Models.PurchaseOrders.Requests.Taxes
{
    public class CreateTaxPurchaseOrderRequest
    {
        public CreateTaxPurchaseOrderRequest()
        {

        }


        public Guid MainBudgetItemId { get; set; }


        public string Name { get; set; } = string.Empty;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public string MWOCECName { get; set; } = string.Empty;

        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.COP;
        public Guid MWOId { get; set; }

        public CreatePurchaseOrderItemRequest PurchaseOrderItem { get; set; } = new();

        public List<string> ValidationErrors { get; set; } = new();


        public void ChangeName(string name)
        {
            ValidationErrors.Clear();
            Name = name;
            PurchaseOrderItem.Name = Name;

        }
        public void ChangePOnumber(string ponumber)
        {
            ValidationErrors.Clear();
            PONumber = ponumber;

        }
        public void ChangeName(CreatePurchaseOrderItemRequest model, string name)
        {
            ValidationErrors.Clear();
            model.Name = name;
            Name = name;

        }
        public void SetMWOBudgetItem(MWOResponse mWO, BudgetItemResponse budgetItem)
        {
            MWOId = mWO.Id;
            MWOCECName = mWO.CECName;
            AddBudgetItem(budgetItem);
        }

        public void AddBudgetItem(BudgetItemResponse response)
        {
            PurchaseOrderItem.SetBudgetItem(response, USDCOP, USDEUR);


        }
        public double SumPOValueUSD => PurchaseOrderItem.TotalValueUSDItem;
        public double SumPOValueCurrency => PurchaseOrderItem.TotalCurrencyValue;
        public double SumBudget => PurchaseOrderItem.Budget;
        public double SumBudgetAssigned => PurchaseOrderItem.BudgetAssigned;
        public double SumBudgetPotencialAssigned => PurchaseOrderItem.BudgetPotencialAssigned;
        public double SumPendingUSD => PurchaseOrderItem.Pending;
    }

}
