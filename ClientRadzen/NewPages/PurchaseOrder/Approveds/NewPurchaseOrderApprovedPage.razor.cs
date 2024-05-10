using Shared.Enums.BudgetItemTypes;
using Shared.Enums.Currencies;
using Shared.ExtensionsMetods;
using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.PurchaseOrders.Request;

namespace ClientRadzen.NewPages.PurchaseOrder.Approveds;
#nullable disable
public partial class NewPurchaseOrderApprovedPage
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public Guid PurchaseOrderId { get; set; }
    [Inject]
    private INewPurchaseOrderService Service { get; set; }
    [Inject]
    private INewSupplierService SupplierService { get; set; }
    //[Inject]
    //private INewBudgetItemService BudgetItemService { get; set; }


    public NewPurchaseOrderApproveRequest Model = null;


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
        var result = await Service.GetPurchaseOrderToApproved(PurchaseOrderId);
        if (result.Succeeded)
        {
            Model = result.Data;
            Model.USDCOP= USDCOP;
            Model.USDEUR= USDEUR;
            



        }
        //var resultBudgetItems = await BudgetItemService.GetAllMWOApprovedForCreatePurchaseOrder(Model.MWOId);
        //if (resultBudgetItems.Succeeded)
        //{

        //    OriginalBudgetItems = resultBudgetItems.Data.BudgetItems;
        //}
        //InitializeBudgetItems();
        //InitializePurchaseOrder();
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
        var result = await Service.ApprovePurchaseOrderAsync(Model);
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
    
    bool debug = true;


    public async Task ChangePONumber(string ponumber)
    {

        Model.PurchaseOrderNumber = ponumber.Trim();
        await ValidateAsync();

    }
    public async Task ChangedExpectedDate(DateTime? expected)
    {
        Model.ExpectedDate = expected;
        await ValidateAsync();
    }
    public List<NewBudgetItemToCreatePurchaseOrderResponse> OriginalBudgetItems { get; set; } = new();
    public List<NewBudgetItemToCreatePurchaseOrderResponse> BudgetItems { get; set; } = new();
    public List<NewPurchaseOrderCreateItemRequest> PurchaseOrderItems => Model.PurchaseOrderItems;

  

}
