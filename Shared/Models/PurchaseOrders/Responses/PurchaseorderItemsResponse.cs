using Shared.Models.Currencies;
using Shared.Models.PurchaseorderStatus;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class PurchaseorderItemsResponse
    {
        public Guid BudgetItemId { get; set; }
        public Guid PurchaseorderItemId { get; set; }
        public double CurrencyValue => UnitaryValueCurrency * Quantity;
        public double ValueUSD => Currency.Id == CurrencyEnum.USD.Id ? CurrencyValue :
            Currency.Id == CurrencyEnum.COP.Id ? CurrencyValue / USDCOP : CurrencyValue / USDEUR;
        public double ActualCurrency { get; set; }
        public double ActualUSD => Currency.Id == CurrencyEnum.USD.Id ? ActualCurrency :
            Currency.Id == CurrencyEnum.COP.Id ? ActualCurrency / USDCOP : ActualCurrency / USDEUR;
        public double CommitmmentUSD => ValueUSD - ActualUSD- PotencialUSD;
        public double AssignedUSD => ActualUSD + CommitmmentUSD + PotencialUSD;
        public double PotencialUSD => Currency.Id == CurrencyEnum.USD.Id ? CurrencyPotencial :
            Currency.Id == CurrencyEnum.COP.Id ? CurrencyPotencial / USDCOP : CurrencyPotencial / USDEUR;
        public double CurrencyPotencial => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? CurrencyValue : 0;
       
       
        public double USDCOP {  get; set; }
        public double USDEUR {  get; set; }
        public CurrencyEnum Currency { get; set; } = CurrencyEnum.None;
        public double UnitaryValueCurrency {  get; set; }
        public double Quantity {  get; set; }
        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;

    }
}
