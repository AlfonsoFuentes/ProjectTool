﻿namespace Shared.NewModels.PurchaseOrders.Responses
{
    public class NewPriorPurchaseOrderReceivedResponse
    {


        public Guid PurchaseOrderItemReceivedId { get; set; }
        public Guid PurchaseOrderItemId { get; set; }
        public double ValueReceivedCurrency { get; set; }
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public DateTime CurrencyDate { get; set; }
     
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;

        public double ValueReceivedUSD =>
           PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? ValueReceivedCurrency :
           PurchaseOrderCurrency.Id ==  CurrencyEnum.COP.Id ? USDCOP == 0 ? 0 : ValueReceivedCurrency / USDCOP :
           PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? USDEUR == 0 ? 0 : ValueReceivedCurrency / USDEUR :
             0;
    }
}
