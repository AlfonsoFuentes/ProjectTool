using Blazored.FluentValidation;
using Client.Infrastructure.Managers.BudgetItems;
using Client.Infrastructure.Managers.CurrencyApis;
using Client.Infrastructure.Managers.PurchaseOrders;
using Client.Infrastructure.Managers.Suppliers;
using ClientRadzen.Pages.Suppliers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.BudgetItems;
using Shared.Models.PurchaseOrders.Requests.Create;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.Suppliers;

#nullable disable
namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class CreatePurchaseOrder
    {
        [Parameter]
        public Guid BudgetItemId { get; set; }

        [Inject]
        private IPurchaseOrderService Service { get; set; }
        [Inject] public IRate _CurrencyService { get; set; }
        [Inject]
        private ISupplierService SupplierService { get; set; }

        DataForCreatePurchaseOrder CommonData { get; set; } = new();

        CreatePurchaseOrderRequest Model { get; set; } = new();
        ConversionRate RateList { get; set; }
        DateOnly CurrentDate => DateOnly.FromDateTime(DateTime.UtcNow);
        BudgetItemResponse ItemToAdd;
        SupplierResponse Supplier;
        BudgetItemResponse BudgetItemToCreatePO = new();
        MWOResponse MWO = new();
        protected override async Task OnInitializedAsync()
        {

            RateList = await _CurrencyService.GetRates();
            var result = await Service.GetDataForCreatePurchaseOrder(BudgetItemId);
            if (result.Succeeded)
            {
                CommonData = result.Data;
                Model = new();
                Model.USDCOP = RateList == null ? 4000 : Math.Round(RateList.COP, 2);
                Model.USDEUR = RateList == null ? 1 : Math.Round(RateList.EUR, 2);
                Model.SetMWOBudgetItem(CommonData.MWO, CommonData.BudgetItem);
                MWO = CommonData.MWO;
                Model.MainBudgetItemId = BudgetItemId;
                BudgetItemToCreatePO = CommonData.BudgetItem;


            }


        }
        FluentValidationValidator _fluentValidationValidator = null!;
        public async Task SaveAsync()
        {
            if (await _fluentValidationValidator.ValidateAsync())
            {
                var result = await Service.CreatePurchaseOrder(Model);
                if (result.Succeeded)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Summary = "Success",
                        Detail = result.Message,
                        Duration = 4000
                    });

                    _NavigationManager.NavigateTo($"/BudgetItemtable/{MWO.Id}");
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
            _NavigationManager.NavigateTo($"/BudgetItemtable/{MWO.Id}");
        }
        async Task CreateSupplier()
        {
            var result = await DialogService.OpenAsync<CreateSupplierForPurchaseOrderDialog>($"Create Supplier",
                new Dictionary<string, object>() { },
                new DialogOptions() { Width = "700px", Height = "512px", Resizable = true, Draggable = true });
            if (result != null)
            {
                Supplier = result as SupplierResponse;
                var resultData = await SupplierService.GetAllSupplier();
                if (resultData.Succeeded)
                {
                    CommonData.Suppliers = resultData.Data;

                }


            }
        }
    }
}
