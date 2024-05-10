using Shared.Enums.PurchaseorderStatus;

namespace Shared.NewModels.PurchaseOrders.Base
{
    public class NewPurchaseOrderItemRequest
    {
        public Guid BudgetItemId => BudgetItem == null ? Guid.Empty : BudgetItem.BudgetItemId;
        public string BudgetItemName => BudgetItem == null ? string.Empty : BudgetItem.Name;
        public NewBudgetItemMWOApprovedResponse BudgetItem { get; set; } = null!;
        public List<NewPurchaseOrderItemReceivedRequest> PurchaseOrderReceiveds { get; set; } = new List<NewPurchaseOrderItemReceivedRequest>();
        public string Name { get; set; } = string.Empty;

        public bool IsTaxNoProductive { get; set; } = false;
        public bool IsTaxAlteration { get; set; } = false;
        public string ExpectedDate { get; set; } = string.Empty;
        public bool IsTaxEditable { get; set; }
        public bool IsCapitalizedSalary { get; set; }
        public string PurchaseRequisition { get; set; } = string.Empty;
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public string SupplierNickName { get; set; } = string.Empty;
        public string SupplierVendorCode { get; set; } = string.Empty;
        public PurchaseOrderStatusEnum PurchaseorderStatus { get; set; } = PurchaseOrderStatusEnum.None;
        public double PurchaseOrderUSDCOP { get; set; }
        public double PurchaseOrderUSDEUR { get; set; }
        public DateTime CurrencyDate { get; set; }
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;
        public double UnitaryValueCurrency { get; set; }
        public double Quantity { get; set; }
        public double POItemValueCurrency => UnitaryValueCurrency * Quantity;

        public double POItemValueUSD => QuoteCurrency.Id == CurrencyEnum.USD.Id ? POItemValueCurrency :
            QuoteCurrency.Id == CurrencyEnum.COP.Id ? PurchaseOrderUSDCOP == 0 ? 0 : POItemValueCurrency / PurchaseOrderUSDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? PurchaseOrderUSDEUR == 0 ? 0 : POItemValueCurrency / PurchaseOrderUSDEUR :
            0;

        public double POItemActualCurrency => PurchaseOrderReceiveds.Sum(x => x.ValueReceivedCurrency);
        public double POItemActualUSD => PurchaseOrderReceiveds.Sum(x => x.ValueReceivedUSD);
        public double POItemPotentialCommitmentUSD =>
            PurchaseorderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? POItemValueUSD : 0;

        public double POItemApprovedUSD =>
            PurchaseorderStatus.Id != PurchaseOrderStatusEnum.Created.Id ? POItemValueUSD : 0;

        public double POItemCommitmentUSD =>
            PurchaseorderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? 0 : POItemValueUSD - POItemActualUSD;

        public double BudgetUSD => BudgetItem == null ? 0 : BudgetItem.BudgetUSD;
        public double BudgetAssignedUSD => BudgetItem == null ? 0 : BudgetItem.AssignedUSD;

        public double BudgetPendingToCommitUSD => BudgetUSD - BudgetAssignedUSD - POItemValueUSD;

        public bool IsNameEmpty => string.IsNullOrEmpty(Name);
        public void SetBudgetItem(NewBudgetItemMWOApprovedResponse _BudgetItem)
        {
            BudgetItem = _BudgetItem;
        }
    }
}
