using Blazored.FluentValidation;
using Client.Infrastructure.Managers.BudgetItems;
using Client.Infrastructure.Managers.CurrencyApis;
using Client.Infrastructure.Managers.PurchaseOrders;
using Client.Infrastructure.Managers.Suppliers;
using ClientRadzen.Pages.Suppliers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.BudgetItems;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.Suppliers;

#nullable disable
namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class CreateRegularPurchaseOrder : IDisposable
    {
        [CascadingParameter]
        public App MainApp { get; set; }
        [Parameter]
        public Guid BudgetItemId { get; set; }
        [Inject]
        private IPurchaseOrderService Service { get; set; }

        [Inject]
        private ISupplierService SupplierService { get; set; }
        [Inject]
        private IBudgetItemService BudgetItemService { get; set; }

        CreatedRegularPurchaseOrderRequest Model = null;
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
            var resultSupplier = await SupplierService.GetAllSupplier();
            if (resultSupplier.Succeeded)
            {
                Suppliers = resultSupplier.Data;

            }

            var result = await BudgetItemService.GetApprovedBudgetItemsById(BudgetItemId);
            if (result.Succeeded)
            {
                Model = new();
                Model.SetMainBudgetItem(result.Data);
                Model.Validator += ValidateAsync;


            }


            Model.TRMUSDCOP = MainApp.RateList == null ? 4000 : Math.Round(MainApp.RateList.COP, 2);
            Model.TRMUSDEUR = MainApp.RateList == null ? 1 : Math.Round(MainApp.RateList.EUR, 2);
          

            var resultBudgetItems = await Service.GetBudgetItemsToCreatePurchaseOrder(Model.MainBudgetItemId);
            if (resultBudgetItems.Succeeded)
            {
               
                OriginalBudgetItems = resultBudgetItems.Data.BudgetItems;
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
            var result = await Service.CreateRegularPurchaseOrder(Model);
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
              await  Model.SetSupplier((result as SupplierResponse));
                var resultData = await SupplierService.GetAllSupplier();
                if (resultData.Succeeded)
                {
                    Suppliers = resultData.Data;

                }


            }
        }

        public void Dispose()
        {
            Model.Validator -= ValidateAsync;
        }
    }
}
