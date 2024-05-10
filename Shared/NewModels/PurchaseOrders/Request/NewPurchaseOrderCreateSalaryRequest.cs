using Shared.Enums.PurchaseorderStatus;

namespace Shared.NewModels.PurchaseOrders.Request
{
    public class NewPurchaseOrderCreateSalaryRequest
    {

        public NewBudgetItemToCreatePurchaseOrderResponse MainBudgetItem { get; set; } = null!;

        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public Guid MWOId => MainBudgetItem == null ? Guid.Empty : MainBudgetItem.MWOId;
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public string CECName => MainBudgetItem == null ? string.Empty : MainBudgetItem.MWOCECName;
        public Guid MainBudgetItemId => MainBudgetItem == null ? Guid.Empty : MainBudgetItem.BudgetItemId;

        public List<NewPurchaseOrderReceiveItemRequest> PurchaseOrderItems { get; set; } = new List<NewPurchaseOrderReceiveItemRequest>();

        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.Closed;
  
        public string SPL { get; set; }="151605000";
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public DateTime CurrencyDate { get; set; }=DateTime.Now;
        public string AccountAssigment => (MainBudgetItem == null) ? string.Empty :MainBudgetItem.MWOCECName;

        public string MWOCECName => (MainBudgetItem == null) ? string.Empty : MainBudgetItem.MWOCECName;
        public string PurchaseorderName { get; set; } = string.Empty;
        public bool IsAlteration { get; set; } = false;
        public bool IsCapitalizedSalary => MainBudgetItem == null ? false : MainBudgetItem.IsCapitalizedSalary;
        public bool CreatePurchaseOrderNumber {  get; set; } = false;
        public bool IsTaxEditable  { get; set; } = false;

        public double POValueUSD => PurchaseOrderItems.Sum(x => x.ItemQuoteValueUSD);
        public double POValueCurrency => PurchaseOrderItems.Sum(x => x.ItemQuoteValueCurrency);
        public bool IsAnyValueNotDefined => PurchaseOrderItems.Any(x => x.ItemQuoteValueCurrency <= 0);
        public bool IsAnyNameEmpty => PurchaseOrderItems.Any(x => x.IsNameEmpty);

        public double TotalBudgetUSD => PurchaseOrderItems.Sum(x => x.BudgetUSD);
        public double TotalAssignedUSD => PurchaseOrderItems.Sum(x => x.BudgetAssignedUSD);
        public double TotalPotentialUSD => PurchaseOrderItems.Sum(x => x.BudgetPotentialUSD);
        public double NewTotalPendingToCommitUSD => PurchaseOrderItems.Sum(x => x.PendingToCommitmUSD);
        public double NewTotalAssignedUSD => TotalAssignedUSD+ POValueUSD;
        public double NewTotalPotentialUSD => POValueUSD + TotalPotentialUSD;
        public void SetMainBudgetItem(NewBudgetItemToCreatePurchaseOrderResponse _BudgetItem, double _USDCOP, double _USDEUR)
        {

            MainBudgetItem = _BudgetItem;
            AddBudgetItem(MainBudgetItem);
            SetTRM(_USDCOP, _USDEUR, DateTime.UtcNow);
            
        }
        public void AddBudgetItem(NewBudgetItemToCreatePurchaseOrderResponse _BudgetItem)
        {
            NewPurchaseOrderReceiveItemRequest item = new();
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
       

       
        public void SetPurchaseOrderName(string purchaseorderName)
        {

            PurchaseorderName = purchaseorderName;
            if (PurchaseOrderItems.Count == 1)
                PurchaseOrderItems[0].Name = purchaseorderName;
         
        }
        public void SetPurchaseOrderItemName(NewPurchaseOrderReceiveItemRequest item, string purchaseorderName)
        {
            item.Name = purchaseorderName;
            if (PurchaseOrderItems.Count == 1)
            {
                PurchaseorderName = purchaseorderName;

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

    }
}
