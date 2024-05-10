using Shared.Enums.MWOStatus;
using Shared.Enums.PurchaseorderStatus;
using Shared.NewModels.PurchaseOrders.Request;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.NewModels.PurchaseOrders.Base
{
    public class NewPurchaseOrderRequest
    {
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public Guid PurchaseOrderId { get; set; }
        public Guid MWOId => (MainBudgetItem == null) ? Guid.Empty : MainBudgetItem.MWOId;
        public string MWOName => (MainBudgetItem == null) ? string.Empty : MainBudgetItem.Name;
        public string CECName => (MainBudgetItem == null) ? string.Empty : MainBudgetItem.MWOCECName;
        public int CostCenter { get; set; }
        public string Name { get; set; } = string.Empty;
        public MWOTypeEnum Type { get; set; } = MWOTypeEnum.None;
        public FocusEnum Focus { get; set; } = FocusEnum.None;
        public MWOStatusEnum MWOStatus { get; set; } = MWOStatusEnum.None;
        public bool IsAssetProductive { get; set; }

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

        public string PurchaseorderName { get; set; } = string.Empty;

        public double POValueCurrency => PurchaseOrderItems == null || PurchaseOrderItems.Count == 0 ? 0 :
          PurchaseOrderItems.Sum(x => x.POItemValueCurrency);
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
        

        public string AccountAssigment => (MainBudgetItem == null) ? string.Empty :
            MainBudgetItem.IsAlteration ? MainBudgetItem.MWOCostCenter : MainBudgetItem.MWOCECName;

        public double BudgetPendingToCommitUSD => PurchaseOrderItems.Sum(x => x.BudgetPendingToCommitUSD);
        public double BudgetUSD => PurchaseOrderItems.Sum(x => x.BudgetUSD);
       
        public string SPL => MainBudgetItem == null ? string.Empty : MainBudgetItem.IsAlteration ? "0735015000" : "151605000";
        public bool IsAlteration => MainBudgetItem == null ? false : MainBudgetItem.IsAlteration;
        public bool IsCapitalizedSalary => MainBudgetItem == null ? false : MainBudgetItem.IsCapitalizedSalary;

        public bool IsTaxEditable => MainBudgetItem == null ? false : !MainBudgetItem.IsTaxesMainTaxesData;
        public bool CanCreatePurchaseOrder => !MainBudgetItem.IsTaxesMainTaxesData;
        public bool IsAnyValueNotDefined => PurchaseOrderItems.Any(x => x.POItemValueCurrency <= 0);
        public bool IsAnyNameEmpty => PurchaseOrderItems.Any(x => x.IsNameEmpty);
        public void Initialize(NewBudgetItemMWOApprovedResponse _BudgetItem, double _USDCOP, double _USDEUR)
        {

            MainBudgetItem = _BudgetItem;
            AddBudgetItem(MainBudgetItem);
            SetTRM(_USDCOP, _USDEUR, DateTime.UtcNow);
            if (MainBudgetItem.IsCapitalizedSalary) SetPurchaseOrderCurrency(CurrencyEnum.USD);
            SetQuoteCurrency(CurrencyEnum.COP);
        }
        public void AddBudgetItem(NewBudgetItemMWOApprovedResponse _BudgetItem)
        {
            NewPurchaseOrderItemRequest item = new();
            item.SetBudgetItem(_BudgetItem);
            SetTRM(USDCOP,USDEUR, DateTime.UtcNow);
            item.QuoteCurrency = QuoteCurrency;
            item.PurchaseOrderCurrency = PurchaseOrderCurrency;
            PurchaseOrderItems.Add(item);
        }
        public void SetTRM(double _usdcop, double _usdEUR, DateTime currencydate)
        {
            USDCOP = _usdcop;
            USDEUR = _usdEUR;
            CurrencyDate = currencydate;
            foreach (var row in PurchaseOrderItems)
            {
                row.PurchaseOrderUSDCOP = _usdcop;
                row.PurchaseOrderUSDEUR= _usdEUR;
                row.CurrencyDate = CurrencyDate;
            }
        }
        public void SetQuoteCurrency(CurrencyEnum _QuoteCurrency)
        {
            QuoteCurrency = _QuoteCurrency;
            foreach (var row in PurchaseOrderItems)
            {
                row.QuoteCurrency = _QuoteCurrency;
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

        public void SetSupplier(NewSupplierResponse newSupplier)
        {
            Supplier = newSupplier;
            TaxCode = MainBudgetItem.IsAlteration ? Supplier.TaxCodeLP : Supplier.TaxCodeLD;
            SetPurchaseOrderCurrency(Supplier.SupplierCurrency);
        }
        public void SetPurchaseOrderName(string purchaseorderName)
        {

            PurchaseorderName = purchaseorderName;
            if (PurchaseOrderItems.Count == 1)
                PurchaseOrderItems[0].Name = purchaseorderName;
            //if (IsTaxEditable)
            //{
            //    PurchaseRequisition = $"Tax for {PurchaseorderName}";
            //    QuoteNo = $"Tax for {PurchaseorderName}";
            //    TaxCode = $"Tax for {PurchaseorderName}";
            //}
        }
        public void SetPurchaseOrderItemName(NewPurchaseOrderCreateItemRequest item, string purchaseorderName)
        {
            item.Name = purchaseorderName;
            if (PurchaseOrderItems.Count == 1)
            {
                PurchaseorderName = purchaseorderName;

            }

        }
    }
}
