using Shared.Enums.Currencies;

namespace Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems
{
    public class ReceivePurchaseorderItemRequest
    {
        public Guid PurchaseOrderItemId { get; set; }
        public string Name { get; set; } = string.Empty;

        public Guid BudgetItemId { get; set; } = Guid.Empty;
        public string BudgetItemName { get; set; } = string.Empty;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public double POValueUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ?
            POValueCurrency : PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ?
           TRMUSDCOP == 0 ? 0 : POValueCurrency / TRMUSDCOP : TRMUSDEUR == 0 ? 0 : POValueCurrency / TRMUSDEUR;
        public double POActualUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ?
           POActualCurrency : PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ?
          TRMUSDCOP == 0 ? 0 : POActualCurrency / TRMUSDCOP : TRMUSDEUR == 0 ? 0 : POActualCurrency / TRMUSDEUR;
        double _ReceivingCurrency;

        public double Quantity {  get; set; }
        public double UnitaryValueCurrency {  get; set; }
        public double POValueCurrency => Quantity * UnitaryValuePurchaseOrderCurrency;

        public double POActualCurrency { get; set; }

        public double POPendingUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ?
          POPendingCurrency : PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ?
         TRMUSDCOP == 0 ? 0 : POPendingCurrency/ TRMUSDCOP : TRMUSDEUR == 0 ? 0 : POPendingCurrency / TRMUSDEUR;
        public double POPendingCurrency => POValueCurrency - POActualCurrency;

       

        public double ReceivingUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ?
            ReceivingCurrency : PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ?
           TRMUSDCOP == 0 ? 0 : ReceivingCurrency / TRMUSDCOP : TRMUSDEUR == 0 ? 0 : ReceivingCurrency / TRMUSDEUR;


        public double PONewActualCurrency => POActualCurrency + ReceivingCurrency;

        public double PONewActualUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ?
            PONewActualCurrency : PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ?
           TRMUSDCOP == 0 ? 0 : PONewActualCurrency / TRMUSDCOP : TRMUSDEUR == 0 ? 0 : PONewActualCurrency / TRMUSDEUR;


        public double PONewPendingCurrency => POValueCurrency - PONewActualCurrency;
        public double PONewPendingUSD => POValueUSD - PONewActualUSD;

        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.COP;
        public double TRMUSDCOP { get;  set; } = 1;
        public double TRMUSDEUR { get;  set; } = 1;
        public double UnitaryValuePurchaseOrderCurrency =>
           PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id && QuoteCurrency.Id == CurrencyEnum.USD.Id ? UnitaryValueCurrency :
           PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id && QuoteCurrency.Id == CurrencyEnum.COP.Id ? UnitaryValueCurrency / TRMUSDCOP :
           PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id && QuoteCurrency.Id == CurrencyEnum.EUR.Id ? UnitaryValueCurrency / TRMUSDEUR :
           PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id && QuoteCurrency.Id == CurrencyEnum.USD.Id ? UnitaryValueCurrency * TRMUSDCOP :
           PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id && QuoteCurrency.Id == CurrencyEnum.COP.Id ? UnitaryValueCurrency :
           PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id && QuoteCurrency.Id == CurrencyEnum.EUR.Id ? UnitaryValueCurrency * TRMUSDCOP * TRMUSDEUR :
           PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id && QuoteCurrency.Id == CurrencyEnum.USD.Id ? UnitaryValueCurrency * TRMUSDEUR :
           PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id && QuoteCurrency.Id == CurrencyEnum.COP.Id ? UnitaryValueCurrency / TRMUSDCOP * TRMUSDEUR :
           PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id && QuoteCurrency.Id == CurrencyEnum.EUR.Id ? UnitaryValueCurrency : 0;

        public double TotalValuePurchaseOrderCurrency => Quantity * UnitaryValuePurchaseOrderCurrency;
        public void SetUSDCOP(double usdcop)
        {
            TRMUSDCOP = usdcop;


        }
        public void SetUSDEUR(double usddeur)
        {

            TRMUSDEUR = usddeur;

        }
        public double ReceivingCurrency
        {
            get { return _ReceivingCurrency; }
            set
            {
                _ReceivingCurrency = value;
                _ReceivePercentagePurchaseOrder = Math.Round(_ReceivingCurrency / POValueCurrency * 100, 2);
            }
        }
        public double MaxPercentageToReceive => POValueCurrency == 0 ? 0 : Math.Round(POPendingCurrency / POValueCurrency * 100, 2);
        double _ReceivePercentagePurchaseOrder;
        public double ReceivePercentagePurchaseOrder
        {
            get => _ReceivePercentagePurchaseOrder;
            set
            {
                _ReceivePercentagePurchaseOrder = value;
                _ReceivingCurrency = POValueCurrency * _ReceivePercentagePurchaseOrder / 100.0;
            }
        }
        public void OnChangeReceivePercentagePurchaseOrder(string percentage)
        {
            double newpercentage = ReceivePercentagePurchaseOrder;
            if (!double.TryParse(percentage, out newpercentage)) return;

            ReceivePercentagePurchaseOrder = newpercentage;

            ReceivingCurrency = POValueCurrency * ReceivePercentagePurchaseOrder / 100.0;
          

        }
        public void OnChangeReceiveCurrencyPurchaseOrder(string receivingcurrencystring)
        {
            double receivingcurrency = ReceivingCurrency;
            if (!double.TryParse(receivingcurrencystring, out receivingcurrency)) return;

            ReceivingCurrency = receivingcurrency;
            ReceivePercentagePurchaseOrder = Math.Round(ReceivingCurrency / POValueCurrency * 100, 2);



        }
    }
}
