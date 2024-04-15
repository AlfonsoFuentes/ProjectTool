#nullable disable
using Azure;
using Blazored.FluentValidation;
using Client.Infrastructure.Managers.BudgetItems;
using Client.Infrastructure.Managers.CurrencyApis;
using Client.Infrastructure.Managers.PurchaseOrders;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseOrders.Requests.Taxes;
using Shared.Models.PurchaseOrders.Responses;

namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class CreateTaxPurchaseOrder
    {
        [CascadingParameter]
        public App MainApp { get; set; }
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
            var result = await Service.CreateTaxPurchaseOrder(Model);
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

        async Task<bool> ValidateAsync()
        {
            Validated = await _fluentValidationValidator.ValidateAsync();
            return Validated;
        }
        bool Validated = false;

        async Task ChangeName(string name)
        {

            Model.Name = name;
            Model.PurchaseOrderItem.Name = Model.Name;
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
            Model.Name = name;
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
}
