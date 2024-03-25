using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.Currencies;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.Suppliers;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Xml.Linq;

namespace Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates
{

    public class CreatedRegularPurchaseOrderRequest
    {
        public CreatedRegularPurchaseOrderRequest()
        {

        }
        public string PurchaseorderName { get; set; } = string.Empty;
        public SupplierResponse? Supplier { get; set; }
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
        public string AccountAssignment => IsAlteration ? CostCenter : MWOCECName;
        public string CostCenter { get; set; } = string.Empty;
        public string MWOCECName { get; set; } = string.Empty;
      
        public CurrencyEnum PurchaseOrderCurrency { get; set; } = CurrencyEnum.None;
            
        public Func<Task<bool>> Validator { get; set; } = null!;
        public List<PurchaseOrderItemRequest> PurchaseOrderItemNoBlank => PurchaseOrderItems.Where(x => x.BudgetItemId != Guid.Empty).ToList();
        public List<PurchaseOrderItemRequest> PurchaseOrderItems { get; set; } = new();
     
        public void SetMainBudgetItem(BudgetItemApprovedResponse budgetItem)
        {
            MWOId = budgetItem.MWOId;
            MWOName = budgetItem.MWOName;
            CostCenter = budgetItem.CostCenter;
            MWOCECName = budgetItem.MWOCECName;

            MainBudgetItem = budgetItem;
            AddBudgetItem(budgetItem);
            AddBlankItem();
        }
        public double OldTRMUSDCOP { get; set; }
        public double OldTRMUSDEUR { get; set; }
        public DateTime OldCurrencyDate {  get; set; }

        double _usdcop;
        public  double TRMUSDCOP
        {
            get => _usdcop;
            set
            {
                _usdcop = value;
                foreach (var item in PurchaseOrderItemNoBlank)
                {
                    item.SetUSDCOP(_usdcop);
                }
            }
        }
        public async Task ChangeTRMUSDCOP(string arg)
        {

            if (string.IsNullOrEmpty(arg))
            {
                return;
            }
            double usdcop = _usdcop;
            if (!double.TryParse(arg, out usdcop))
            {

            }
            TRMUSDCOP = usdcop;
            if (Validator != null) await Validator();
        }
        double _usdeur;
        public  double TRMUSDEUR
        {
            get => _usdeur;
            set
            {
                _usdeur = value;
                foreach (var item in PurchaseOrderItemNoBlank)
                {
                    item.SetUSDEUR(_usdeur);
                }
            }
        }
        public DateTime CurrencyDate { get; set; } = DateTime.UtcNow;
        public string CurrencyDateOnly => CurrencyDate.ToShortDateString();
        public async Task ChangeTRMUSDEUR( string arg)
        {

            if (string.IsNullOrEmpty(arg))
            {
                return;
            }
            double usdeur = _usdeur;
            if (!double.TryParse(arg, out usdeur))
            {

            }
            TRMUSDEUR = usdeur;
            if (Validator != null) await Validator();
        }

        CurrencyEnum _QuoteCurrency = CurrencyEnum.COP;
        public CurrencyEnum QuoteCurrency
        {
            get => _QuoteCurrency;
            set
            {
                _QuoteCurrency = value;
                ChangeQuoteCurrency(value);

            }
        }
        
        
        public async Task ChangeName(string name)
        {

            PurchaseorderName = name;
            if (PurchaseOrderItemNoBlank.Count == 1)
            {
                PurchaseOrderItems[0].Name = PurchaseorderName;
            }
            if (Validator != null) await Validator();
        }
        public async Task ChangeName(PurchaseOrderItemRequest model, string name)
        {

            model.Name = name;

            if (PurchaseOrderItemNoBlank.Count == 1)
            {
                PurchaseorderName = name;
            }
            if (Validator != null) await Validator();
        }
        public void ChangeQuoteCurrency(CurrencyEnum currencyEnum)
        {

            foreach (var item in PurchaseOrderItems)
            {
                item.ChangeCurrency(currencyEnum);
            }

        }

        public async Task SetSupplier(SupplierResponse? _Supplier)
        {

            if (_Supplier == null)
            {
                PurchaseOrderCurrency = CurrencyEnum.COP;
                return;
            }
            Supplier = _Supplier;


            if (Validator != null) await Validator();
            PurchaseOrderCurrency = _Supplier.SupplierCurrency;
        }

       

        public void AddMWOBudgetItem(BudgetItemApprovedResponse budgetItem)
        {


            AddBudgetItem(budgetItem);
            AddBlankItem();
        }

        public void AddBudgetItem(BudgetItemApprovedResponse response)
        {
            if (PurchaseOrderItems.Count == 0)
                PurchaseOrderItems.Add(new PurchaseOrderItemRequest());

            PurchaseOrderItems[PurchaseOrderItems.Count - 1].SetBudgetItem(response, TRMUSDCOP, TRMUSDEUR);


        }
        public void AddBlankItem()
        {
            if (!PurchaseOrderItems.Any(x => x.BudgetItemId == Guid.Empty))
            {
                PurchaseOrderItems.Add(new PurchaseOrderItemRequest());
            }


        }
       
        public  double SumPOValueUSD => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.POItemValueUSD), 2);
        public  double SumPOValueCurrency => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.POItemCurrencyValue), 2);
        public  double SumBudget => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.Budget), 2);
        public  double SumBudgetAssigned => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.BudgetAssigned), 2);
        public  double SumBudgetPotencial => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.BudgetPotencial), 2);
        public  double SumPendingUSD => PurchaseOrderItemNoBlank.Count == 0 ? 0 : Math.Round(PurchaseOrderItemNoBlank.Sum(x => x.POItemPendingUSD),2);
        public bool IsAnyValueNotDefined => PurchaseOrderItemNoBlank.Any(x => x.CurrencyUnitaryValue <= 0);

        public async Task ChangeCurrency(PurchaseOrderItemRequest item, CurrencyEnum newCurrency)
        {

            double originalValueInUsd = item.UnitaryCostInUSD;
            if (newCurrency.Id == CurrencyEnum.COP.Id)
            {
                item.CurrencyUnitaryValue = originalValueInUsd * TRMUSDCOP;
            }
            else if (newCurrency.Id == CurrencyEnum.EUR.Id)
            {
                item.CurrencyUnitaryValue = originalValueInUsd * TRMUSDEUR;
            }
            else if (newCurrency.Id == CurrencyEnum.USD.Id)
            {
                item.CurrencyUnitaryValue = originalValueInUsd;
            }
            _QuoteCurrency = newCurrency;
            if (Validator != null) await Validator();
        }
        public async Task ChangeQuantity(PurchaseOrderItemRequest item, string arg)
        {

            if (string.IsNullOrEmpty(arg))
            {
                return;
            }
            double quantity = item.Quantity;
            if (!double.TryParse(arg, out quantity))
            {

            }
            item.Quantity = quantity;
            if (Validator != null) await Validator();
        }
        public async Task ChangeCurrencyValue(PurchaseOrderItemRequest item, string arg)
        {

            if (string.IsNullOrEmpty(arg))
            {
                return;
            }
            double currencyvalue = item.Quantity;
            if (!double.TryParse(arg, out currencyvalue))
            {

            }
            item.CurrencyUnitaryValue = currencyvalue;
            if (Validator != null) await Validator();
        }
        public async Task ChangePR(string purchaserequisition)
        {
            PurchaseRequisition = purchaserequisition;
            if (Validator != null) await Validator();
        }
        public async Task ChangeQuote(string quoteno)
        {
            QuoteNo = quoteno;
            if (Validator != null) await Validator();
        }
    }

}
