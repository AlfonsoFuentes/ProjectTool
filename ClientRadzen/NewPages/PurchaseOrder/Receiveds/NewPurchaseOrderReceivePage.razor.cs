using Shared.Enums.WayToReceivePurchaseOrdersEnums;
using Shared.ExtensionsMetods;
using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.PurchaseOrders.Request;

namespace ClientRadzen.NewPages.PurchaseOrder.Receiveds;
#nullable disable
public partial class NewPurchaseOrderReceivePage
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public Guid PurchaseOrderId { get; set; }
    [Inject]
    private INewPurchaseOrderService Service { get; set; }
    [Inject]
    private INewSupplierService SupplierService { get; set; }



    public NewPurchaseOrderReceiveRequest Model = null;


    List<NewSupplierResponse> Suppliers { get; set; } = new();

    FluentValidationValidator _fluentValidationValidator = null!;


    NewBudgetItemToCreatePurchaseOrderResponse ItemToAdd;
    double USDCOP, USDEUR;

    protected override async Task OnInitializedAsync()
    {
        var resultSupplier = await SupplierService.GetAllSupplier();
        if (resultSupplier.Succeeded)
        {
            Suppliers = resultSupplier.Data.Suppliers;

        }
        USDCOP = MainApp.RateList == null ? 4000 : Math.Round(MainApp.RateList.COP, 2);
        USDEUR = MainApp.RateList == null ? 1 : Math.Round(MainApp.RateList.EUR, 2);
        var result = await Service.GetPurchaseOrderToReceive(PurchaseOrderId);
        if (result.Succeeded)
        {
            Model = result.Data;
            InitializePurchaseOrderItems();


        }

        StateHasChanged();
    }
    void InitializePurchaseOrderItems()
    {
        foreach (var item in Model.PurchaseOrderItems)
        {
            item.ReceiveUSDCOP = USDCOP;
            item.ReceiveUSDEUR = USDEUR;

        }
    }


    async Task<bool> ValidateAsync()
    {
        Validated = await _fluentValidationValidator.ValidateAsync();
        return Validated;
    }
    bool Validated = false;
    public async Task SaveAsync()
    {
        var result = await Service.ReceivePurchaseOrderAsync(Model);
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
    bool debug = true;
    void Cancel()
    {
        Navigation.NavigateBack();
    }
    public async Task ChangeReceiving(NewPurchaseOrderReceiveItemRequest item, string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double receiving = arg.ToDouble();
        item.ReceivingCurrency = receiving;
        await ValidateAsync();
        StateHasChanged();
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
            PercentageToReceive = Math.Round(Model.PendingCurrency / Model.POValueCurrency * 100, 2);
            foreach (var row in Model.PurchaseOrderItems)
            {
                row.ReceivingCurrency = row.PendingCurrency;



            }
        }
        await ValidateAsync();

    }
    void ClearValues()
    {
        foreach (var row in Model.PurchaseOrderItems)
        {
            row.ReceivingCurrency = 0;


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
                row.ReceivingCurrency = row.ItemQuoteValueCurrency * PercentageToReceive / 100.0;
            }
        }


        await ValidateAsync();
    }
}
