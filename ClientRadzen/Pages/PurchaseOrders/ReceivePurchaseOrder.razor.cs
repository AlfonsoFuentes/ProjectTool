#nullable disable
using Blazored.FluentValidation;
using Client.Infrastructure.Managers.PurchaseOrders;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.PurchaseOrders.Requests.Approves;
using Shared.Models.PurchaseOrders.Requests.Receives;

namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class ReceivePurchaseOrder
    {
        [Inject]
        public IPurchaseOrderService Service { get; set; }
        ReceivePurchaseOrderRequest Model { get; set; } = new();
        [Parameter]
        public Guid PurchaseorderId { get; set; }
        FluentValidationValidator _fluentValidationValidator = null!;
        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetPurchaseOrderToReceiveById(PurchaseorderId);
            if (result.Succeeded)
            {
                Model = result.Data;
            }
        }
        public async Task SaveAsync()
        {
            if (await _fluentValidationValidator.ValidateAsync())
            {
                var result = await Service.ReceivePurchaseOrder(Model);
                if (result.Succeeded)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Summary = "Success",
                        Detail = result.Message,
                        Duration = 4000
                    });

                    _NavigationManager.NavigateTo($"/PurchaseOrdersTable");
                }
                else
                {
                    Model.ValidationErrors = result.Messages;
                }
            }

        }
        void Cancel()
        {
            _NavigationManager.NavigateTo($"/PurchaseOrdersTable");
        }
        bool DisableReceiveButton => Model.ReceivingCurrency == 0 ? true : Model.PendingCurrency < 0 ? true :
            Model.ItemsInPurchaseorder.Any(x => x.PendingCurrency < 0);
    }
}
