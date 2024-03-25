using Shared.Models.BudgetItems;
using Shared.Models.Currencies;

namespace Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems
{
    public class PurchaseOrderItemRequest
    {
        public PurchaseOrderItemRequest() { }
        public void SetBudgetItem(BudgetItemApprovedResponse _BudgetItem, double usdcop, double usdeur)
        {
            BudgetItemName = _BudgetItem.NomenclatoreName;
            Name = _BudgetItem.Name;
            BudgetItemId = _BudgetItem.BudgetItemId;
            Budget = _BudgetItem.Budget;
            BudgetAssigned = _BudgetItem.Assigned;
            BudgetPotencial = _BudgetItem.Potencial;
            TRMUSDCOP = usdcop;
            TRMUSDEUR = usdeur;
        }
        public double Budget {  get; set; }
        public double BudgetAssigned {  get; set; }
        public double BudgetPotencial {  get; set; }
        public Guid PurchaseOrderItemId { get; set; }
        public string Name { get; set; } = string.Empty;
      
        public Guid BudgetItemId { get; set; } = Guid.Empty;
        public string BudgetItemName { get; set; } = string.Empty;
        public double Quantity { get; set; } = 1;
        public double POItemActualUSD { get; set; }
       
        public double POItemPendingUSD => POItemValueUSD - POItemActualUSD;
        public void ChangeQuantity(string arg)
        {

            if (string.IsNullOrEmpty(arg))
            {
                return;
            }
            double quantity = Quantity;
            if (!double.TryParse(arg, out quantity))
            {

            }
            Quantity = quantity;
        }
        public void ChangeCurrency(CurrencyEnum newCurrency)
        {
            double originalValueInUsd = UnitaryCostInUSD;
            if (newCurrency.Id == CurrencyEnum.COP.Id)
            {
                CurrencyUnitaryValue = originalValueInUsd * TRMUSDCOP;
            }
            else if (newCurrency.Id == CurrencyEnum.EUR.Id)
            {
                CurrencyUnitaryValue = originalValueInUsd * TRMUSDEUR;
            }
            else if (newCurrency.Id == CurrencyEnum.USD.Id)
            {
                CurrencyUnitaryValue = originalValueInUsd;
            }
            _QuoteCurrency = newCurrency;

        }
        public void ChangeCurrencyValue(string arg)
        {

            if (string.IsNullOrEmpty(arg))
            {
                return;
            }
            double currencyvalue = Quantity;
            if (!double.TryParse(arg, out currencyvalue))
            {

            }
            CurrencyUnitaryValue = currencyvalue;
        }
        CurrencyEnum _QuoteCurrency = CurrencyEnum.COP;
        public CurrencyEnum QuoteCurrency
        {
            get { return _QuoteCurrency; }
            set { _QuoteCurrency = value; }
        }

        public double POItemCurrencyValue => Quantity * CurrencyUnitaryValue;
        public double CurrencyUnitaryValue { get; set; }


        public double POItemValueUSD => Quantity * UnitaryCostInUSD;
        public double UnitaryCostInUSD => QuoteCurrency.Id == CurrencyEnum.USD.Id ?
            CurrencyUnitaryValue : QuoteCurrency.Id == CurrencyEnum.COP.Id ?
           TRMUSDCOP == 0 ? 0 : CurrencyUnitaryValue / TRMUSDCOP : TRMUSDEUR == 0 ? 0 : CurrencyUnitaryValue / TRMUSDEUR;
        public double TRMUSDCOP { get;  set; } = 1;
        public double TRMUSDEUR { get;  set; } = 1;
        public void SetUSDCOP(double usdcop)
        {
            TRMUSDCOP = usdcop;


        }
        public void SetUSDEUR(double usddeur)
        {

            TRMUSDEUR = usddeur;

        }
    }

}
