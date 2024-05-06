using Shared.Enums.Currencies;
using Shared.Enums.PurchaseorderStatus;
using Shared.Enums.WayToReceivePurchaseOrdersEnums;
using Shared.Models.BudgetItems;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.NewModels.Suppliers.Reponses;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates
{
    public class ReceiveRegularPurchaseOrderRequest
    {
      
       
        public Guid PurchaseOrderId { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public string PurchaseorderName { get; set; } = string.Empty;
        public NewSupplierResponse? Supplier { get; set; }
        public string SupplierName => Supplier == null ? string.Empty : Supplier.NickName;
        public string VendorCode => Supplier == null ? string.Empty : Supplier.VendorCode;
        public string TaxCode => Supplier == null ? string.Empty : IsAlteration || IsMWONoProductive ? Supplier.TaxCodeLP : Supplier.TaxCodeLD;
        public bool IsAssetProductive { get; set; }
        public bool IsMWONoProductive => !IsAssetProductive;
        public bool IsAlteration => MainBudgetItem.IsAlteration;
        public BudgetItemApprovedResponse MainBudgetItem { get; set; } = new();
        public Guid MainBudgetItemId => MainBudgetItem.BudgetItemId;
        public string SPL => IsAlteration ? "0735015000" : "151605000";

        public string QuoteNo { get; set; } = string.Empty;
        public string PurchaseRequisition { get; set; } = string.Empty;
        public Guid MWOId { get; set; }
        public string MWOName { get; set; } = string.Empty;
        public string AccountAssignment => IsAlteration ? CostCenter : MWOCECName;
        public string CostCenter { get; set; } = string.Empty;
        public string MWOCECName { get; set; } = string.Empty;

        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.COP;

        public List<ReceivePurchaseorderItemRequest> PurchaseOrderItemsToReceive { get; set; } = new();



        public double MaxPercentageToReceive => SumPOValueUSD == 0 ? 0 : Math.Round(SumPOPendingUSD / SumPOValueUSD * 100.0, 2);

        public double PercentageAlteration { get; set; }

        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;

        public WayToReceivePurchaseorderEnum WayToReceivePurchaseOrder { get; set; } = WayToReceivePurchaseorderEnum.None;

        public double PercentageToReceive { get; set; }
       
       
        
        double _usdcop;
        public double TRMUSDCOP
        {
            get => _usdcop;
            set
            {
                _usdcop = value;
                foreach (var item in PurchaseOrderItemsToReceive)
                {
                    item.SetUSDCOP(_usdcop);
                }
            }
        }
  
        double _usdeur;
        public double OldTRMUSDCOP { get; set; }
        public double OldTRMUSDEUR { get; set; }
        public DateTime OldCurrencyDate { get; set; }

        public DateTime CurrencyDate { get; set; }
        public string CurrencyDateOnly => CurrencyDate.ToShortDateString();
        public double TRMUSDEUR
        {
            get => _usdeur;
            set
            {
                _usdeur = value;
                foreach (var item in PurchaseOrderItemsToReceive)
                {
                    item.SetUSDEUR(_usdeur);
                }
            }
        }
       
        public WayToReceivePurchaseorderEnum WayToReceivePurchaseorderEnum { get; set; } = WayToReceivePurchaseorderEnum.None;

        public double SumPOValueUSD => PurchaseOrderItemsToReceive.Count == 0 ? 0 : PurchaseOrderItemsToReceive.Sum(x => x.POValueUSD);

        public double SumPOValueCurrency => PurchaseOrderItemsToReceive.Count == 0 ? 0 : PurchaseOrderItemsToReceive.Sum(x => x.POValueCurrency);
        public double SumPOActualUSD => PurchaseOrderItemsToReceive.Count == 0 ? 0 : PurchaseOrderItemsToReceive.Sum(x => x.POActualUSD);
        public double SumPOActualCurrency => PurchaseOrderItemsToReceive.Count == 0 ? 0 : PurchaseOrderItemsToReceive.Sum(x => x.POActualCurrency);
        public double SumReceivingCurrency => PurchaseOrderItemsToReceive.Count == 0 ? 0 : PurchaseOrderItemsToReceive.Sum(x => x.ReceivingCurrency);
        public double SumReceivingUSD => PurchaseOrderItemsToReceive.Count == 0 ? 0 : PurchaseOrderItemsToReceive.Sum(x => x.ReceivingUSD);
        public double SumPONewActualCurrency => PurchaseOrderItemsToReceive.Count == 0 ? 0 : PurchaseOrderItemsToReceive.Sum(x => x.PONewActualCurrency);
        public double SumPONewActualUSD => PurchaseOrderItemsToReceive.Count == 0 ? 0 : PurchaseOrderItemsToReceive.Sum(x => x.PONewActualUSD);
        public double SumPOPendingCurrency => PurchaseOrderItemsToReceive.Count == 0 ? 0 : PurchaseOrderItemsToReceive.Sum(x => x.POPendingCurrency);
        public double SumPOPendingUSD => PurchaseOrderItemsToReceive.Count == 0 ? 0 : PurchaseOrderItemsToReceive.Sum(x => x.POPendingUSD);
        public double SumPONewPendingCurrency => PurchaseOrderItemsToReceive.Count == 0 ? 0 : PurchaseOrderItemsToReceive.Sum(x => x.PONewPendingCurrency);
        public double SumPONewPendingUSD => PurchaseOrderItemsToReceive.Count == 0 ? 0 : PurchaseOrderItemsToReceive.Sum(x => x.PONewPendingUSD);


    }
}
