using Shared.Models.Currencies;
using Shared.Models.PurchaseorderStatus;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class PurchaseorderItemsResponse
    {
        public Guid BudgetItemId { get; set; }
        public Guid PurchaseorderItemId { get; set; }
        public double QuoteCurrencyValue => QuoteUnitaryValueCurrency * Quantity;
        public double ValueUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? TotalValuePurchaseOrderCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? TotalValuePurchaseOrderCurrency / USDCOP : TotalValuePurchaseOrderCurrency / USDEUR;
        public double ActualCurrency { get; set; }
        public double ActualUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? ActualCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? ActualCurrency / USDCOP : ActualCurrency / USDEUR;
        public double CommitmmentUSD => ValueUSD - ActualUSD- PotencialUSD;
        public double AssignedUSD => ActualUSD + CommitmmentUSD + PotencialUSD;
        public double PotencialUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? CurrencyPotencial :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? CurrencyPotencial / USDCOP : CurrencyPotencial / USDEUR;
        public double CurrencyPotencial => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? TotalValuePurchaseOrderCurrency : 0;
       
       
        public double USDCOP {  get; set; }
        public double USDEUR {  get; set; }
        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public double QuoteUnitaryValueCurrency {  get; set; }
        public double Quantity {  get; set; }
        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;
        public double UnitaryValuePurchaseOrderCurrency =>
           PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id && QuoteCurrency.Id == CurrencyEnum.USD.Id ? QuoteUnitaryValueCurrency :
           PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id && QuoteCurrency.Id == CurrencyEnum.COP.Id ? QuoteUnitaryValueCurrency / USDCOP :
           PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id && QuoteCurrency.Id == CurrencyEnum.EUR.Id ? QuoteUnitaryValueCurrency / USDEUR :
           PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id && QuoteCurrency.Id == CurrencyEnum.USD.Id ? QuoteUnitaryValueCurrency * USDCOP :
           PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id && QuoteCurrency.Id == CurrencyEnum.COP.Id ? QuoteUnitaryValueCurrency :
           PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id && QuoteCurrency.Id == CurrencyEnum.EUR.Id ? QuoteUnitaryValueCurrency * USDCOP * USDEUR :
           PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id && QuoteCurrency.Id == CurrencyEnum.USD.Id ? QuoteUnitaryValueCurrency * USDEUR :
           PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id && QuoteCurrency.Id == CurrencyEnum.COP.Id ? QuoteUnitaryValueCurrency / USDCOP * USDEUR :
           PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id && QuoteCurrency.Id == CurrencyEnum.EUR.Id ? QuoteUnitaryValueCurrency : 0;

        public double TotalValuePurchaseOrderCurrency => Quantity * UnitaryValuePurchaseOrderCurrency;
    }
}
