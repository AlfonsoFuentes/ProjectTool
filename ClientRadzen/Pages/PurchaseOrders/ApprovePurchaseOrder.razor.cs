#nullable disable
using Blazored.FluentValidation;
using Client.Infrastructure.Managers.PurchaseOrders;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.PurchaseOrders.Requests.Approves;

namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class ApprovePurchaseOrder
    {
        [Inject]
        public IPurchaseOrderService Service { get; set; }
        ApprovePurchaseOrderRequest Model { get; set; } = new();
        [Parameter]
        public Guid PurchaseorderId { get; set; }
        FluentValidationValidator _fluentValidationValidator = null!;
        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetPurchaseOrderToApproveById(PurchaseorderId);
            if (result.Succeeded)
            {
                Model = result.Data;
            }
        }
        public async Task SaveAsync()
        {
            if (await _fluentValidationValidator.ValidateAsync())
            {
                var result = Model.IsAlteration ? await Service.ApprovePurchaseOrderForAlteration(Model) : await Service.ApprovePurchaseOrder(Model);
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
    }
}
