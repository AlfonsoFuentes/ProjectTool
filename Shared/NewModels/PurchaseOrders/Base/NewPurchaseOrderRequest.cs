using Shared.Enums.CostCenter;
using Shared.Enums.MWOStatus;
using Shared.Enums.PurchaseorderStatus;
using System.Reflection;

namespace Shared.NewModels.PurchaseOrders.Base
{
    public class NewPurchaseOrderRequest
    {
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public Guid PurchaseOrderId { get; set; }
        public Guid MWOId => (MainBudgetItem == null) ? Guid.Empty : MainBudgetItem.MWOId;
        public string MWOName => (MainBudgetItem == null) ? string.Empty : MainBudgetItem.Name;
        public string CECName => (MainBudgetItem == null) ? string.Empty : MainBudgetItem.MWOCECName;
        public CostCenterEnum CostCenter => MainBudgetItem == null ? CostCenterEnum.None : MainBudgetItem.MWOCostCenter;
        public string PurchaseOrderName { get; set; } = string.Empty;
        public MWOTypeEnum Type => MainBudgetItem == null ? MWOTypeEnum.None : MainBudgetItem.MWOType;
        public FocusEnum Focus => MainBudgetItem == null ? FocusEnum.None : MainBudgetItem.MWOFocus;
        public MWOStatusEnum MWOStatus => MainBudgetItem == null ? MWOStatusEnum.None : MainBudgetItem.MWOStatus;
        public bool IsAssetProductive => MainBudgetItem == null ? false : MainBudgetItem.MWOIsAssetProductive;

        public Guid MainBudgetItemId => MainBudgetItem == null ? Guid.Empty : MainBudgetItem.BudgetItemId;
        public NewBudgetItemMWOApprovedResponse MainBudgetItem { get; set; } = null!;
        public Guid? SupplierId => Supplier == null ? Guid.Empty : Supplier.SupplierId;
        public NewSupplierResponse? Supplier { get; set; } = null!;
        public string SupplierName => Supplier == null ? string.Empty : Supplier.Name;
        public string SupplierNickName => Supplier == null ? string.Empty : Supplier.NickName;
        public string SupplierVendorCode => Supplier == null ? string.Empty : Supplier.VendorCode;
        public List<NewPurchaseOrderItemRequest> PurchaseOrderItems { get; set; } = new List<NewPurchaseOrderItemRequest>();
        public string QuoteNo { get; set; } = string.Empty;
        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;
        public string PurchaseRequisition { get; set; } = string.Empty;
        public DateTime? ApprovedDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public string TaxCode { get; set; } = string.Empty;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public DateTime CurrencyDate { get; set; }
        public double POValueQuoteCurrency => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
        PurchaseOrderItems.Sum(x => x.POItemValueQuoteCurrency);
        public double POValuePurchaseOrderCurrency => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
        PurchaseOrderItems.Sum(x => x.POItemValuePurchaseOrderCurrency);

        public double POValueUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.POItemValueUSD);
        public double POActualCurrency => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.POItemActualCurrency);

        public double POActualUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.POItemActualUSD);

        public double POApprovedUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.POItemApprovedUSD);
        public double POPotentialCommitmentUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.POItemPotentialCommitmentUSD);
        public double POCommitmentUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.POItemCommitmentUSD);
        public double POCommitmentCurrency => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.POItemCommitmentCurrency);
        public string AccountAssigment => (MainBudgetItem == null) ? string.Empty :
            MainBudgetItem.IsAlteration ? MainBudgetItem.MWOCostCenter.Name : MainBudgetItem.MWOCECName;

         public double POActualReceivingCurrency => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.POItemActualReceivingCurrency);
        public double POActualReceivingUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.POItemActualReceivingUSD);
        public double POCommitmentReceivingCurrency => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.POItemCommitmentReceivingCurrency);
        public double POCommitmentReceivingUSD => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
            PurchaseOrderItems.Sum(x => x.POItemCommitmentReceivingUSD);
        public double BudgetPendingToCommitUSD => PurchaseOrderItems.Sum(x => x.BudgetPendingToCommitUSD);
        public double BudgetUSD => PurchaseOrderItems.Sum(x => x.BudgetUSD);
        public double BudgetAssignedUSD => PurchaseOrderItems.Sum(x => x.BudgetAssignedUSD);
        public double BudgetAssignedItemUSD => PurchaseOrderItems.Sum(x => x.BudgetAssignedItemUSD);
        public string SPL => MainBudgetItem == null ? string.Empty : MainBudgetItem.IsAlteration ? "0735015000" : "151605000";
        public bool IsAlteration => MainBudgetItem == null ? false : MainBudgetItem.IsAlteration;
        public bool IsCapitalizedSalary => MainBudgetItem == null ? false : MainBudgetItem.IsCapitalizedSalary;

        public bool IsTaxEditable => MainBudgetItem == null ? false : !MainBudgetItem.IsTaxesMainTaxesData;
        public bool CanCreatePurchaseOrder => MainBudgetItem == null ? false:!MainBudgetItem.IsTaxesMainTaxesData;
        public bool IsAnyValueNotDefined => PurchaseOrderItems.Any(x => x.POItemValueQuoteCurrency <= 0);
        public bool IsAnyValueNotWellReceived => PurchaseOrderItems.Any(x => x.POItemCommitmentReceivingCurrency < 0);
        public bool IsAnyNameEmpty => PurchaseOrderItems.Any(x => x.IsNameEmpty);
        public bool IsCompletedReceived => POCommitmentReceivingCurrency == 0;
        public double MaxPercentageToReceive => POValuePurchaseOrderCurrency == 0 ? 0 : Math.Round(POCommitmentCurrency / POValuePurchaseOrderCurrency * 100.0, 2);
        public void Initialize(NewBudgetItemMWOApprovedResponse _BudgetItem, double _USDCOP, double _USDEUR)
        {

            MainBudgetItem = _BudgetItem;
            AddBudgetItem(MainBudgetItem);
            SetTRM(_USDCOP, _USDEUR, DateTime.UtcNow);
            if (MainBudgetItem.IsCapitalizedSalary) SetPurchaseOrderCurrency(CurrencyEnum.USD);
            SetQuoteCurrency(CurrencyEnum.COP);
        }
        public bool AddBudgetItem(NewBudgetItemMWOApprovedResponse _BudgetItem)
        {
            if(!PurchaseOrderItems.Any(x=>x.BudgetItemId==_BudgetItem.BudgetItemId))
            {
                NewPurchaseOrderItemRequest item = new();
                item.SetBudgetItem(_BudgetItem);
                PurchaseOrderItems.Add(item);

                SetTRM(USDCOP, USDEUR, DateTime.UtcNow);
                SetPurchaseOrderSatatus(PurchaseOrderStatus);
                SetQuoteCurrency(QuoteCurrency);
                SetPurchaseOrderCurrency(PurchaseOrderCurrency);
                return true;
            }
            return false;
            

        }
        public bool RemoveBudgetItem(NewPurchaseOrderItemRequest itemRequest)
        {
            if (PurchaseOrderItems.Any(x => x.BudgetItemId == itemRequest.BudgetItemId))
            {
                var item = PurchaseOrderItems.Single(x => x.BudgetItemId == itemRequest.BudgetItemId);
                PurchaseOrderItems.Remove(item);
                return true;
            }
            return false;
        }
        public void SetTRM(double _usdcop, double _usdEUR)
        {
            USDCOP = _usdcop;
            USDEUR = _usdEUR;
            CurrencyDate = DateTime.UtcNow;
            foreach (var row in PurchaseOrderItems)
            {
                row.PurchaseOrderUSDCOP = _usdcop;
                row.PurchaseOrderUSDEUR = _usdEUR;
                row.CurrencyDate = CurrencyDate;
            }
        }
        public void SetTRMToReceive(double _usdcop, double _usdEUR)
        {
            foreach (var row in PurchaseOrderItems)
            {
                row.ReceivingValue.USDCOP= _usdcop;
                row.ReceivingValue.USDEUR= _usdEUR;
                row.ReceivingValue.CurrencyDate = CurrencyDate;
                row.ReceivingValue.PurchaseOrderCurrency = PurchaseOrderCurrency;
            }
        }
        public void SetTRM(double _usdcop, double _usdEUR, DateTime currencydate)
        {
            USDCOP = _usdcop;
            USDEUR = _usdEUR;
            CurrencyDate = currencydate;
            foreach (var row in PurchaseOrderItems)
            {
                row.PurchaseOrderUSDCOP = _usdcop;
                row.PurchaseOrderUSDEUR = _usdEUR;
                row.CurrencyDate = CurrencyDate;
            }
        }
        public void SetQuoteCurrency(CurrencyEnum _QuoteCurrency)
        {
            QuoteCurrency = _QuoteCurrency;
            foreach (var row in PurchaseOrderItems)
            {
                row.SetQuoteCurrency(_QuoteCurrency);
            }
        }
        public void SetPurchaseOrderCurrency(CurrencyEnum _PurchaseOrderCurrency)
        {
            PurchaseOrderCurrency = MainBudgetItem.IsAlteration ? CurrencyEnum.COP : _PurchaseOrderCurrency;
            foreach (var row in PurchaseOrderItems)
            {
                row.PurchaseOrderCurrency = _PurchaseOrderCurrency;
            }
        }
        public void SetPurchaseOrderSatatus(PurchaseOrderStatusEnum statusEnum)
        {
            PurchaseOrderStatus = statusEnum;

            foreach (var row in PurchaseOrderItems)
            {
                row.PurchaseorderStatus = PurchaseOrderStatus;
            }
        }
        public void SetSupplier(NewSupplierResponse newSupplier)
        {
            Supplier = newSupplier;
            TaxCode = MainBudgetItem.IsAlteration ? Supplier.TaxCodeLP : Supplier.TaxCodeLD;
            SetPurchaseOrderCurrency(Supplier.SupplierCurrency);
        }
        public void SetPurchaseOrderName(string purchaseorderName)
        {

            PurchaseOrderName = purchaseorderName;
            if (PurchaseOrderItems.Count == 1)
                PurchaseOrderItems[0].Name = purchaseorderName;

        }
        public void SetPurchaseOrderItemName(NewPurchaseOrderItemRequest item, string purchaseorderName)
        {
            item.Name = purchaseorderName;
            if (PurchaseOrderItems.Count == 1)
            {
                PurchaseOrderName = purchaseorderName;

            }

        }
    }
}
