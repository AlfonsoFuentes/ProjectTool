using Shared.Models.Currencies;
using Shared.Models.PurchaseorderStatus;
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
        public static PurchaseOrderItem Create(Guid purchasorderid, Guid mwobudgetitemid)
        {
            PurchaseOrderItem item = new PurchaseOrderItem();
            item.Id = Guid.NewGuid();
            item.BudgetItemId = mwobudgetitemid;
            item.PurchaseOrderId = purchasorderid;

            return item;
        }
        public string Name { get; set; } = string.Empty;

        public void ChangeBudgetItem(Guid newbudgetitemid)
        {
            BudgetItemId = newbudgetitemid;
        }

        public double UnitaryValueCurrency { get; set; }
        public double ActualCurrency { get; set; }
        public double Quantity { get; set; }
        public bool IsTaxNoProductive { get; set; } = false;
        public bool IsTaxAlteration { get; set; } = false;
        [NotMapped]
        public string ExpectedDateDate => PurchaseOrder == null ? string.Empty : 
            PurchaseOrder.POExpectedDateDate==null?string.Empty:
            PurchaseOrder.POExpectedDateDate!.Value.ToString("d");
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
        public PurchaseOrderStatusEnum PurchaseorderStatus =>
            PurchaseOrder == null ? PurchaseOrderStatusEnum.None :
            PurchaseOrderStatusEnum.GetType(PurchaseOrder.PurchaseOrderStatus);
        [NotMapped]
        public double USDCOP => PurchaseOrder == null ? 0 : PurchaseOrder.USDCOP;
        [NotMapped]
        public double USDEUR => PurchaseOrder == null ? 0 : PurchaseOrder.USDEUR;
        [NotMapped]
        public CurrencyEnum PurchaseOrderCurrency => PurchaseOrder == null ? CurrencyEnum.None : CurrencyEnum.GetType(PurchaseOrder.Currency);
        [NotMapped]
        public CurrencyEnum QuoteCurrency => PurchaseOrder == null ? CurrencyEnum.None : CurrencyEnum.GetType(PurchaseOrder.QuoteCurrency);
        [NotMapped]
        public double TotalValueCurrency => UnitaryValueCurrency * Quantity;
        [NotMapped]
        public double AssignedUSD =>
            PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? TotalValueCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? TotalValueCurrency / USDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? TotalValueCurrency / USDEUR :
            0;
        [NotMapped]
        public double QuoteValueUSD =>
            QuoteCurrency.Id == CurrencyEnum.USD.Id ? QuoteValueCurrency :
            QuoteCurrency.Id == CurrencyEnum.COP.Id ? QuoteValueCurrency / USDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? QuoteValueCurrency / USDEUR :
            0;
        [NotMapped]
        public double QuoteValueCurrency =>
            QuoteCurrency.Id == CurrencyEnum.USD.Id ? AssignedUSD :
            QuoteCurrency.Id == CurrencyEnum.COP.Id ? AssignedUSD * USDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? AssignedUSD * USDEUR :
            0;
        [NotMapped]
        public double ActualUSD =>
            PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? ActualCurrency :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? ActualCurrency / USDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? ActualCurrency / USDEUR :
            0;
        [NotMapped]
        public double PotentialCommitmentUSD =>
            PurchaseorderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? AssignedUSD : 0;
        [NotMapped]
        public double ApprovedUSD =>
            PurchaseorderStatus.Id != PurchaseOrderStatusEnum.Created.Id ? AssignedUSD : 0;
        [NotMapped]
        public double PendingToReceiveUSD =>
            PurchaseorderStatus.Id == PurchaseOrderStatusEnum.Created.Id ? 0 : ApprovedUSD - ActualUSD;


    }
}
