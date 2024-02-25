using Shared.Models.Currencies;
using Shared.Models.PurchaseorderStatus;
using Shared.Models.WayToReceivePurchaseOrdersEnums;

namespace Shared.Models.PurchaseOrders.Requests.Receives
{
    public class ReceivePurchaseOrderRequest
    {
        public List<string> ValidationErrors { get; set; } = new();
        public List<ReceivePurchaseorderItemRequest> ItemsInPurchaseorder { get; set; } = new();
        public Guid PurchaseorderId { get; set; }
        public string PurchaseorderName { get; set; } = string.Empty;

        public decimal MaxPercentageToReceive =>Convert.ToDecimal(100.0 - SumOriginalPendingUSD/SumPOValueUSD * 100.0);
        public string PurchaseRequisition { get; set; } = string.Empty;
        public string MWOName { get; set; } = string.Empty;
        public string PONumber { get; set; } = string.Empty;
        public bool IsNoAssetProductive { get; set; }
        public bool IsAlteration { get; set; }
        public double PercentageAlteration {  get; set; }
        public void ChangePONumber(string ponumber)
        {
            ValidationErrors.Clear();
            PONumber = ponumber.Trim();


        }
        public Guid MWOId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? ExpetedOn { get; set; } = DateTime.UtcNow;
        public string Supplier { get; set; } = string.Empty;
        public string QuoteNo { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public string AccountAssigment { get; set; } = string.Empty;
        public string MWOCode { get; set; } = string.Empty;
        public string TaxCode { get; set; } = string.Empty;
        public Guid? SupplierId { get; set; }
        public double PurchaseOrderValue { get; set; }

        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;

        public WayToReceivePurchaseorderEnum WayToReceivePurchaseOrder { get; set; } = WayToReceivePurchaseorderEnum.None;

        public double PercentageToReceive { get; set; }
        public void OnChangeReceivePercentagePurchaseOrder(string percentage)
        {
            double newpercentage = PercentageToReceive;
            if (!double.TryParse(percentage, out newpercentage)) return;

            PercentageToReceive = newpercentage;
            foreach (var row in ItemsInPurchaseorder)
            {
                row.ReceivingCurrency = row.POValueCurrency * PercentageToReceive / 100.0;
                row.ActualUSD = row.OriginalActualUSD + row.ReceivingUSD;
                row.PendingUSD = row.OriginalPendingUSD - row.ReceivingUSD;
                row.ReceivePercentagePurchaseOrder = PercentageToReceive;
            }
        }





        public double POValueCurrency => Currency.Id == CurrencyEnum.USD.Id ?
            SumPOValueUSD : Currency.Id == CurrencyEnum.COP.Id ?
           USDCOP == 0 ? 0 : SumPOValueUSD * USDCOP : USDEUR == 0 ? 0 : SumPOValueUSD * USDEUR;
        public double ActualCurrency => Currency.Id == CurrencyEnum.USD.Id ?
            SumActualUSD : Currency.Id == CurrencyEnum.COP.Id ?
           USDCOP == 0 ? 0 : SumActualUSD * USDCOP : USDEUR == 0 ? 0 : SumActualUSD * USDEUR;
        public double PendingCurrency => Currency.Id == CurrencyEnum.USD.Id ?
            SumPendingUSD : Currency.Id == CurrencyEnum.COP.Id ?
           USDCOP == 0 ? 0 : SumPendingUSD * USDCOP : USDEUR == 0 ? 0 : SumPendingUSD * USDEUR;

        public double ReceivingCurrency => Currency.Id == CurrencyEnum.USD.Id ?
         SumReceivingUSD : Currency.Id == CurrencyEnum.COP.Id ?
        USDCOP == 0 ? 0 : SumReceivingUSD * USDCOP : USDEUR == 0 ? 0 : SumReceivingUSD * USDEUR;

        public double USDCOP { get; set; }
        public double USDEUR { get; set; }

        public CurrencyEnum Currency { get; set; } = CurrencyEnum.None;
        public double SumPOValueUSD => ItemsInPurchaseorder.Count == 0 ? 0 : ItemsInPurchaseorder.Sum(x => x.POValueUSD);
        public double SumPOValueCurrency => ItemsInPurchaseorder.Count == 0 ? 0 : ItemsInPurchaseorder.Sum(x => x.POValueCurrency);
        public double SumOriginalPendingUSD => ItemsInPurchaseorder.Count == 0 ? 0 : ItemsInPurchaseorder.Sum(x => x.OriginalPendingUSD);
        public double SumActualUSD => ItemsInPurchaseorder.Count == 0 ? 0 : ItemsInPurchaseorder.Sum(x => x.ActualUSD);
        public double SumPendingUSD => ItemsInPurchaseorder.Count == 0 ? 0 :Math.Round( ItemsInPurchaseorder.Sum(x => x.PendingUSD));
        public double SumReceivingUSD => ItemsInPurchaseorder.Count == 0 ? 0 : ItemsInPurchaseorder.Sum(x => x.ReceivingUSD);
    }
}
