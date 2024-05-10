using Shared.Enums.PurchaseorderStatus;

namespace Shared.NewModels.PurchaseOrders.Request
{
    public class NewPurchaseOrderCreateRequest
    {

        public NewBudgetItemToCreatePurchaseOrderResponse MainBudgetItem { get; set; } = null!;

        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public Guid MWOId => MainBudgetItem == null ? Guid.Empty : MainBudgetItem.MWOId;
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public string CECName => MainBudgetItem == null ? string.Empty : MainBudgetItem.MWOCECName;
        public Guid MainBudgetItemId => MainBudgetItem == null ? Guid.Empty : MainBudgetItem.BudgetItemId;
        public Guid? SupplierId => Supplier == null ? Guid.Empty : Supplier.SupplierId;
        public string SupplierName => Supplier == null ? string.Empty : Supplier.Name;
        public string SupplierNickName => Supplier == null ? string.Empty : Supplier.NickName;
        public string SupplierVendorCode => Supplier == null ? string.Empty : Supplier.VendorCode;
        public string TaxCode { get; set; } = string.Empty;
        public NewSupplierResponse? Supplier { get; set; } = null!;
        public List<NewPurchaseOrderCreateItemRequest> PurchaseOrderItems { get; set; } = new List<NewPurchaseOrderCreateItemRequest>();
        public string QuoteNo { get; set; } = "";

        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.Created;
        public string PurchaseRequisition { get; set; } = "";
       
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public DateTime CurrencyDate { get; set; }
        public string AccountAssigment => (MainBudgetItem == null) ? string.Empty :
            MainBudgetItem.IsAlteration ? MainBudgetItem.MWOCostCenter : MainBudgetItem.MWOCECName;
        public string SPL => MainBudgetItem == null ? string.Empty : MainBudgetItem.IsAlteration ? "0735015000" : "151605000";
        public string MWOCECName => (MainBudgetItem == null) ? string.Empty : MainBudgetItem.MWOCECName;
        public string PurchaseorderName { get; set; } = string.Empty;
        public bool IsAlteration => MainBudgetItem == null ? false : MainBudgetItem.IsAlteration;
        public bool IsCapitalizedSalary => MainBudgetItem == null ? false : MainBudgetItem.IsCapitalizedSalary;

        public bool IsTaxEditable => MainBudgetItem == null ? false : !MainBudgetItem.IsTaxesMainTaxesData;

        public double POValueUSD => PurchaseOrderItems.Sum(x => x.ItemQuoteValueUSD);
        public double POValueCurrency => PurchaseOrderItems.Sum(x => x.ItemQuoteValueCurrency);
        public bool IsAnyValueNotDefined => PurchaseOrderItems.Any(x => x.ItemQuoteValueCurrency <= 0);
        public bool IsAnyNameEmpty => PurchaseOrderItems.Any(x => x.IsNameEmpty);

        public double TotalBudgetUSD => PurchaseOrderItems.Sum(x => x.BudgetUSD);
        public double TotalAssignedUSD => PurchaseOrderItems.Sum(x => x.BudgetAssignedUSD);
        public double TotalPotentialUSD => PurchaseOrderItems.Sum(x => x.BudgetPotentialUSD);
        public double NewTotalPendingToCommitUSD => PurchaseOrderItems.Sum(x => x.PendingToCommitmUSD);
        public double NewTotalAssignedUSD => TotalAssignedUSD;
        public double NewTotalPotentialUSD => POValueUSD + TotalPotentialUSD;
        public void SetMainBudgetItem(NewBudgetItemToCreatePurchaseOrderResponse _BudgetItem, double _USDCOP, double _USDEUR)
        {

            MainBudgetItem = _BudgetItem;
            AddBudgetItem(MainBudgetItem);
            SetTRM(_USDCOP, _USDEUR, DateTime.UtcNow);
            if (MainBudgetItem.IsCapitalizedSalary) SetPurchaseOrderCurrency(CurrencyEnum.USD);
            SetQuoteCurrency(CurrencyEnum.COP);
        }
        public void AddBudgetItem(NewBudgetItemToCreatePurchaseOrderResponse _BudgetItem)
        {
            NewPurchaseOrderCreateItemRequest item = new();
            item.SetBudgetItem(_BudgetItem);
            item.USDCOP = USDCOP;
            item.USDEUR = USDEUR;
            item.CurrencyDate = CurrencyDate;
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
                row.USDCOP = _usdcop;
                row.USDEUR = _usdEUR;
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
