using Shared.Enums.PurchaseorderStatus;

namespace Shared.NewModels.PurchaseOrders.Request
{
    public class NewPurchaseOrderReceiveItemRequest
    {

        public Guid PurchaseOrderItemId { get; set; }
        public Guid BudgetItemId => BudgetItem == null ? Guid.Empty : BudgetItem.BudgetItemId;

        public bool IsNameEmpty => string.IsNullOrEmpty(Name);
        public NewBudgetItemToCreatePurchaseOrderResponse BudgetItem { get; set; } = null!;
        public double BudgetUSD => BudgetItem == null ? 0 : BudgetItem.BudgetUSD;
        public double BudgetAssignedUSD => BudgetItem == null ? 0 : BudgetItem.AssignedUSD;
        public double BudgetPotentialUSD => BudgetItem == null ? 0 : BudgetItem.PotentialCommitmentUSD;
        public double NewPotentialUSD => BudgetPotentialUSD + ItemQuoteValueUSD;
        public double PendingToCommitmUSD => BudgetUSD - NewPotentialUSD;
        public DateTime CurrencyDate { get; set; }
        public string Name { get; set; } = string.Empty;
        public double UnitaryValueCurrency { get; set; }
        public double Quantity { get; set; } = 1;
        public bool IsTaxNoProductive { get; set; } = false;
        public bool IsTaxAlteration { get; set; } = false;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public double ReceiveUSDCOP { get; set; }
        public double ReceiveUSDEUR { get; set; }
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;
        public double ItemQuoteValueCurrency => UnitaryValueCurrency * Quantity;
        public double ItemAssignedUSD =>
            PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? ItemQuoteValueCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? USDCOP == 0 ? 0 : ItemQuoteValueCurrency / USDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? USDEUR == 0 ? 0 : ItemQuoteValueCurrency / USDEUR :
            0;

        public double ItemQuoteValueUSD =>
            QuoteCurrency.Id == CurrencyEnum.USD.Id ? ItemQuoteValueCurrency :
            QuoteCurrency.Id == CurrencyEnum.COP.Id ? USDCOP == 0 ? 0 : ItemQuoteValueCurrency / USDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? USDEUR == 0 ? 0 : ItemQuoteValueCurrency / USDEUR :
            0;

        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;

        public double ActualCurrency => Receiveds.Sum(x => x.ReceivedCurrency);
        public double ReceivingCurrency { get; set; }

        public double ActualUSD => Receiveds.Sum(x => x.ReceivedUSD);


        public double NewActualCurrency => ActualCurrency + ReceivingCurrency;
        public double ReceivingUSD =>
            PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? ReceivingCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? USDCOP == 0 ? 0 : ReceivingCurrency / USDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? USDEUR == 0 ? 0 : ReceivingCurrency / USDEUR :
            0;
        public double NewActualUSD => ActualUSD + ReceivingUSD;

        public double PendingCurrency => ItemQuoteValueCurrency - ActualCurrency;
        public double PendingUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? PendingCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? USDCOP == 0 ? 0 : PendingCurrency / USDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? USDEUR == 0 ? 0 : PendingCurrency / USDEUR :
            0;

        public double NewPendingCurrency => ItemQuoteValueCurrency - NewActualCurrency;
        public double NewPendingUSD => ItemAssignedUSD - NewActualUSD;

        public bool IsAnyValueNotDefined => Receiveds.Any(x => x.ReceivedCurrency <= 0);
        public List<NewPurchaseOrderReceiveItemActualRequest> Receiveds { get; set; } = new();
        public void SetBudgetItem(NewBudgetItemToCreatePurchaseOrderResponse _BudgetItem)
        {
            BudgetItem = _BudgetItem;
        }
    }
}
