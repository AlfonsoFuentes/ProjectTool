using Shared.Models.Currencies;

namespace Shared.Models.PurchaseOrders.Requests.Receives
{
    public class ReceivePurchaseorderItemRequest
    {
        public Guid PurchaseOrderItemId { get; set; }
        public double POValueUSD { get; set; }
        public double POValueCurrency => Currency.Id == CurrencyEnum.USD.Id ?
           POValueUSD : Currency.Id == CurrencyEnum.COP.Id ?
          USDCOP == 0 ? 0 : POValueUSD * USDCOP : USDEUR == 0 ? 0 : POValueUSD * USDEUR;
        public Guid BudgetItemId { get; set; }

        public string PurchaseOrderItemName { get; set; } = string.Empty;
        public double ReceivingUSD => Currency.Id == CurrencyEnum.USD.Id ?
            ReceivingCurrency : Currency.Id == CurrencyEnum.COP.Id ?
           USDCOP == 0 ? 0 : ReceivingCurrency / USDCOP : USDEUR == 0 ? 0 : ReceivingCurrency / USDEUR;
        public double ActualUSD { get; set; }
        public double PendingUSD { get; set; }
        public double OriginalActualUSD { get; set; }
        public double OriginalPendingUSD { get; set; }
        public double OriginalPendingCurrency => Currency.Id == CurrencyEnum.USD.Id ?
            OriginalPendingUSD : Currency.Id == CurrencyEnum.COP.Id ?
           USDCOP == 0 ? 0 : OriginalPendingUSD * USDCOP : USDEUR == 0 ? 0 : OriginalPendingUSD * USDEUR;
        public double ActualCurrency => Currency.Id == CurrencyEnum.USD.Id ?
            ActualUSD : Currency.Id == CurrencyEnum.COP.Id ?
           USDCOP == 0 ? 0 : ActualUSD * USDCOP : USDEUR == 0 ? 0 : ActualUSD * USDEUR;
        public double PendingCurrency => Currency.Id == CurrencyEnum.USD.Id ?
            PendingUSD : Currency.Id == CurrencyEnum.COP.Id ?
           USDCOP == 0 ? 0 : PendingUSD * USDCOP : USDEUR == 0 ? 0 : PendingUSD * USDEUR;
        public double ReceivingCurrency { get; set; }
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }

        public CurrencyEnum Currency { get; set; } = CurrencyEnum.None;

        public double ReceivePercentagePurchaseOrder { get; set; }
        public void OnChangeReceivePercentagePurchaseOrder(string percentage)
        {
            double newpercentage = ReceivePercentagePurchaseOrder;
            if (!double.TryParse(percentage, out newpercentage)) return;

            ReceivePercentagePurchaseOrder = newpercentage;

            ReceivingCurrency = POValueCurrency * ReceivePercentagePurchaseOrder / 100.0;
            ActualUSD = Math.Round(OriginalActualUSD + ReceivingUSD, 2);
            PendingUSD = Math.Round(OriginalPendingUSD - ReceivingUSD, 2);
        }
        public void OnChangeReceiveCurrencyPurchaseOrder(string receivingcurrencystring)
        {
            double receivingcurrency = ReceivingCurrency;
            if (!double.TryParse(receivingcurrencystring, out receivingcurrency)) return;

            ReceivingCurrency = receivingcurrency;
            ReceivePercentagePurchaseOrder = Math.Round(ReceivingCurrency / POValueCurrency * 100, 2);

            ActualUSD = Math.Round(OriginalActualUSD + ReceivingUSD, 2);
            PendingUSD = Math.Round(OriginalPendingUSD - ReceivingUSD, 2);
        }
    }
}
