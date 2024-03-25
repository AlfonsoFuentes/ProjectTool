using Blazored.FluentValidation;
using Client.Infrastructure.Managers.CurrencyApis;
using Client.Infrastructure.Managers.PurchaseOrders;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;
using Shared.Models.PurchaseOrders.Requests.Taxes;
#nullable disable
namespace ClientRadzen.Pages.PurchaseOrders;
public partial class EditTaxPurchaseOrder
{
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
    public async Task SaveAsync()
    {
        if (await _fluentValidationValidator.ValidateAsync())
        {
            var result = await Service.EditPurchaseOrderTax(Model);
            if (result.Succeeded)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Success",
                    Detail = result.Message,
                    Duration = 4000
                });

            Cancel();
            }
            else
            {
                Model.ValidationErrors = result.Messages;
                StateHasChanged();
            }
        }

    }
    void Cancel()
    {
        Navigation.NavigateBack();
    }


}
