using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.Currencies;
using Shared.Models.MWO;
using Shared.Models.PurchaseorderStatus;
using Shared.Models.Suppliers;
using System.Xml.Linq;

namespace Shared.Models.PurchaseOrders.Requests.Create
{
    public class EditPurchaseOrderCreatedRequest
    {
        public Guid MainBudgetItemId { get; set; }
        public Guid PurchaseorderId { get; set; }
        public Guid SupplierId { get; set; }
        public List<string> ValidationErrors { get; set; } = new();
        public string MWOName { get; set; } = string.Empty;
        public List<EditPurchaseorderItemCreatedRequest> PurchaseOrderItems { get; set; } = new();
        public PurchaseOrderStatusEnum PurchaseOrderStatusEnum { get; set; } = PurchaseOrderStatusEnum.None;
        public string PurchaseOrderName { get; set; } = string.Empty;
        public string QuoteNo { get; set; } = string.Empty;
        public string PurchaseRequisition { get; set; } = string.Empty;
        public string TaxCode { get; set; } = string.Empty;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public bool AssetRealProductive { get; set; }
        public string AccountAssigment { get; set; } = string.Empty;
        public string MWOCECName { get; set; } = string.Empty;
        public bool IsAlteration { get; set; }
        public string SPL => IsAlteration ? "0735015000" : "151605000";
        public string VendorCode { get; set; } = string.Empty;
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public Guid MWOId { get; set; }

        CurrencyEnum _QuoteCurrency = CurrencyEnum.COP;
        public DateTime CurrencyDate { get; set; }
        public CurrencyEnum QuoteCurrency
        {
            get => _QuoteCurrency;
            set
            {
                _QuoteCurrency = value;
                ChangeQuoteCurrency(value);

            }
        }
        public void ChangeName(string name)
        {
            ValidationErrors.Clear();
            PurchaseOrderName = name;
            if (PurchaseOrderItems.Count == 1)
            {
                PurchaseOrderItems[0].PurchaseOrderItemName = PurchaseOrderName;
            }

        }
        public void ChangeName(EditPurchaseorderItemCreatedRequest model, string name)
        {
            ValidationErrors.Clear();
            model.PurchaseOrderItemName = name;

            if (PurchaseOrderItems.Count == 1)
            {
                PurchaseOrderName = name;
            }

        }
        public void ChangeQuoteCurrency(CurrencyEnum currencyEnum)
        {
            ValidationErrors.Clear();
            foreach (var item in PurchaseOrderItems)
            {
                item.ChangeCurrency(currencyEnum);
            }

        }

        public void SetSupplier(SupplierResponse _Supplier)
        {
            ValidationErrors.Clear();
            SupplierId = _Supplier.Id;

            VendorCode = _Supplier.VendorCode;
            TaxCode = IsAlteration ? _Supplier.TaxCodeLP :
                         AssetRealProductive ?
                             _Supplier.TaxCodeLD : _Supplier.TaxCodeLP;
            PurchaseOrderCurrency = _Supplier.SupplierCurrency;
        }

        public SupplierResponse Supplier { get; set; } = null!;


        public double SumPOValueUSD => PurchaseOrderItems.Count == 0 ? 0 : PurchaseOrderItems.Sum(x => x.TotalValueUSDItem);
        public double SumPOValueCurrency => PurchaseOrderItems.Count == 0 ? 0 : PurchaseOrderItems.Sum(x => x.TotalCurrencyValue);
        public double SumBudget => PurchaseOrderItems.Count == 0 ? 0 : PurchaseOrderItems.Sum(x => x.Budget);
        public double SumBudgetAssigned => PurchaseOrderItems.Count == 0 ? 0 : PurchaseOrderItems.Sum(x => x.BudgetAssigned);
        public double SumBudgetPotencialAssigned => PurchaseOrderItems.Count == 0 ? 0 : PurchaseOrderItems.Sum(x => x.BudgetPotencialAssigned);
    }
}
