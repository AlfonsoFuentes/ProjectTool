using Shared.Models.BudgetItems;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseorderStatus;
using Shared.Models.Suppliers;
using Shared.Models.WayToReceivePurchaseOrdersEnums;
using System.Reflection;

namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates
{
    public class ReceiveRegularPurchaseOrderRequest
    {
        public Func<Task<bool>> Validator { get; set; } = null!;
        public Guid PurchaseOrderId { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public string PurchaseorderName { get; set; } = string.Empty;
        public SupplierResponse? Supplier { get; set; }
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


        public List<ReceivePurchaseorderItemRequest> PurchaseOrderItemsToReceive { get; set; } = new();



        public double MaxPercentageToReceive => SumPOValueUSD == 0 ? 0 : Math.Round(SumPOPendingUSD / SumPOValueUSD * 100.0, 2);

        public double PercentageAlteration { get; set; }

        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;

        public WayToReceivePurchaseorderEnum WayToReceivePurchaseOrder { get; set; } = WayToReceivePurchaseorderEnum.None;

        public double PercentageToReceive { get; set; }
        public async Task OnChangeWayToReceivePurchaseOrder(WayToReceivePurchaseorderEnum wayToReceivePurchaseOrder)
        {



            if (WayToReceivePurchaseOrder.Id != wayToReceivePurchaseOrder.Id)
            {
                WayToReceivePurchaseOrder = wayToReceivePurchaseOrder;
                ClearValues();
            }
            if (WayToReceivePurchaseOrder.Id == WayToReceivePurchaseorderEnum.CompleteOrder.Id)
            {
                PercentageToReceive = Math.Round(SumPOPendingCurrency / SumPOValueCurrency * 100, 2);
                foreach (var row in PurchaseOrderItemsToReceive)
                {
                    row.ReceivingCurrency = row.POPendingCurrency;


                    row.ReceivePercentagePurchaseOrder = Math.Round(row.ReceivingCurrency / row.POValueCurrency * 100, 2);
                }
            }
            if (Validator != null) await Validator();

        }
        void ClearValues()
        {
            foreach (var row in PurchaseOrderItemsToReceive)
            {
                row.ReceivingCurrency = 0;

                row.ReceivePercentagePurchaseOrder = 0;
            }
        }
        public async Task OnChangeReceivePercentagePurchaseOrder(string percentage)
        {

            double newpercentage = PercentageToReceive;
            if (!double.TryParse(percentage, out newpercentage)) return;

            if (!(newpercentage < 0 || newpercentage > 100))
            {
                PercentageToReceive = newpercentage;
                if (newpercentage > MaxPercentageToReceive)
                {
                    PercentageToReceive = MaxPercentageToReceive;
                }

                foreach (var row in PurchaseOrderItemsToReceive)
                {
                    row.ReceivePercentagePurchaseOrder = PercentageToReceive;
                }
            }


            if (Validator != null) await Validator();
        }
        public async Task OnChangeReceivingItem(ReceivePurchaseorderItemRequest item, string receivingvalue)
        {
            double newreceivingitem = item.ReceivingCurrency;
            if (!double.TryParse(receivingvalue, out newreceivingitem)) return;
            item.ReceivingCurrency = newreceivingitem;
           
            if (Validator != null) await Validator();
        }
        public async Task OnChangePercentageReceivingItem(ReceivePurchaseorderItemRequest item, string receivingpercentage)
        {
            double newpercentage = item.ReceivePercentagePurchaseOrder;
            if (!double.TryParse(receivingpercentage, out newpercentage)) return;
            if (!(newpercentage < 0 || newpercentage > 100))
            {

                if (newpercentage > item.MaxPercentageToReceive)
                {
                    newpercentage = item.MaxPercentageToReceive;
                }
                item.ReceivePercentagePurchaseOrder = newpercentage;

            }


            if (Validator != null) await Validator();
        }
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
        public async Task ChangeTRMUSDCOP(string arg)
        {

            if (string.IsNullOrEmpty(arg))
            {
                return;
            }
            double usdcop = _usdcop;
            if (!double.TryParse(arg, out usdcop))
            {

            }
            TRMUSDCOP = usdcop;
            if (Validator != null) await Validator();
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
        public async Task ChangeTRMUSDEUR(string arg)
        {

            if (string.IsNullOrEmpty(arg))
            {
                return;
            }
            double usdeur = _usdeur;
            if (!double.TryParse(arg, out usdeur))
            {

            }
            TRMUSDEUR = usdeur;
            if (Validator != null) await Validator();
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


        public async Task SetSupplier(SupplierResponse? _Supplier)
        {

            if (_Supplier == null)
            {
                PurchaseOrderCurrency = CurrencyEnum.COP;
                return;
            }
            Supplier = _Supplier;


            if (Validator != null) await Validator();
            foreach (var row in PurchaseOrderItemsToReceive)
            {
                row.PurchaseOrderCurrency = _Supplier.SupplierCurrency;

            }

            PurchaseOrderCurrency = _Supplier.SupplierCurrency;
        }
    }
}
