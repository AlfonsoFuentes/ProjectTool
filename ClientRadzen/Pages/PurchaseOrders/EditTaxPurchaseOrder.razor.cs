using Blazored.FluentValidation;
using Client.Infrastructure.Managers.CurrencyApis;
using Client.Infrastructure.Managers.PurchaseOrders;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseOrders.Requests.Taxes;
#nullable disable
namespace ClientRadzen.Pages.PurchaseOrders;
public partial class EditTaxPurchaseOrder
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public Guid PurchaseOrderId { get; set; }

    [Inject]
    private IPurchaseOrderService Service { get; set; }
    [Inject] public IRate _CurrencyService { get; set; }




    EditTaxPurchaseOrderRequest Model { get; set; } = new();
    ConversionRate RateList { get; set; }
    DateOnly CurrentDate => DateOnly.FromDateTime(DateTime.UtcNow);



    protected override async Task OnInitializedAsync()
    {

        RateList = await _CurrencyService.GetRates();
        var result = await Service.GetPurchaseOrderTaxToEdit(PurchaseOrderId);
        if (result.Succeeded)
        {


            Model = result.Data;

            Model.USDCOP = RateList == null ? 4000 : Math.Round(RateList.COP, 2);
            Model.USDEUR = RateList == null ? 1 : Math.Round(RateList.EUR, 2);
        }



    }
    FluentValidationValidator _fluentValidationValidator = null!;
    async Task<bool> ValidateAsync()
    {
        Validated = await _fluentValidationValidator.ValidateAsync();
        return Validated;
    }
    bool Validated = false;
    public async Task SaveAsync()
    {
        var result = await Service.EditPurchaseOrderTax(Model);
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
    async Task ChangeName(string name)
    {

        Model.PurchaseorderName = name;
        Model.PurchaseOrderItem.Name = Model.PurchaseorderName;
        await ValidateAsync();

    }
    async Task ChangePOnumber(string ponumber)
    {

        Model.PONumber = ponumber;
        await ValidateAsync();
    }
    async Task ChangeName(PurchaseOrderItemRequest model, string name)
    {

        model.Name = name;
        Model.PurchaseorderName = name;
        await ValidateAsync();
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
        item.ActualCurrency = currencyvalue;

        await ValidateAsync();
    }

}
