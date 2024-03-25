using Shared.Models.Currencies;

namespace Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems
{
    public class ReceivePurchaseorderItemRequest
    {
        public Guid PurchaseOrderItemId { get; set; }
        public string Name { get; set; } = string.Empty;

        public Guid BudgetItemId { get; set; } = Guid.Empty;
        public string BudgetItemName { get; set; } = string.Empty;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public double POValueUSD { get; set; } //read from Database
        public double POActualUSD { get; set; } //read from Database
        double _ReceivingCurrency;


        public double POValueCurrency => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ?
            POValueUSD : PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ?
           TRMUSDCOP == 0 ? 0 : POValueUSD * TRMUSDCOP : TRMUSDEUR == 0 ? 0 : POValueUSD * TRMUSDEUR;

        public double POActualCurrency => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ?
           POActualUSD : PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ?
          TRMUSDCOP == 0 ? 0 : POActualUSD * TRMUSDCOP : TRMUSDEUR == 0 ? 0 : POActualUSD * TRMUSDEUR;

        public double POPendingUSD => POValueUSD - POActualUSD;//read from Database
        public double POPendingCurrency => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ?
          POPendingUSD : PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ?
         TRMUSDCOP == 0 ? 0 : POPendingUSD * TRMUSDCOP : TRMUSDEUR == 0 ? 0 : POPendingUSD * TRMUSDEUR;


        public double ReceivingUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ?
            ReceivingCurrency : PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ?
           TRMUSDCOP == 0 ? 0 : ReceivingCurrency / TRMUSDCOP : TRMUSDEUR == 0 ? 0 : ReceivingCurrency / TRMUSDEUR;


        public double PONewActualCurrency => POActualCurrency + ReceivingCurrency;

        public double PONewActualUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ?
            PONewActualCurrency : PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ?
           TRMUSDCOP == 0 ? 0 : PONewActualCurrency / TRMUSDCOP : TRMUSDEUR == 0 ? 0 : PONewActualCurrency / TRMUSDEUR;


        public double PONewPendingCurrency => POValueCurrency - PONewActualCurrency;
        public double PONewPendingUSD => POValueUSD - PONewActualUSD;


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
