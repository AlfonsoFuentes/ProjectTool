namespace Shared.NewModels.PurchaseOrders.Base
{
    public class NewPurchaseOrderItemReceivedRequest
    {
        public Guid PurchaseOrderItemReceivedId { get; set; }
        public string BudgetItemNomclatoreName { get; set; } = string.Empty;
        public double ReceivingValueCurrency { get; set; }
        public double ValueReceivedCurrency { get; set; }
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public DateTime CurrencyDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public double ValueReceivedUSD =>
           PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? ValueReceivedCurrency :
           PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? USDCOP == 0 ? 0 : ValueReceivedCurrency / USDCOP :
           PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? USDEUR == 0 ? 0 : ValueReceivedCurrency / USDEUR :
             0;
        public double ReceivingValueUSD =>
          PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? ReceivingValueCurrency :
          PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? USDCOP == 0 ? 0 : ReceivingValueCurrency / USDCOP :
          PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? USDEUR == 0 ? 0 : ReceivingValueCurrency / USDEUR :
            0;
    }
}
