using Shared.Enums.Currencies;
using Shared.Enums.PurchaseorderStatus;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Data
{
    public class PurchaseOrderItem : BaseEntity, ITenantEntity
    {
        public Guid BudgetItemId { get; private set; }
        public BudgetItem BudgetItem { get; set; } = null!;
        public Guid PurchaseOrderId { get; private set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;
        public string TenantId { get; set; } = string.Empty;
        public ICollection<PurchaseOrderItemReceived> PurchaseOrderReceiveds { get; set; } = new List<PurchaseOrderItemReceived>();
        public static PurchaseOrderItem Create(Guid purchasorderid, Guid mwobudgetitemid)
        {
            PurchaseOrderItem item = new PurchaseOrderItem();
            item.Id = Guid.NewGuid();
            item.BudgetItemId = mwobudgetitemid;
            item.PurchaseOrderId = purchasorderid;

            return item;
        }
        public PurchaseOrderItemReceived AddPurchaseOrderReceived()
        {
            var row = PurchaseOrderItemReceived.Create();
            row.PurchaseOrderItemId = Id;
            row.ReceivedDate = DateTime.UtcNow;
            return row;
        }
        public string Name { get; set; } = string.Empty;

        public void ChangeBudgetItem(Guid newbudgetitemid)
        {
            BudgetItemId = newbudgetitemid;
        }

        public double UnitaryValueCurrency { get; set; }
        public double Quantity { get; set; }
        public bool IsTaxNoProductive { get; set; } = false;
        public bool IsTaxAlteration { get; set; } = false;
        [NotMapped]
        public string NomenclatoreName => BudgetItem == null ? string.Empty : BudgetItem.NomenclatoreName;
        [NotMapped]
        public CurrencyEnum PurchaseOrderCurrency => PurchaseOrder == null ? CurrencyEnum.None : CurrencyEnum.GetType(PurchaseOrder.PurchaseOrderCurrency);
        [NotMapped]
        public CurrencyEnum QuoteCurrency => PurchaseOrder == null ? CurrencyEnum.None : CurrencyEnum.GetType(PurchaseOrder.QuoteCurrency);
        [NotMapped]
        public double UnitaryValuePurchaseOrderCurrency => UnitaryValueCurrency;
        [NotMapped]
        public double UnitaryValueUSD => PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? UnitaryValuePurchaseOrderCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? USDCOP == 0 ? 0 : UnitaryValuePurchaseOrderCurrency / USDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? USDEUR == 0 ? 0 : UnitaryValuePurchaseOrderCurrency / USDEUR :
            0;
        [NotMapped]
        public double UnitaryValueQuoteCurrency => QuoteCurrency.Id == CurrencyEnum.USD.Id ? UnitaryValueUSD :
            QuoteCurrency.Id == CurrencyEnum.COP.Id ? UnitaryValueUSD * USDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? UnitaryValueUSD * USDEUR : 0;

        [NotMapped]
        public double POItemQuoteValueCurrency => UnitaryValueQuoteCurrency * Quantity;
        [NotMapped]
        public double POItemValueUSD => UnitaryValueUSD * Quantity;

        [NotMapped]
        public double POItemActualCurrency => (PurchaseOrderReceiveds == null || PurchaseOrderReceiveds.Count == 0) ? 0 : PurchaseOrderReceiveds.Sum(x => x.ValueReceivedCurrency);
        [NotMapped]
        public DateTime? CurrencyDate => PurchaseOrder == null ? null! : PurchaseOrder.CurrencyDate;
        [NotMapped]
        public DateTime? POExpectedDate => PurchaseOrder == null ? null! : PurchaseOrder.POExpectedDateDate;
        [NotMapped]
        public string ExpectedDateDate => POExpectedDate == null ? string.Empty : POExpectedDate!.Value.ToString("d");
        [NotMapped]
        public bool IsTaxEditable => PurchaseOrder == null ? false : PurchaseOrder.IsTaxEditable;
        [NotMapped]
        public bool IsCapitalizedSalary => PurchaseOrder == null ? false : PurchaseOrder.IsCapitalizedSalary;

        [NotMapped]
        public string PurchaseRequisition => PurchaseOrder == null ? string.Empty : PurchaseOrder.PurchaseRequisition;
        [NotMapped]
        public string PurchaseOrderNumber => PurchaseOrder == null ? string.Empty : PurchaseOrder.PONumber;
        [NotMapped]
        public string Supplier => PurchaseOrder == null ? string.Empty : PurchaseOrder.Supplier == null ? string.Empty : PurchaseOrder.Supplier.NickName;
        [NotMapped]
        public PurchaseOrderStatusEnum PurchaseorderStatus => PurchaseOrder == null ? PurchaseOrderStatusEnum.None :
            PurchaseOrderStatusEnum.GetType(PurchaseOrder.PurchaseOrderStatus);
        [NotMapped]
        public double USDCOP => PurchaseOrder == null ? 0 : PurchaseOrder.USDCOP;
        [NotMapped]
        public double USDEUR => PurchaseOrder == null ? 0 : PurchaseOrder.USDEUR;

        [NotMapped]
        public double ActualUSD => (PurchaseOrderReceiveds == null || PurchaseOrderReceiveds.Count == 0) ? 0 : PurchaseOrderReceiveds.Sum(x => x.ValueReceivedUSD);
        [NotMapped]
        public double PotentialCommitmentUSD =>
            PurchaseorderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? POItemValueUSD : 0;
        [NotMapped]
        public double ApprovedUSD =>
            PurchaseorderStatus.Id != PurchaseOrderStatusEnum.Created.Id ? POItemValueUSD : 0;
        [NotMapped]
        public double CommitmentUSD =>
            PurchaseorderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? 0 : ApprovedUSD - ActualUSD;



    }
}
