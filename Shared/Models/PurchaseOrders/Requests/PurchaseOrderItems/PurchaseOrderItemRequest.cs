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
            OriginalAssignedCurrency = AssignedCurrency;
            OriginalPotencialCurrency = PotencialCurrency;

        }
        public double OriginalAssignedCurrency { get; set; }
        public double OriginalPotencialCurrency { get; set; }
        public double Budget { get; set; }
        public double AssignedCurrency { get; set; }
        public double PotencialCurrency { get; set; }
        public double ActualCurrency { get; set; }
        public double AssignedUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? AssignedCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? AssignedCurrency / TRMUSDCOP :
            AssignedCurrency / TRMUSDEUR;
        public double PotencialUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? PotencialCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? PotencialCurrency / TRMUSDCOP :
            PotencialCurrency / TRMUSDEUR;
        public double ActualUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? ActualCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? ActualCurrency / TRMUSDCOP :
            ActualCurrency / TRMUSDEUR;
        public Guid PurchaseOrderItemId { get; set; }
        public string Name { get; set; } = string.Empty;

        public Guid BudgetItemId { get; set; } = Guid.Empty;
        public string BudgetItemName { get; set; } = string.Empty;
        public double Quantity { get; set; } = 1;


        public double POItemPendingUSD => Budget - AssignedUSD - PotencialUSD;

        public void ChangeCurrency(CurrencyEnum newCurrency)
        {
            double originalValueInUsd = UnitaryValueUSD;
            if (newCurrency.Id == CurrencyEnum.COP.Id)
            {
                QuoteCurrencyValue = originalValueInUsd * TRMUSDCOP;
            }
            else if (newCurrency.Id == CurrencyEnum.EUR.Id)
            {
                QuoteCurrencyValue = originalValueInUsd * TRMUSDEUR;
            }
            else if (newCurrency.Id == CurrencyEnum.USD.Id)
            {
                QuoteCurrencyValue = originalValueInUsd;
            }
            QuoteCurrency = newCurrency;

        }


        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.COP;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.COP;


        public double POValueUSD => Quantity * UnitaryValueUSD;
        public double UnitaryValueUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ?
            UnitaryValuePurchaseOrderCurrency : PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ?
           TRMUSDCOP == 0 ? 0 : UnitaryValuePurchaseOrderCurrency / TRMUSDCOP : TRMUSDEUR == 0 ? 0 : UnitaryValuePurchaseOrderCurrency / TRMUSDEUR;
        public double TRMUSDCOP { get; set; } = 1;
        public double TRMUSDEUR { get; set; } = 1;

        double _QuoteCurrencyValue;
        public double QuoteCurrencyValue
        {
            get
            {
                return _QuoteCurrencyValue;
            }
            set
            {
                _QuoteCurrencyValue = value;
               
            }
        }

        public double UnitaryValuePurchaseOrderCurrency =>
            PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id && QuoteCurrency.Id == CurrencyEnum.USD.Id ? QuoteCurrencyValue :
            PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id && QuoteCurrency.Id == CurrencyEnum.COP.Id ? QuoteCurrencyValue / TRMUSDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id && QuoteCurrency.Id == CurrencyEnum.EUR.Id ? QuoteCurrencyValue / TRMUSDEUR :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id && QuoteCurrency.Id == CurrencyEnum.USD.Id ? QuoteCurrencyValue * TRMUSDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id && QuoteCurrency.Id == CurrencyEnum.COP.Id ? QuoteCurrencyValue :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id && QuoteCurrency.Id == CurrencyEnum.EUR.Id ? QuoteCurrencyValue * TRMUSDCOP * TRMUSDEUR :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id && QuoteCurrency.Id == CurrencyEnum.USD.Id ? QuoteCurrencyValue * TRMUSDEUR :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id && QuoteCurrency.Id == CurrencyEnum.COP.Id ? QuoteCurrencyValue / TRMUSDCOP * TRMUSDEUR :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id && QuoteCurrency.Id == CurrencyEnum.EUR.Id ? QuoteCurrencyValue : 0;

        public double TotalValuePurchaseOrderCurrency => Quantity * UnitaryValuePurchaseOrderCurrency;

    }

}
