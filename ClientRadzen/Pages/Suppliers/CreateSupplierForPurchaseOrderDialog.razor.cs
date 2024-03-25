using Blazored.FluentValidation;
using Client.Infrastructure.Managers.Suppliers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.Suppliers;
#nullable disable
namespace ClientRadzen.Pages.Suppliers
{
    public partial class CreateSupplierForPurchaseOrderDialog
    {
        [CascadingParameter]
        private App MainApp { get; set; }
        CreateSupplierForPurchaseOrderRequest Model { get; set; } = new();

        [Inject]
        private ISupplierService Service { get; set; } = null!;
        protected override void OnInitialized()
        {
            Model.Validator += ValidateAsync;
        }

        private async Task SaveAsync()
        {


            var result = await Service.CreateSupplierForPurchaseOrder(Model);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);
                DialogService.Close(result.Data);
            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }


        }

       
        FluentValidationValidator _fluentValidationValidator = null!;
        async Task<bool> ValidateAsync()
        {
            NotValidated = !(await _fluentValidationValidator.ValidateAsync());
            return NotValidated;
        }
        bool NotValidated = true;

        public void Dispose()
        {
            Model.Validator -= ValidateAsync;
        }

      

       
        private void CancelAsync()
        {
            DialogService.Close(false);
        }

    }
}
