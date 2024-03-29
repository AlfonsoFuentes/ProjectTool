using Blazored.FluentValidation;
using Client.Infrastructure.Managers.CurrencyApis;
using Client.Infrastructure.Managers.PurchaseOrders;
using Client.Infrastructure.Managers.Suppliers;
using ClientRadzen.Pages.Suppliers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.BudgetItems;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits;
using Shared.Models.Suppliers;

#nullable disable
namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class EditPurchaseOrderApproved
    {
        [CascadingParameter]
        public App MainApp { get; set; }
        [Parameter]
        public Guid PurchaseOrderId { get; set; }
        [Inject]
        private IPurchaseOrderService Service { get; set; }


        [Inject]
        private ISupplierService SupplierService { get; set; }


        EditPurchaseOrderRegularApprovedRequest Model = null!;
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
        List<SupplierResponse> Suppliers { get; set; } = new();


        FluentValidationValidator _fluentValidationValidator = null!;

        BudgetItemApprovedResponse ItemToAdd;

        
       protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetPurchaseOrderApprovedToEdit(PurchaseOrderId);
            if (result.Succeeded)
            {
                Model = result.Data;
                Model.Validator += ValidateAsync;
                Model.AddBlankItem();
            }

            var resultSupplier = await SupplierService.GetAllSupplier();
            if (resultSupplier.Succeeded)
            {
                Suppliers = resultSupplier.Data;

            }
            var resultBudgetItems = await Service.GetBudgetItemsToCreatePurchaseOrder(Model.MainBudgetItemId);
            if (resultBudgetItems.Succeeded)
            {

                OriginalBudgetItems = resultBudgetItems.Data.BudgetItems;

            }
          
            //StateHasChanged();
        }

        async Task<bool> ValidateAsync()
        {
            Validated = await _fluentValidationValidator.ValidateAsync();
            return Validated;
        }
        bool Validated = false;

        public async Task SaveAsync()
        {
            var result = await Service.EditPurchaseOrderApproved(Model);
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
        async Task CreateSupplier()
        {
            var result = await DialogService.OpenAsync<CreateSupplierForPurchaseOrderDialog>($"Create Supplier",
                new Dictionary<string, object>() { },
                new DialogOptions() { Width = "400px", Height = "450px", Resizable = true, Draggable = true });
            if (result != null)
            {
                await Model.SetSupplier(result as SupplierResponse);
                var resultData = await SupplierService.GetAllSupplier();
                if (resultData.Succeeded)
                {
                    Suppliers = resultData.Data;

                }


            }
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
    }
}
