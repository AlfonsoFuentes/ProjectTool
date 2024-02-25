using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.Currencies;
using Shared.Models.MWO;
using Shared.Models.Suppliers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models.PurchaseOrders.Requests.Create
{

    public class CreatePurchaseOrderRequest
    {
        public CreatePurchaseOrderRequest()
        {

        }


        public Guid MainBudgetItemId { get; set; }

        public Guid SupplierId { get; set; }
        public List<CreatePurchaseOrderItemRequest> ItemsToCreate => ItemsForm.Where(x => x.BudgetItemId != Guid.Empty).ToList();
        public List<CreatePurchaseOrderItemRequest> ItemsForm { get; set; } = new();
        public string Name { get; set; } = string.Empty;
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
        public List<string> ValidationErrors { get; set; } = new();

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
            Name = name;
            if (ItemsToCreate.Count == 1)
            {
                ItemsForm[0].Name = Name;
            }

        }
        public void ChangeName(CreatePurchaseOrderItemRequest model, string name)
        {
            ValidationErrors.Clear();
            model.Name = name;

            if (ItemsToCreate.Count == 1)
            {
                Name = name;
            }

        }
        public void ChangeQuoteCurrency(CurrencyEnum currencyEnum)
        {
            ValidationErrors.Clear();
            foreach (var item in ItemsForm)
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

        public void SetMWOBudgetItem(MWOResponse mWO, BudgetItemResponse budgetItem)
        {
            MWOId = mWO.Id;
            IsAlteration = budgetItem.Type.Id == BudgetItemTypeEnum.Alterations.Id;
            AssetRealProductive = mWO.IsRealProductive;
            AccountAssigment = IsAlteration ? mWO.CostCenter : mWO.CECName;
            MWOCECName = mWO.CECName;
            AddBudgetItem(budgetItem);
            AddBlankItem();
        }

        public void AddBudgetItem(BudgetItemResponse response)
        {
            if (ItemsForm.Count == 0)
                ItemsForm.Add(new CreatePurchaseOrderItemRequest());

            ItemsForm[ItemsForm.Count - 1].SetBudgetItem(response, USDCOP, USDEUR);


        }
        public void AddBlankItem()
        {
            if (!ItemsForm.Any(x => x.BudgetItemId == Guid.Empty))
            {
                ItemsForm.Add(new CreatePurchaseOrderItemRequest());
            }


        }
        public double SumPOValueUSD => ItemsToCreate.Count == 0 ? 0 : ItemsToCreate.Sum(x => x.TotalValueUSDItem);
        public double SumPOValueCurrency => ItemsToCreate.Count == 0 ? 0 : ItemsToCreate.Sum(x => x.TotalCurrencyValue);
        public double SumBudget => ItemsToCreate.Count == 0 ? 0 : ItemsToCreate.Sum(x => x.Budget);
        public double SumBudgetAssigned => ItemsToCreate.Count == 0 ? 0 : ItemsToCreate.Sum(x => x.BudgetAssigned);
        public double SumBudgetPotencialAssigned => ItemsToCreate.Count == 0 ? 0 : ItemsToCreate.Sum(x => x.BudgetPotencialAssigned);
        public double SumPendingUSD => ItemsToCreate.Count == 0 ? 0 : ItemsToCreate.Sum(x => x.Pending);

    }

}
