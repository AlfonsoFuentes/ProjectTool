using Shared.Commons.Results;
using Shared.ExtensionsMetods;
using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.PurchaseOrders.Base;
using Shared.NewModels.PurchaseOrders.Request;
#nullable disable
namespace ClientRadzen.NewPages.NewPurchaseOrder.Salaries;
public partial class NewPurchaseOrderEditSalaryPage
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public Guid PurchaseOrderId { get; set; }
    [Inject]
    private INewPurchaseOrderService Service { get; set; }

    [Inject]
    private INewBudgetItemService BudgetItemService { get; set; }

    public NewPurchaseOrderEditSalaryRequest Model = null;

    FluentValidationValidator _fluentValidationValidator = null!;
    protected override async Task OnInitializedAsync()
    {
        var USDCOP = MainApp.RateList == null ? 4000 : Math.Round(MainApp.RateList.COP, 2);
        var USDEUR = MainApp.RateList == null ? 1 : Math.Round(MainApp.RateList.EUR, 2);
        var resultpurchaseOrder = await Service.GetPurchaseOrderToEditSalary(PurchaseOrderId);
        if (resultpurchaseOrder.Succeeded)
        {
            Model = resultpurchaseOrder.Data;
        }
       
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
        var result = await Service.EditSalaryPurchaseOrderAsync(Model);
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


    public async Task ChangeName(string name)
    {
        Model.PurchaseOrder.SetPurchaseOrderName(name);

        await ValidateAsync();
        StateHasChanged();
    }

    public async Task ChangePONumber(string ponumber)
    {

        Model.PurchaseOrder.PurchaseOrderNumber = ponumber.Trim();
        await ValidateAsync();

    }
    public async Task ChangeCurrencyValue(double currencyvalue)
    {
        Model.EditPurchaseOrderSalary(currencyvalue);
        await ValidateAsync();
        StateHasChanged();
    }




}
