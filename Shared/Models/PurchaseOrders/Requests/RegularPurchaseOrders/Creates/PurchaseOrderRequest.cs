using Shared.Models.BudgetItems;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.Suppliers;

namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates
{
    public abstract class PurchaseOrderRequest
    {
        
        public string PurchaseorderName { get; set; } = string.Empty;
        public SupplierResponse? Supplier { get; set; }
        public Guid SupplierId=> Supplier == null ? Guid.Empty : Supplier.Id;
        public string SupplierName => Supplier == null ? string.Empty : Supplier.NickName;
        public string VendorCode => Supplier == null ? string.Empty : Supplier.VendorCode;
        public string TaxCode => Supplier == null ? string.Empty : IsAlteration || IsMWONoProductive ? Supplier.TaxCodeLP : Supplier.TaxCodeLD;
        public bool IsAssetProductive { get; set; }
        public bool IsMWONoProductive => !IsAssetProductive;
        public bool IsAlteration => MainBudgetItem.IsAlteration;
        public BudgetItemApprovedResponse MainBudgetItem { get; set; } = new();
        public Guid MainBudgetItemId => MainBudgetItem.BudgetItemId;
        public string SPL => IsAlteration ? "0735015000" : "151605000";

        public string QuoteNo { get; set; } = string.Empty;
        public string PurchaseRequisition { get; set; } = string.Empty;
        public Guid MWOId { get; set; }
        public string MWOName { get; set; } = string.Empty;
        public string AccountAssigment => IsAlteration ? CostCenter : MWOCECName;
        public string CostCenter { get; set; } = string.Empty;
        public string MWOCECName { get; set; } = string.Empty;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum QuoteCurrency { get; set; }= CurrencyEnum.None;
        public abstract double SumPOValueUSD { get; }
        public abstract double SumPOValueCurrency { get;  }
        public abstract double SumBudget { get;  }
        public abstract double SumBudgetAssigned { get;  }
        public abstract double SumBudgetPotencial { get;  }
        
        public abstract double SumPendingUSD { get;  }
        public abstract List<PurchaseOrderItemRequest> PurchaseOrderItems { get; set; }
    }
}
