using Shared.Enums.PurchaseorderStatus;

namespace Shared.NewModels.PurchaseOrders.Responses
{
    public class NewPriorPurchaseOrderItemResponse
    {
        public List<NewPriorPurchaseOrderReceivedResponse> PurchaseOrderReceiveds { get; set; } = new List<NewPriorPurchaseOrderReceivedResponse>();
        public Guid NewPriorPurchaseOrderItemResponseId { get; set; }
        public Guid BudgetItemId { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double UnitaryValueCurrency { get; set; }
        public double Quantity { get; set; }
        public bool IsTaxNoProductive { get; set; } = false;
        public bool IsTaxAlteration { get; set; } = false;
        public DateTime? POExpectedDate { get; set; }
        public string ExpectedOn { get; set; } = string.Empty;
        public bool IsTaxEditable { get; set; }
        public bool IsCapitalizedSalary { get; set; }
        public string PurchaseRequisition { get; set; } = string.Empty;
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;

        public string PurchaseOrderLegendToDelete => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ?
            PurchaseRequisition : PurchaseOrderNumber;

        public double QuoteValueCurrency => UnitaryValueCurrency * Quantity;

        public double AssignedUSD =>
            PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? QuoteValueCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? QuoteValueCurrency / USDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? QuoteValueCurrency / USDEUR :
            0;

        public double QuoteValueUSD =>
            QuoteCurrency.Id == CurrencyEnum.USD.Id ? QuoteValueCurrency :
            QuoteCurrency.Id == CurrencyEnum.COP.Id ? QuoteValueCurrency / USDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? QuoteValueCurrency / USDEUR :
            0;
        public double ActualCurrency => (PurchaseOrderReceiveds == null || PurchaseOrderReceiveds.Count == 0) ? 0 :
            PurchaseOrderReceiveds.Sum(x => x.ValueReceivedCurrency);

        public double PendintToReceiveCurrency => QuoteValueCurrency - ActualCurrency;
        public double ActualUSD => (PurchaseOrderReceiveds == null || PurchaseOrderReceiveds.Count == 0) ? 0 :
            PurchaseOrderReceiveds.Sum(x => x.ValueReceivedUSD);

        public double PotentialCommitmentUSD =>
            PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? AssignedUSD : 0;

        public double ApprovedUSD =>
            PurchaseOrderStatus.Id != PurchaseOrderStatusEnum.Created.Id ? AssignedUSD : 0;

        public double PendingToReceiveUSD =>
            PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? PendintToReceiveCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? PendintToReceiveCurrency / USDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? PendintToReceiveCurrency / USDEUR :
            0;
        public string LabelAction => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ?
          $"Edit {PurchaseRequisition}" : $"Edit {PurchaseOrderNumber}";
    }
}
