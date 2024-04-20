using Blazored.FluentValidation;
using Client.Infrastructure.Managers.BudgetItems;
using Client.Infrastructure.Managers.CurrencyApis;
using Client.Infrastructure.Managers.PurchaseOrders;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseOrders.Requests.Taxes;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace ClientRadzen.Pages.PurchaseOrders;
public partial class EditCapitalizedSalary
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public Guid PurchaseOrderId { get; set; }

    [Inject]
    private IPurchaseOrderService Service { get; set; }
    [Inject] public IRate _CurrencyService { get; set; }
    



    EditCapitalizedSalaryPurchaseOrderRequest Model { get; set; } = new();
    ConversionRate RateList { get; set; }
    DateOnly CurrentDate => DateOnly.FromDateTime(DateTime.UtcNow);



    protected override async Task OnInitializedAsync()
    {

        RateList = await _CurrencyService.GetRates();
        var result = await Service.GetPurchaseOrderCapitalizedSalarToEdit(PurchaseOrderId);
        if (result.Succeeded)
        {


            Model = result.Data;


        }



    }
    async Task<bool> ValidateAsync()
    {
        Validated = await _fluentValidationValidator.ValidateAsync();
        return Validated;
    }
    bool Validated = false;

    FluentValidationValidator _fluentValidationValidator = null!;
    public async Task SaveAsync()
    {
        var result = await Service.EditPurchaseOrderCapitalizedSalary(Model);
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
        item.QuoteCurrencyValue = currencyvalue;
        item.ActualCurrency = currencyvalue;

        await ValidateAsync();
    }
    public async Task ChangeName(string name)
    {

        Model.PurchaseOrderName = name;
        Model.PurchaseOrderItem.Name = name;
        await ValidateAsync();
    }
    public async Task ChangePurchaseorderNumber(string ponumber)
    {

        Model.PurchaseorderNumber = ponumber;
        await ValidateAsync();

    }



}
