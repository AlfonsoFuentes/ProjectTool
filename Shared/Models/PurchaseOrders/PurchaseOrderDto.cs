using Shared.Enums.Currencies;
using Shared.Enums.PurchaseorderStatus;
using Shared.Models.BudgetItems;
using Shared.NewModels.Suppliers.Reponses;

namespace Shared.Models.PurchaseOrders
{
    public enum PurchaseOrderAction
    {
        Create,
        Update,
        Delete
    }
    public class PurchaseOrderDto
    {
        public PurchaseOrderAction PurchaseOrderAction { get; set; } = PurchaseOrderAction.Create;
        public PurchaseOrderStatusEnum PurchaseOrderStatus { get; set; } = PurchaseOrderStatusEnum.None;
        public Guid PurchaseOrderId { get; set; }

        public List<PurchaseOrderItemRequestDto> PurchaseOrderItems { get; set; } = new();
        public List<PurchaseOrderItemRequestDto> ConcretePurchaseOrderItems => PurchaseOrderItems.Where(x => x.PurchaseOrderItemId != Guid.Empty).ToList();
        public NewSupplierResponse? Supplier { get; set; }
        public Guid SupplierId => Supplier == null ? Guid.Empty : Supplier.SupplierId;
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
        double _USDCOP;
        public double USDCOP
        {
            get { return _USDCOP; }
            set
            {
                _USDCOP = value;
                foreach (var row in ConcretePurchaseOrderItems)
                {
                    row.USDCOP = value;
                }
            }
        }
        double _USDEUR;

        public double USDEUR
        {
            get { return _USDEUR; }
            set
            {
                _USDEUR = value;
                foreach (var row in ConcretePurchaseOrderItems)
                {
                    row.USDEUR = value;
                }
            }
        }
        CurrencyEnum _PurchaseOrderCurrency = CurrencyEnum.None;
        public CurrencyEnum PurchaseOrderCurrency
        {
            get => _PurchaseOrderCurrency;
            set
            {
                _PurchaseOrderCurrency = value;
                foreach (var row in ConcretePurchaseOrderItems)
                {
                    row.PurchaseOrderCurrency = value;
                }
            }
        }
        CurrencyEnum _QuoteCurrency = CurrencyEnum.None;
        public CurrencyEnum QuoteCurrency
        {
            get => _QuoteCurrency;
            set
            {
                _QuoteCurrency = value;
                foreach (var row in ConcretePurchaseOrderItems)
                {
                    row.QuoteCurrency = value;
                }
            }
        }
    }
    public class PurchaseOrderItemRequestDto
    {
        public Guid PurchaseOrderItemId { get; set; }
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
        public CurrencyEnum QuoteCurrency { get; set; } = CurrencyEnum.None;
        public double USDCOP { get; set; }
        public double USDEUR { get; set; }
        public double UnitaryValueQuoteCurrency { get; set; }
        public double UnitaryValuePurchaseOrderCurrency =>
            PurchaseOrderCurrency.Id == CurrencyEnum.USD.Id ? UnitaryValueFromQuoteValueUSD :
            PurchaseOrderCurrency.Id == CurrencyEnum.COP.Id ? UnitaryValueFromQuoteValueUSD * USDCOP :
            PurchaseOrderCurrency.Id == CurrencyEnum.EUR.Id ? UnitaryValueFromQuoteValueUSD * USDEUR :
            0;

        public double UnitaryValueFromQuoteValueUSD =>
            QuoteCurrency.Id == CurrencyEnum.USD.Id ? UnitaryValueQuoteCurrency :
            QuoteCurrency.Id == CurrencyEnum.COP.Id ? UnitaryValueQuoteCurrency / USDCOP :
            QuoteCurrency.Id == CurrencyEnum.EUR.Id ? UnitaryValueQuoteCurrency / USDEUR :
            0;

        public double Quantity { get; set; }
        public double TotalValueFromQuoteValueUSD => UnitaryValueFromQuoteValueUSD * Quantity;
        public double TotalValuePurchaseOrderCurrency => UnitaryValuePurchaseOrderCurrency * Quantity;

        public double Budget { get; set; }
        public double PriorAssigned { get; set; }
        public double CurrentActual { get; set; }
        public double NewAssigned => PriorAssigned + TotalValueFromQuoteValueUSD;
        public double CurrentPotential { get; set; }
        public double NewPotential => CurrentPotential + TotalValueFromQuoteValueUSD;
        public double PendingToAssign => Budget - NewAssigned;
        public string NomenclatoreName { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public Guid BudgetItemId {  get; private set; }
        public void SetBudgetItem(BudgetItemApprovedResponse _BudgetItem, double usdcop, double usdeur)
        {
            NomenclatoreName = _BudgetItem.NomenclatoreName;
            Name = _BudgetItem.Name;
            BudgetItemId = _BudgetItem.BudgetItemId;
            Budget = _BudgetItem.BudgetUSD;
            PriorAssigned = _BudgetItem.AssignedUSD;
            CurrentPotential = _BudgetItem.PotentialUSD;
            USDCOP = usdcop;
            USDEUR = usdeur;
            

        }
    }
}
