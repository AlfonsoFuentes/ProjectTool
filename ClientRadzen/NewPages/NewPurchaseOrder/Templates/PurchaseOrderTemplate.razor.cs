using Shared.Commons.Results;
using Shared.Enums.BudgetItemTypes;
using Shared.Enums.Currencies;
using Shared.Enums.WayToReceivePurchaseOrdersEnums;
using Shared.ExtensionsMetods;
using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.PurchaseOrders.Base;
#nullable disable
namespace ClientRadzen.NewPages.NewPurchaseOrder.Templates;
public partial class PurchaseOrderTemplate
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public NewPurchaseOrderRequest Model { get; set; } = null!;
    [Parameter]
    public EventCallback<NewPurchaseOrderRequest> ModelChanged { get; set; }
    [Parameter]

    public List<NewSupplierResponse> Suppliers { get; set; } = new();
    [Parameter]

    public EventCallback<List<NewSupplierResponse>> SuppliersChanged { get; set; }
    [Parameter]
    [EditorRequired]
    public Func<Task<IResult>> OnSaveAsync { get; set; } = null!;
    [Parameter]
    [EditorRequired]
    public Func<Task<IResult<NewSupplierListResponse>>> OnGetSuppliers { get; set; } = null!;
    [Parameter]
    [EditorRequired]
    public List<NewBudgetItemMWOApprovedResponse> OriginalBudgetItems { get; set; } = new();
    [Parameter]
    [EditorRequired]
    public PurchaseOrderTemplateAction PurchaseOrderAction { get; set; }
    [Parameter]
    [EditorRequired]
    public Func<Task<bool>> OnValidateAsync { get; set; } = null!;
    [Parameter]
    [EditorRequired]
    public FluentValidationValidator _fluentValidationValidator { get; set; } = null!;

    string NameSaveButton =>
        PurchaseOrderAction == PurchaseOrderTemplateAction.Create ? $"Create {Model.PurchaseRequisition}" :
        PurchaseOrderAction == PurchaseOrderTemplateAction.EditCreated ? $"Edit {Model.PurchaseRequisition}" :
        PurchaseOrderAction == PurchaseOrderTemplateAction.Approve ? $"Approve {Model.PurchaseOrderNumber}" :
        PurchaseOrderAction == PurchaseOrderTemplateAction.EditApproved ? $"Edit {Model.PurchaseOrderNumber}" :
        PurchaseOrderAction == PurchaseOrderTemplateAction.Receive ? $"Receive {Model.PurchaseOrderNumber}" :
        PurchaseOrderAction == PurchaseOrderTemplateAction.EditReceiving ? $"Edit {Model.PurchaseOrderNumber}" :
        PurchaseOrderAction == PurchaseOrderTemplateAction.EditClosed ? $"Edit {Model.PurchaseOrderNumber}" :
        string.Empty;

    bool ShowPurchaseOrderNumber =>
        PurchaseOrderAction == PurchaseOrderTemplateAction.Approve ||
        PurchaseOrderAction == PurchaseOrderTemplateAction.EditApproved ||
        PurchaseOrderAction == PurchaseOrderTemplateAction.Receive ||
        PurchaseOrderAction == PurchaseOrderTemplateAction.EditReceiving ||
        PurchaseOrderAction == PurchaseOrderTemplateAction.EditClosed;
    bool ShowReceive => PurchaseOrderAction == PurchaseOrderTemplateAction.Receive;
    bool ShowEditReceive => PurchaseOrderAction == PurchaseOrderTemplateAction.EditReceiving ||
        PurchaseOrderAction == PurchaseOrderTemplateAction.EditClosed;

    bool IsNotAbleToAddBudgetItem => PurchaseOrderAction == PurchaseOrderTemplateAction.Receive ||
        PurchaseOrderAction == PurchaseOrderTemplateAction.EditReceiving ||
        PurchaseOrderAction == PurchaseOrderTemplateAction.EditClosed;

    public bool IsNotAbleToEditMainData => !(
          PurchaseOrderAction == PurchaseOrderTemplateAction.Create ||
          PurchaseOrderAction == PurchaseOrderTemplateAction.EditCreated ||
         PurchaseOrderAction == PurchaseOrderTemplateAction.Approve ||
         PurchaseOrderAction == PurchaseOrderTemplateAction.EditApproved);
    protected override void OnParametersSet()
    {
        InitializeBudgetItems();
        InitializePurchaseOrder();
    }

    bool Validated = false;
    public async Task ValidateAsync()
    {
        Validated = await OnValidateAsync.Invoke();
        StateHasChanged();
    }
    public async Task SaveAsync()
    {
        if (OnSaveAsync != null)
        {
            await ModelChanged.InvokeAsync(Model);

            var result = await OnSaveAsync.Invoke();
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                await ModelChanged.InvokeAsync(Model);
                Cancel();
            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);


            }
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
            if (OnGetSuppliers != null)
            {
                var resultData = await OnGetSuppliers.Invoke();
                if (resultData.Succeeded)
                {
                    Suppliers = resultData.Data.Suppliers;
                    await SuppliersChanged.InvokeAsync(Suppliers);
                }
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
        await ModelChanged.InvokeAsync(Model);
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

        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    public async Task ChangeName(string name)
    {
        Model.SetPurchaseOrderName(name);

        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    public async Task ChangeName(NewPurchaseOrderItemRequest model, string name)
    {

        Model.SetPurchaseOrderItemName(model, name);

        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    public async Task ChangeQuoteCurrency(CurrencyEnum currencyEnum)
    {
        Model.SetQuoteCurrency(currencyEnum);
        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    public async Task SetSupplier(NewSupplierResponse _Supplier)
    {

        if (_Supplier == null)
        {
            Model.PurchaseOrderCurrency = CurrencyEnum.COP;
            return;
        }
        Model.SetSupplier(_Supplier);
        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();

    }
    public async Task ChangeQuantity(NewPurchaseOrderItemRequest item, string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double quantity = arg.ToDouble();
        item.Quantity = quantity;
        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    public async Task ChangeCurrencyValue(NewPurchaseOrderItemRequest item, double arg)
    {


        double currencyvalue = arg;

        item.UnitaryValueQuoteCurrency = currencyvalue;
        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    public async Task ChangePR(string purchaserequisition)
    {
        Model.PurchaseRequisition = purchaserequisition;
        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    public async Task ChangeQuote(string quoteno)
    {
        Model.QuoteNo = quoteno;
        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    public async Task ChangeReceivingValueCurrency(NewPurchaseOrderItemRequest item, string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double receiving = arg.ToDouble();
        item.ReceivingValue.ReceivingValueCurrency = receiving;
        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    public async Task ChangeReceivedValueCurrency(NewPurchaseOrderItemReceivedRequest item, string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double receiving = arg.ToDouble();
        item.ValueReceivedCurrency = receiving;
        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    public async Task ChangeReceivingTRMUSDCOP(NewPurchaseOrderItemRequest item, string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double usdcop = arg.ToDouble();
        item.ReceivingValue.USDCOP = usdcop;
        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    public async Task ChangeReceivedTRMUSDCOP(NewPurchaseOrderItemReceivedRequest item, string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double usdcop = arg.ToDouble();
        item.USDCOP = usdcop;
        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    public async Task ChangeReceivingTRMUSDEUR(NewPurchaseOrderItemRequest item, string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double usdeur = arg.ToDouble();
        item.ReceivingValue.USDEUR = usdeur;
        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    public async Task ChangeReceivedTRMUSDEUR(NewPurchaseOrderItemReceivedRequest item, string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double usdeur = arg.ToDouble();
        item.USDEUR = usdeur;
        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    public List<NewBudgetItemMWOApprovedResponse> BudgetItems { get; set; } = new();
    public List<NewPurchaseOrderItemRequest> PurchaseOrderItems { get; set; } = new();
    public List<NewPurchaseOrderItemReceivedRequest> PurchaseOrderItemReceiveds { get; set; } = new();
    public async Task AddBudgetItemToPurchaseOrder(NewBudgetItemMWOApprovedResponse budgetitem)
    {

        if (Model.AddBudgetItem(budgetitem))
        {
            InitializeBudgetItems();
            InitializePurchaseOrder();
        }


        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    public async Task RemovedBudgetItemToPurchaseOrder(NewPurchaseOrderItemRequest Item)
    {
        if (Model.RemoveBudgetItem(Item))
        {
            InitializeBudgetItems();
            InitializePurchaseOrder();
        }


        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
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


        if (Model.MainBudgetItem != null && Model.PurchaseOrderItems.Any(x => x.BudgetItemId == Model.MainBudgetItem.BudgetItemId))
        {
            PurchaseOrderItems.Add(Model.PurchaseOrderItems.Single(x => x.BudgetItemId == Model.MainBudgetItem.BudgetItemId));

        }


        foreach (var item in Model.PurchaseOrderItems)
        {
            if (!PurchaseOrderItems.Any(x => x.BudgetItemId == item.BudgetItemId))
            {
                PurchaseOrderItems.Add(item);

            }
            if (BudgetItems.Any(x => x.BudgetItemId == item.BudgetItemId))
            {
                var budgetitem = BudgetItems.Single(x => x.BudgetItemId == item.BudgetItemId);
                BudgetItems.Remove(budgetitem);

            }
        }
        foreach(var item in Model.PurchaseOrderItems)
        {
            foreach(var received in item.PurchaseOrderReceiveds)
            {
                PurchaseOrderItemReceiveds.Add(received);
            }
        }
        if (IsNotAbleToAddBudgetItem) return;
        if (BudgetItems.Count > 0)
        {
            PurchaseOrderItems.Add(new());
        }
    }
    public async Task ChangePONumber(string ponumber)
    {

        Model.PurchaseOrderNumber = ponumber.Trim();
        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();

    }
    public async Task ChangedExpectedDate(DateTime? expected)
    {
        Model.ExpectedDate = expected;
        await ModelChanged.InvokeAsync(Model);
        await ValidateAsync();
    }
    double PercentageToReceive { get; set; }
    WayToReceivePurchaseorderEnum WayToReceivePurchaseOrder { get; set; } = WayToReceivePurchaseorderEnum.None;
    public async Task OnChangeWayToReceivePurchaseOrder(WayToReceivePurchaseorderEnum wayToReceivePurchaseOrder)
    {



        if (WayToReceivePurchaseOrder.Id != wayToReceivePurchaseOrder.Id)
        {
            WayToReceivePurchaseOrder = wayToReceivePurchaseOrder;
            ClearValues();
        }
        if (WayToReceivePurchaseOrder.Id == WayToReceivePurchaseorderEnum.CompleteOrder.Id)
        {
            PercentageToReceive = Math.Round(Model.POCommitmentCurrency / Model.POValuePurchaseOrderCurrency * 100, 2);
            foreach (var row in Model.PurchaseOrderItems)
            {
                row.ReceivingValue.ReceivingValueCurrency = row.POItemCommitmentCurrency;



            }
        }
        await ValidateAsync();

    }
    void ClearValues()
    {
        foreach (var row in Model.PurchaseOrderItems)
        {
            row.ReceivingValue.ReceivingValueCurrency = 0;


        }
    }
    public async Task OnChangeReceivePercentagePurchaseOrder(string percentage)
    {

        double newpercentage = percentage.ToDouble();


        if (!(newpercentage < 0 || newpercentage > 100))
        {
            PercentageToReceive = newpercentage;
            if (newpercentage > Model.MaxPercentageToReceive)
            {
                PercentageToReceive = Model.MaxPercentageToReceive;
            }

            foreach (var row in Model.PurchaseOrderItems)
            {
                row.ReceivingValue.ReceivingValueCurrency = row.POItemValuePurchaseOrderCurrency * PercentageToReceive / 100.0;
            }
        }


        await ValidateAsync();
    }
}
public enum PurchaseOrderTemplateAction
{
    Create,
    EditCreated,
    Approve,
    EditApproved,
    Receive,
    EditReceiving,
    EditClosed
}
