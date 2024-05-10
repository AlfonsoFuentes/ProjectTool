namespace Shared.NewModels.PurchaseOrders.Request
{
    public class NewPurchaseOrderReceiveItemActualRequest
    {
        public string POItemName { get; set; } = string.Empty;
        public Guid ReceivedId { get; set; }
        public double ReceivedCurrency { get; set; }

        public DateTime CurrencyDate { get; set; }
        public string CurrencyDatestring=>CurrencyDate.ToString("d");
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public double ReceivedUSD =>
          PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? ReceivedCurrency :
          PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? USDCOP == 0 ? 0 : ReceivedCurrency / USDCOP :
          PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? USDEUR == 0 ? 0 : ReceivedCurrency / USDEUR :
          0;


    }
}
