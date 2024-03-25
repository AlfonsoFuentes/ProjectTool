#nullable disable
using Blazored.FluentValidation;
using Client.Infrastructure.Managers.PurchaseOrders;
using Client.Infrastructure.Managers.Suppliers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.BudgetItems;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;

namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class ApprovePurchaseOrder:IDisposable
    {
        [CascadingParameter]
        public App MainApp { get; set; }
        [Parameter]
        public Guid PurchaseOrderId { get; set; }
        [Inject]
        private IPurchaseOrderService Service { get; set; }


        [Inject]
        private ISupplierService SupplierService { get; set; }


        ApprovedRegularPurchaseOrderRequest Model = new();
        List<BudgetItemApprovedResponse> OriginalBudgetItems { get; set; } = new();
        List<BudgetItemApprovedResponse> BudgetItems => GetBudgetItems();
        List<BudgetItemApprovedResponse> GetBudgetItems()
        {
            List<BudgetItemApprovedResponse> response = new();
            foreach (var row in OriginalBudgetItems)
            {
                if (!Model.PurchaseOrderItems.Any(x => x.BudgetItemId == row.BudgetItemId))
                {
                    response.Add(row);
                }
            }

            return response;
        }
      


        FluentValidationValidator _fluentValidationValidator = null!;

        BudgetItemApprovedResponse ItemToAdd;

        
       protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetPurchaseOrderToApproveById(PurchaseOrderId);
            if (result.Succeeded)
            {
                Model = result.Data;
                Model.Validator += ValidateAsync;
                Model.AddBlankItem();
            }

            var resultBudgetItems = await Service.GetBudgetItemsToCreatePurchaseOrder(Model.MainBudgetItemId);
            if (resultBudgetItems.Succeeded)
            {

                OriginalBudgetItems = resultBudgetItems.Data.BudgetItems;

            }
            await ValidateAsync();
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
            var result = Model.IsAlteration?await Service.ApprovePurchaseOrderForAlteration(Model):await Service.ApproveRegularPurchaseOrder(Model);
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
            //_NavigationManager.NavigateTo($"/MWOApproved/{Model.MWOId}");
        }
        bool UpdateCurrentTRM = false;
        public void ClickUpdateCurrentTRM()
        {
            Model.OldCurrencyDate = Model.CurrencyDate;
            Model.OldTRMUSDCOP = Model.TRMUSDCOP;
            Model.OldTRMUSDEUR = Model.TRMUSDEUR;

            Model.CurrencyDate = DateTime.UtcNow;
            Model.TRMUSDCOP = MainApp.RateList.COP;
            Model.TRMUSDEUR = MainApp.RateList.EUR;
            UpdateCurrentTRM = true;
        }
        public void ClickUpdateOldTRM()
        {


            Model.CurrencyDate = Model.OldCurrencyDate;
            Model.TRMUSDCOP = Model.OldTRMUSDCOP;
            Model.TRMUSDEUR = Model.OldTRMUSDEUR;
            UpdateCurrentTRM = false;
        }

        public void Dispose()
        {
            Model.Validator -= ValidateAsync;
        }
    }
}
