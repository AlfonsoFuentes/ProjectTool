#nullable disable
using Blazored.FluentValidation;
using Client.Infrastructure.Managers.BudgetItems;
using Client.Infrastructure.Managers.PurchaseOrders;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;

namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class CreateCapitalizedSalary
    {
        [CascadingParameter]
        public App MainApp { get; set; }
        [Parameter]
        public Guid BudgetItemId { get; set; }
        [Inject]
        private IPurchaseOrderService Service { get; set; }

       
        [Inject]
        private IBudgetItemService BudgetItemService { get; set; }

        CreateCapitalizedSalaryPurchaseOrderRequest Model = null;
       
       

        FluentValidationValidator _fluentValidationValidator = null!;



      
        protected override async Task OnInitializedAsync()
        {
           
            var result = await BudgetItemService.GetApprovedBudgetItemsById(BudgetItemId);
            if (result.Succeeded)
            {
                Model = new();
                Model.SetMainBudgetItem(result.Data);
              

                Model.USDCOP = MainApp.RateList == null ? 4000 : Math.Round(MainApp.RateList.COP, 2);
                Model.USDEUR = MainApp.RateList == null ? 1 : Math.Round(MainApp.RateList.EUR, 2);

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
            var result = await Service.CreatePurchaseOrderCapitalizedSalary(Model);
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
            item.ActualUSD = currencyvalue;

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
}
