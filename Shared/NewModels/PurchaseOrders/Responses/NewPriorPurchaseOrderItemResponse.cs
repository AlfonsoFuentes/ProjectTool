using Shared.Enums.PurchaseorderStatus;

namespace Shared.NewModels.PurchaseOrders.Responses
{
    public class NewPriorPurchaseOrderItemResponse
    {
        public List<NewPriorPurchaseOrderReceivedResponse> PurchaseOrderReceiveds { get; set; } = new List<NewPriorPurchaseOrderReceivedResponse>();
        public Guid PurchaseOrderItemId { get; set; }
        public Guid BudgetItemId { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public string Name { get; set; } = string.Empty;
       
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

        public double UnitaryValuePurchaseOrderCurrency { get; set; }
        public double UnitaryValuePurchaseOrderUSD =>
            PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? UnitaryValuePurchaseOrderCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? UnitaryValuePurchaseOrderCurrency / USDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? UnitaryValuePurchaseOrderCurrency / USDEUR :
            0;
        public double UnitaryValueQuoteCurrency =>
            QuoteCurrency.Id == CurrencyEnum.USD.Id ? UnitaryValuePurchaseOrderUSD :
            QuoteCurrency.Id == CurrencyEnum.COP.Id ? UnitaryValuePurchaseOrderUSD * USDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? UnitaryValuePurchaseOrderUSD * USDEUR :
            0;

        public double QuoteValueCurrency => UnitaryValueQuoteCurrency * Quantity;

        public double AssignedUSD =>
            QuoteCurrency.Id == CurrencyEnum.USD.Id ? QuoteValueCurrency :
            QuoteCurrency.Id == CurrencyEnum.COP.Id ? QuoteValueCurrency / USDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? QuoteValueCurrency / USDEUR :
            0;

       public double ActualCurrency => (PurchaseOrderReceiveds == null || PurchaseOrderReceiveds.Count == 0) ? 0 :
            PurchaseOrderReceiveds.Sum(x => x.ValueReceivedCurrency);

        public double PendingToReceiveCurrency => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? 0 : QuoteValueCurrency - ActualCurrency;
       
        public double ActualUSD => (PurchaseOrderReceiveds == null || PurchaseOrderReceiveds.Count == 0) ? 0 :
            PurchaseOrderReceiveds.Sum(x => x.ValueReceivedUSD);

        public double PotentialCommitmentUSD =>
            PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? AssignedUSD : 0;

        public double ApprovedUSD =>
            PurchaseOrderStatus.Id != PurchaseOrderStatusEnum.Created.Id ? AssignedUSD : 0;

        public double PendingToReceiveUSD => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? 0 : AssignedUSD - ActualUSD;
      
        public string LabelAction => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ?
          $"Edit {PurchaseRequisition}" : $"Edit {PurchaseOrderNumber}";
    }
}
