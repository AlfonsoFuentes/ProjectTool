using Blazored.FluentValidation;
using Client.Infrastructure.Managers.PurchaseOrders;
using Client.Infrastructure.Managers.Suppliers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.BudgetItems;
using Shared.Models.PurchaseOrders.Requests.Create;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.Suppliers;
#nullable disable
namespace ClientRadzen.Pages.PurchaseOrders
{
    public partial class EditPurchaseOrderCreated
    {
        [Parameter]
        public Guid PurchaseOrderId { get; set; }
        [Inject]
        private IPurchaseOrderService Service { get; set; }
        
        EditPurchaseOrderCreatedRequest Model = new();
        FluentValidationValidator _fluentValidationValidator = null!;
        BudgetItemResponse ItemToChange;


        DataForEditPurchaseOrder CommonData { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
           
            var resultData=await Service.GetDataForEditPurchaseOrder(PurchaseOrderId);
            if(resultData.Succeeded)
            {
                CommonData=resultData.Data;
                Model= resultData.Data.PurchaseOrder;
            }
           
        }
        public async Task SaveAsync()
        {
            if (await _fluentValidationValidator.ValidateAsync())
            {
                var result = await Service.EditPurchaseOrderCreated(Model);
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
