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
            AssignedCurrency = _BudgetItem.Assigned;
            PotencialCurrency = _BudgetItem.Potencial;
            TRMUSDCOP = usdcop;
            TRMUSDEUR = usdeur;
        }
       
        public double Budget { get; set; }
        public double AssignedCurrency {  get; set; }
        public double PotencialCurrency {  get; set; }
        public double ActualCurrency {  get; set; }
        public double AssignedUSD => Currency.Id == CurrencyEnum.USD.Id ? AssignedCurrency :
            Currency.Id == CurrencyEnum.COP.Id ? AssignedCurrency / TRMUSDCOP :
            AssignedCurrency / TRMUSDEUR;
        public double PotencialUSD => Currency.Id == CurrencyEnum.USD.Id ? PotencialCurrency :
            Currency.Id == CurrencyEnum.COP.Id ? PotencialCurrency / TRMUSDCOP :
            PotencialCurrency / TRMUSDEUR;
        public double ActualUSD => Currency.Id == CurrencyEnum.USD.Id ? ActualCurrency :
            Currency.Id == CurrencyEnum.COP.Id ? ActualCurrency / TRMUSDCOP :
            ActualCurrency / TRMUSDEUR;
        public Guid PurchaseOrderItemId { get; set; }
        public string Name { get; set; } = string.Empty;

        public Guid BudgetItemId { get; set; } = Guid.Empty;
        public string BudgetItemName { get; set; } = string.Empty;
        public double Quantity { get; set; } = 1;
       

        public double POItemPendingUSD => POValueUSD - ActualUSD- PotencialUSD;
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
        public CurrencyEnum Currency
        {
            get { return _QuoteCurrency; }
            set { _QuoteCurrency = value; }
        }

        public double TotalCurrencyValue => Quantity * CurrencyUnitaryValue;
        public double CurrencyUnitaryValue { get; set; }


        public double POValueUSD => Quantity * UnitaryCostInUSD;
        public double UnitaryCostInUSD => Currency.Id == CurrencyEnum.USD.Id ?
            CurrencyUnitaryValue : Currency.Id == CurrencyEnum.COP.Id ?
           TRMUSDCOP == 0 ? 0 : CurrencyUnitaryValue / TRMUSDCOP : TRMUSDEUR == 0 ? 0 : CurrencyUnitaryValue / TRMUSDEUR;
        public double TRMUSDCOP { get; set; } = 1;
        public double TRMUSDEUR { get; set; } = 1;
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
