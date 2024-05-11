using Shared.Enums.PurchaseorderStatus;

namespace Shared.NewModels.PurchaseOrders.Base
{
    public class NewPurchaseOrderItemRequest
    {
        public Guid PurchaseOrderItemId { get; set; }
        public Guid BudgetItemId => BudgetItem == null ? Guid.Empty : BudgetItem.BudgetItemId;
        public string BudgetItemName => BudgetItem == null ? string.Empty : BudgetItem.Name;
        public string Nomenclatore => BudgetItem == null ? string.Empty : BudgetItem.Nomenclatore;
        public NewBudgetItemMWOApprovedResponse BudgetItem { get; set; } = null!;
        public List<NewPurchaseOrderItemReceivedRequest> PurchaseOrderReceiveds { get; set; } = new List<NewPurchaseOrderItemReceivedRequest>();
        public NewPurchaseOrderItemReceivedRequest ReceivingValue { get; set; } = new();
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
        public DateTime? CurrencyDate { get; set; }
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;
        public double UnitaryValueQuoteCurrency { get; set; }
        public double Quantity { get; set; } = 1;
        public double POItemValueQuoteCurrency => UnitaryValueQuoteCurrency * Quantity;
        public double POItemValuePurchaseOrderCurrency => UnitaryValuePurchaseOrderCurrency * Quantity;
        public double POItemValueUSD => QuoteCurrency.Id == CurrencyEnum.USD.Id ? POItemValueQuoteCurrency :
            QuoteCurrency.Id == CurrencyEnum.COP.Id ? PurchaseOrderUSDCOP == 0 ? 0 : POItemValueQuoteCurrency / PurchaseOrderUSDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? PurchaseOrderUSDEUR == 0 ? 0 : POItemValueQuoteCurrency / PurchaseOrderUSDEUR :
            PurchaseorderStatus.Id == PurchaseOrderStatusEnum.Closed.Id ? POItemActualUSD : 0;

        public double UnitaryValuePurchaseOrderCurrency => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? UnitaryValueUSD :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? PurchaseOrderUSDCOP == 0 ? 0 : UnitaryValueUSD * PurchaseOrderUSDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? PurchaseOrderUSDEUR == 0 ? 0 : UnitaryValueUSD * PurchaseOrderUSDEUR :
            0;
        public double UnitaryValueUSD => QuoteCurrency.Id == CurrencyEnum.USD.Id ? UnitaryValueQuoteCurrency :
            QuoteCurrency.Id == CurrencyEnum.COP.Id ? PurchaseOrderUSDCOP == 0 ? 0 : UnitaryValueQuoteCurrency / PurchaseOrderUSDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? PurchaseOrderUSDEUR == 0 ? 0 : UnitaryValueQuoteCurrency / PurchaseOrderUSDEUR :
            0;

        public double POItemActualReceivingCurrency => POItemActualCurrency + ReceivingValue.ReceivingValueCurrency;
        public double POItemActualReceivingUSD => POItemActualUSD + ReceivingValue.ReceivingValueUSD;
        public double POItemCommitmentReceivingCurrency => POItemValuePurchaseOrderCurrency - POItemActualReceivingCurrency;
        public double POItemCommitmentReceivingUSD => POItemValueUSD - POItemActualReceivingUSD;


        public double POItemActualCurrency => PurchaseOrderReceiveds.Sum(x => x.ValueReceivedCurrency);
        public double POItemActualUSD => PurchaseOrderReceiveds.Sum(x => x.ValueReceivedUSD);
        public double POItemPotentialCommitmentUSD =>
            PurchaseorderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? POItemValueUSD : 0;
        public double POItemApprovedUSD =>
            PurchaseorderStatus.Id != PurchaseOrderStatusEnum.Created.Id ? POItemValueUSD : 0;
        public double POItemCommitmentUSD =>
            PurchaseorderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? 0 : POItemValueUSD - POItemActualUSD;
        public double POItemCommitmentCurrency =>
           PurchaseorderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? 0 : POItemValuePurchaseOrderCurrency - POItemActualCurrency;
        public double BudgetUSD => BudgetItem == null ? 0 : BudgetItem.BudgetUSD;
        public double BudgetAssignedUSD => BudgetItem == null ? 0 : BudgetItem.AssignedUSD;
        public double BudgetAssignedItemUSD => BudgetAssignedUSD + POItemValueUSD;
        public double BudgetPendingToCommitUSD => BudgetUSD - BudgetAssignedItemUSD;

        public bool IsNameEmpty => string.IsNullOrEmpty(Name);
        public void SetBudgetItem(NewBudgetItemMWOApprovedResponse _BudgetItem)
        {
            BudgetItem = _BudgetItem;
        }
        public void SetQuoteCurrency(CurrencyEnum _QuoteCurrency)
        {
            var oldUnitaryValueUSD = UnitaryValueUSD;

            UnitaryValueQuoteCurrency =
                _QuoteCurrency.Id == CurrencyEnum.USD.Id ? oldUnitaryValueUSD :
                _QuoteCurrency.Id == CurrencyEnum.COP.Id ? oldUnitaryValueUSD * PurchaseOrderUSDCOP :
                oldUnitaryValueUSD * PurchaseOrderUSDEUR;


            QuoteCurrency = _QuoteCurrency;
        }
    }
}
