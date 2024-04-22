using Shared.Models.Currencies;
using Shared.Models.PurchaseorderStatus;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models.PurchaseOrders.Responses
{
    public class NewPurchaseOrderItemResponse
    {
        public Guid BudgetItemId { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double UnitaryValueCurrency { get; set; }
        public double ActualCurrency { get; set; }
        public double Quantity { get; set; }
        public bool IsTaxNoProductive { get; set; } = false;
        public bool IsTaxAlteration { get; set; } = false;
        public bool IsCapitalizedSalary { get; set; } = false;
        public bool IsTaxEditable { get; set; } = false;
        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;
        public string PurchaseRequisition { get; set; } = string.Empty;
        public string PurchaseOrderNumber { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;
        public double TotalValueCurrency => UnitaryValueCurrency * Quantity;
        public double AssignedUSD =>
            PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? TotalValueCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? TotalValueCurrency / USDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? TotalValueCurrency / USDEUR :
            0;

        public double QuoteValueUSD =>
            QuoteCurrency.Id == CurrencyEnum.USD.Id ? QuoteValueCurrency :
            QuoteCurrency.Id == CurrencyEnum.COP.Id ? QuoteValueCurrency / USDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? QuoteValueCurrency / USDEUR :
            0;

        public double QuoteValueCurrency =>
            QuoteCurrency.Id == CurrencyEnum.USD.Id ? AssignedUSD :
            QuoteCurrency.Id == CurrencyEnum.COP.Id ? AssignedUSD * USDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? AssignedUSD * USDEUR :
            0;

        public double ActualUSD =>
            PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? ActualCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? ActualCurrency / USDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? ActualCurrency / USDEUR :
            0;

        public double PotentialCommitmentUSD =>
            PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? AssignedUSD : 0;

        public double ApprovedUSD =>
            PurchaseOrderStatus.Id != PurchaseOrderStatusEnum.Created.Id ? AssignedUSD : 0;

        public double PendingToReceiveUSD =>
            PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? 0 : ApprovedUSD - ActualUSD;

        public string ExpectedOn { get; set; } = string.Empty;
        public string LabelAction => PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id ?
           $"Edit {PurchaseRequisition}" : $"Edit {PurchaseOrderNumber}";
    }
}
