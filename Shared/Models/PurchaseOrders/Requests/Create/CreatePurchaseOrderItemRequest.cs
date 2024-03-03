using Shared.Models.BudgetItems;
using Shared.Models.Currencies;

namespace Shared.Models.PurchaseOrders.Requests.Create
{
    public class CreatePurchaseOrderItemRequest
    {
        public CreatePurchaseOrderItemRequest() { }
        public void SetBudgetItem(BudgetItemApprovedResponse _BudgetItem, double usdcop, double usdeur)
        {
            BudgetItemName = _BudgetItem.NomenclatoreName;
            Name = _BudgetItem.Name;
            BudgetItemId = _BudgetItem.Id;
            Budget = _BudgetItem.Budget;
            BudgetAssigned = _BudgetItem.Assigned;
            BudgetPotencialAssigned = _BudgetItem.PotencialAssigned;
            USDCOP = usdcop;
            USDEUR = usdeur;
        }
     

        public string Name { get; set; } = string.Empty;
        public double Budget { get; set; }
        public double BudgetAssigned { get; set; }
        public double BudgetPotencialAssigned { get; set; }
        public double Pending => Budget - BudgetAssigned - TotalValueUSDItem - BudgetPotencialAssigned;
        public Guid BudgetItemId { get; set; } = Guid.Empty;
        public string BudgetItemName { get; set; } = string.Empty;
        public double Quantity { get; set; } = 1;
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

        CurrencyEnum _QuoteCurrency = CurrencyEnum.COP;
        public CurrencyEnum QuoteCurrency
        {
            get { return _QuoteCurrency; }
            set { _QuoteCurrency = value; }
        }
        public void ChangeCurrency(CurrencyEnum newCurrency)
        {
            double originalValueInUsd = UnitaryCostInUSD;
            if (newCurrency.Id == CurrencyEnum.COP.Id)
            {
                CurrencyValue = originalValueInUsd * USDCOP;
            }
            else if (newCurrency.Id == CurrencyEnum.EUR.Id)
            {
                CurrencyValue = originalValueInUsd * USDEUR;
            }
            else if (newCurrency.Id == CurrencyEnum.USD.Id)
            {
                CurrencyValue = originalValueInUsd;
            }
            _QuoteCurrency = newCurrency;

        }
        public double TotalCurrencyValue => Quantity * CurrencyValue;
        public double CurrencyValue { get; set; }
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
            CurrencyValue = currencyvalue;
        }
     
        public double TotalValueUSDItem => Quantity * UnitaryCostInUSD;
        public double UnitaryCostInUSD => QuoteCurrency.Id == CurrencyEnum.USD.Id ?
            CurrencyValue : QuoteCurrency.Id == CurrencyEnum.COP.Id ?
           USDCOP == 0 ? 0 : CurrencyValue / USDCOP : USDEUR == 0 ? 0 : CurrencyValue / USDEUR;
        public double USDCOP { get; set; } = 1;
        public double USDEUR { get; set; } = 1;

    }

}
