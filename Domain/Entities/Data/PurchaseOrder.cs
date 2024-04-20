using Shared.Models.Currencies;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Data
{
    public class PurchaseOrder : BaseEntity, ITenantEntity
    {
        public Guid MWOId { get; set; }
        public MWO MWO { get; set; } = null!;
        public string TenantId { get; set; } = string.Empty;
        public Guid MainBudgetItemId { get; set; }
        public Guid? SupplierId { get; set; }
        public Supplier? Supplier { get; set; } = null!;
        public ICollection<DownPayment> DownPayments { get; set; } = new List<DownPayment>();
        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
        public static PurchaseOrder Create(Guid mwoid)
        {
            PurchaseOrder item = new PurchaseOrder();
            item.MWOId = mwoid;
            item.Id = Guid.NewGuid();

            return item;
        }
        public PurchaseOrderItem AddPurchaseOrderItem(Guid mwobudgetitemid, string name)
        {
            var row = PurchaseOrderItem.Create(Id, mwobudgetitemid);
            row.Name = name;

            return row;
        }
        public PurchaseOrderItem AddPurchaseOrderItemForNoProductiveTax(Guid mwobudgetitemid, string name)
        {
            var row = PurchaseOrderItem.Create(Id, mwobudgetitemid);
            row.Name = name;
            row.IsTaxNoProductive = true;
            return row;
        }
        public PurchaseOrderItem AddPurchaseOrderItemForAlteration(Guid mwobudgetitemid, string name)
        {
            var row = PurchaseOrderItem.Create(Id, mwobudgetitemid);
            row.Name = name;

            row.IsTaxAlteration = true;
            return row;
        }
        public string QuoteNo { get; set; } = "";
        public int QuoteCurrency { get; set; }
        public int Currency { get; set; }
        public int PurchaseOrderStatus { get; set; }
        public string PurchaseRequisition { get; set; } = "";


        public DateTime? POApprovedDate { get; set; }
        public DateTime? POExpectedDateDate { get; set; }
        public DateTime? POClosedDate { get; set; }
        public string PONumber { get; set; } = "";
        public string SPL { get; set; } = "";
        public string TaxCode { get; set; } = "";
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public DateTime CurrencyDate { get; set; }
        public string AccountAssigment { get; set; } = "";
        public double POValueCurrency { get; set; }
   
        public double ActualCurrency { get; set; }
        public string PurchaseorderName { get; set; } = string.Empty;
       
        public bool IsAlteration { get; set; } = false;
        public bool IsCapitalizedSalary { get; set; } = false;
       
        public bool IsTaxEditable { get; set; } = false;
        [NotMapped]
        public double ActualUSD => Currency == CurrencyEnum.USD.Id ? ActualCurrency :
            Currency == CurrencyEnum.COP.Id ? ActualCurrency / USDCOP : ActualCurrency / USDEUR;
        [NotMapped]
        public double POValueUSD => Currency == CurrencyEnum.USD.Id ? POValueCurrency :
            Currency == CurrencyEnum.COP.Id ? POValueCurrency / USDCOP : POValueCurrency / USDEUR;
    }
}
