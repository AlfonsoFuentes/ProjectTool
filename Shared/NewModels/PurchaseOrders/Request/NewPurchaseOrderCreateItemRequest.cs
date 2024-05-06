using Shared.Enums.PurchaseorderStatus;

namespace Shared.NewModels.PurchaseOrders.Request
{
    public class NewPurchaseOrderCreateItemRequest
    {
        public Guid BudgetItemId => BudgetItem == null ? Guid.Empty : BudgetItem.BudgetItemId;
       
        public bool IsNameEmpty=>string.IsNullOrEmpty(Name);
        public NewBudgetItemToCreatePurchaseOrderResponse BudgetItem { get; set; } = null!;
        public double BudgetUSD => BudgetItem == null ? 0 : BudgetItem.BudgetUSD;
        public double BudgetAssignedUSD => BudgetItem == null ? 0 : BudgetItem.AssignedUSD;
        public double BudgetPotentialUSD => BudgetItem == null ? 0 : BudgetItem.PotentialCommitmentUSD;
        public double NewPotentialUSD => BudgetPotentialUSD + ItemQuoteValueUSD;
        public double PendingToCommitmUSD => BudgetUSD - NewPotentialUSD;
        public DateTime CurrencyDate {  get; set; }
        public string Name { get; set; } = string.Empty;
        public double UnitaryValueCurrency { get; set; }
        public double Quantity { get; set; } = 1;
        public bool IsTaxNoProductive { get; set; } = false;
        public bool IsTaxAlteration { get; set; } = false;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;
        public double ItemQuoteValueCurrency => UnitaryValueCurrency * Quantity;
        public double ItemAssignedUSD =>
            PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? ItemQuoteValueCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? ItemQuoteValueCurrency / USDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? ItemQuoteValueCurrency / USDEUR :
            0;

        public double ItemQuoteValueUSD =>
            QuoteCurrency.Id == CurrencyEnum.USD.Id ? ItemQuoteValueCurrency :
            QuoteCurrency.Id == CurrencyEnum.COP.Id ? ItemQuoteValueCurrency / USDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? ItemQuoteValueCurrency / USDEUR :
            0;

        public PurchaseOrderStatusEnum PurchaseOrderStatus => PurchaseOrderStatusEnum.Created;

        public void SetBudgetItem( NewBudgetItemToCreatePurchaseOrderResponse _BudgetItem)
        {
            BudgetItem = _BudgetItem;
        }


    }
}
