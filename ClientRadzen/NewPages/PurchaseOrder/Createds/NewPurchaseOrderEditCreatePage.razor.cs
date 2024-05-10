using Shared.Enums.BudgetItemTypes;
using Shared.Enums.Currencies;
using Shared.ExtensionsMetods;
using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.PurchaseOrders.Request;

namespace ClientRadzen.NewPages.PurchaseOrder.Createds;
#nullable disable
public partial class NewPurchaseOrderEditCreatePage
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public Guid PurchaseOrderId { get; set; }
    [Inject]
    private INewPurchaseOrderService Service { get; set; }
    [Inject]
    private INewSupplierService SupplierService { get; set; }
    [Inject]
    private INewBudgetItemService BudgetItemService { get; set; }


    public NewPurchaseOrderCreateEditRequest Model = null;


    List<NewSupplierResponse> Suppliers { get; set; } = new();

    FluentValidationValidator _fluentValidationValidator = null!;


    NewBudgetItemToCreatePurchaseOrderResponse ItemToAdd;


    protected override async Task OnInitializedAsync()
    {
        var resultSupplier = await SupplierService.GetAllSupplier();
        if (resultSupplier.Succeeded)
        {
            Suppliers = resultSupplier.Data.Suppliers;

        }
        var USDCOP = MainApp.RateList == null ? 4000 : Math.Round(MainApp.RateList.COP, 2);
        var USDEUR = MainApp.RateList == null ? 1 : Math.Round(MainApp.RateList.EUR, 2);
      
        var resultPurchaseOrder = await Service.GetPurchaseOrderCreatedToEdit(PurchaseOrderId);
        if(resultPurchaseOrder.Succeeded)
        {
            Model=resultPurchaseOrder.Data;
          
        }
        OldCurrencyDate = Model.CurrencyDate;
        OldTRMUSDCOP = Model.USDCOP;
        OldTRMUSDEUR = Model.USDEUR;
        var resultBudgetItems = await BudgetItemService.GetAllMWOApprovedForCreatePurchaseOrder(Model.MWOId);
        if (resultBudgetItems.Succeeded)
        {

            OriginalBudgetItems = resultBudgetItems.Data.BudgetItems;
        }
        InitializeBudgetItems();
        InitializePurchaseOrder();
        StateHasChanged();
    }


    async Task<bool> ValidateAsync()
    {
        Validated = await _fluentValidationValidator.ValidateAsync();
        return Validated;
    }
    bool Validated = false;
    public async Task SaveAsync()
    {
        var result = await Service.EditCreatedPurchaseOrderAsync(Model);
        if (result.Succeeded)
        {
            MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

            Cancel();
        }
        else
        {
            MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);


        }

    }

    void Cancel()
    {
        Navigation.NavigateBack();
    }
    async Task CreateSupplier()
    {
        var result = await DialogService.OpenAsync<NewCreateSupplierDialog>($"Create Supplier",
            new Dictionary<string, object>() { },
            new DialogOptions() { Width = "400px", Height = "450px", Resizable = true, Draggable = true });
        if (result != null)
        {

            await SetSupplier(result as NewSupplierResponse);
            var resultData = await SupplierService.GetAllSupplier();
            if (resultData.Succeeded)
            {
                Suppliers = resultData.Data.Suppliers;

            }


        }
    }


    public async Task ChangeTRMUSDEUR(string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double usdeur = arg.ToDouble();
        Model.SetTRM(Model.USDEUR, usdeur, DateTime.UtcNow);
        await ValidateAsync();
    }
    public async Task ChangeTRMUSDCOP(string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double usdcop = arg.ToDouble();
        Model.SetTRM(usdcop, Model.USDEUR, DateTime.UtcNow);

        await ValidateAsync();
    }

    public async Task ChangeName(string name)
    {
        Model.SetPurchaseOrderName(name);

        await ValidateAsync();
        StateHasChanged();
    }
    public async Task ChangeName(NewPurchaseOrderCreateItemRequest model, string name)
    {

        Model.SetPurchaseOrderItemName(model, name);

        await ValidateAsync();
        StateHasChanged();
    }
    public async Task ChangeQuoteCurrency(CurrencyEnum currencyEnum)
    {
        Model.SetQuoteCurrency(currencyEnum);
        await ValidateAsync();
        StateHasChanged();
    }

    public async Task SetSupplier(NewSupplierResponse _Supplier)
    {

        if (_Supplier == null)
        {
            Model.PurchaseOrderCurrency = CurrencyEnum.COP;
            return;
        }
        Model.SetSupplier(_Supplier);
        await ValidateAsync();
        StateHasChanged();

    }



    public async Task ChangeQuantity(NewPurchaseOrderCreateItemRequest item, string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double quantity = arg.ToDouble();
        item.Quantity = quantity;
        await ValidateAsync();
        StateHasChanged();
    }
    public async Task ChangeCurrencyValue(NewPurchaseOrderCreateItemRequest item, double arg)
    {


        double currencyvalue = arg;

        item.UnitaryValueCurrency = currencyvalue;
        StateHasChanged();
        await ValidateAsync();
        StateHasChanged();
    }
    public async Task ChangePR(string purchaserequisition)
    {
        Model.PurchaseRequisition = purchaserequisition;
        await ValidateAsync();
        StateHasChanged();
    }
    public async Task ChangeQuote(string quoteno)
    {
        Model.QuoteNo = quoteno;
        await ValidateAsync();
        StateHasChanged();
    }
    bool debug = true;


    int MaxColumn = 12;

    public List<NewBudgetItemToCreatePurchaseOrderResponse> OriginalBudgetItems { get; set; } = new();
    public List<NewBudgetItemToCreatePurchaseOrderResponse> BudgetItems { get; set; } = new();
    public List<NewPurchaseOrderCreateItemRequest> PurchaseOrderItems { get; set; } = new();

    public void AddBudgetItemToPurchaseOrder(NewBudgetItemToCreatePurchaseOrderResponse budgetitem)
    {
        PurchaseOrderItems = new();
        Model.AddBudgetItem(budgetitem);
        foreach (var item in Model.PurchaseOrderItems)
        {
            PurchaseOrderItems.Add(item);
        }
        if (BudgetItems.Any(x => x.BudgetItemId == budgetitem.BudgetItemId))
        {
            BudgetItems.Remove(budgetitem);
        }
        if (BudgetItems.Count > 0)
        {
            PurchaseOrderItems.Add(new());
        }
    }
    public void RemovedBudgetItemToPurchaseOrder(NewPurchaseOrderCreateItemRequest Item)
    {
        PurchaseOrderItems = new();
        if (Model.PurchaseOrderItems.Any(x => x.BudgetItemId == Item.BudgetItemId))
        {
            var item = Model.PurchaseOrderItems.Single(x => x.BudgetItemId == Item.BudgetItemId);
            Model.PurchaseOrderItems.Remove(item);
            InitializeBudgetItems();
        }

        foreach (var item in Model.PurchaseOrderItems)
        {
            PurchaseOrderItems.Add(item);
        }
        if (BudgetItems.Any(x => x.BudgetItemId == Item.BudgetItemId))
        {
            var budgetitem = BudgetItems.Single(x => x.BudgetItemId == Item.BudgetItemId);
            BudgetItems.Remove(budgetitem);
        }
        if (BudgetItems.Count > 0)
        {
            PurchaseOrderItems.Add(new());
        }
    }
    void InitializeBudgetItems()
    {
        BudgetItems = new();

        if (Model.IsAlteration)
        {
            BudgetItems = OriginalBudgetItems.Where(x => x.IsAlteration).ToList();
        }
        else if (!Model.IsCapitalizedSalary)
        {
            BudgetItems = OriginalBudgetItems.Where(x => x.IsAlteration == false).ToList();

            if (BudgetItems.Any(x => x.IsMainItemTaxesNoProductive))
            {
                var budget = BudgetItems.Single(x => x.IsMainItemTaxesNoProductive);
                BudgetItems.Remove(budget);
            }
            if (BudgetItems.Any(x => x.Type.Id == BudgetItemTypeEnum.Engineering.Id && x.IsEngineeringItem))
            {
                var budget = BudgetItems.Single(x => x.Type.Id == BudgetItemTypeEnum.Engineering.Id && x.IsEngineeringItem);
                BudgetItems.Remove(budget);
            }
        }
    }
    void InitializePurchaseOrder()
    {
        PurchaseOrderItems = new();
        foreach (var item in Model.PurchaseOrderItems)
        {
            PurchaseOrderItems.Add(item);
            if (BudgetItems.Any(x => x.BudgetItemId == item.BudgetItemId))
            {
                var budgetitem = BudgetItems.Single(x => x.BudgetItemId == item.BudgetItemId);
                BudgetItems.Remove(budgetitem);

            }
        }

        if (BudgetItems.Count > 0)
        {
            PurchaseOrderItems.Add(new());
        }
    }
    bool UpdateCurrentTRM = false;
    public async Task ClickUpdateCurrentTRM()
    {
        Model.CurrencyDate = DateTime.UtcNow;
        Model.USDCOP = MainApp.RateList.COP;
        Model.USDEUR = MainApp.RateList.EUR;
        UpdateCurrentTRM = true;
        await ValidateAsync();
    }
    double OldTRMUSDCOP, OldTRMUSDEUR;
    DateTime OldCurrencyDate;
    public async Task ClickUpdateOldTRM()
    {


        Model.CurrencyDate = OldCurrencyDate;
        Model.USDCOP = OldTRMUSDCOP;
        Model.USDEUR = OldTRMUSDEUR;
        UpdateCurrentTRM = false;
        await ValidateAsync();
    }

}
