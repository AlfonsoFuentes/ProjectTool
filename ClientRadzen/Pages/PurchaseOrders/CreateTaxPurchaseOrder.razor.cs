#nullable disable
using Azure;
using Blazored.FluentValidation;
using Client.Infrastructure.Managers.BudgetItems;
using Client.Infrastructure.Managers.CurrencyApis;
using Client.Infrastructure.Managers.PurchaseOrders;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.PurchaseOrders.Requests.Taxes;
using Shared.Models.PurchaseOrders.Responses;

namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class CreateTaxPurchaseOrder
    {
        [Parameter]
        public Guid BudgetItemId { get; set; }

        [Inject]
        private IPurchaseOrderService Service { get; set; }
        [Inject] public IRate _CurrencyService { get; set; }
        [Inject]
        private IBudgetItemService BudgetItemService { get; set; }

    

        CreateTaxPurchaseOrderRequest Model { get; set; } = new();
        ConversionRate RateList { get; set; }
        DateOnly CurrentDate => DateOnly.FromDateTime(DateTime.UtcNow);



        protected override async Task OnInitializedAsync()
        {

            RateList = await _CurrencyService.GetRates();
            var result = await BudgetItemService.GetApprovedBudgetItemsById(BudgetItemId);
            if (result.Succeeded)
            {
               

                Model = new();
                
                Model.USDCOP = RateList == null ? 4000 : Math.Round(RateList.COP, 2);
                Model.USDEUR = RateList == null ? 1 : Math.Round(RateList.EUR, 2);

                Model.SetMainBudgetItem(result.Data);



            }
            


        }
        FluentValidationValidator _fluentValidationValidator = null!;
        public async Task SaveAsync()
        {
            if (await _fluentValidationValidator.ValidateAsync())
            {
                var result = await Service.CreateTaxPurchaseOrder(Model);
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
}
