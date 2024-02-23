using Blazored.FluentValidation;
using Client.Infrastructure.Managers.Suppliers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.Suppliers;

namespace ClientRadzen.Pages.Suppliers
{
    public partial class CreateSupplierForPurchaseOrderDialog
    {
        CreateSupplierRequest Model { get; set; } = new();



        FluentValidationValidator _fluentValidationValidator = null!;
        [Inject]
        private ISupplierService Service { get; set; } = null!;

        private async Task SaveAsync()
        {
            if (await _fluentValidationValidator!.ValidateAsync())
            {

                var result = await Service.CreateSupplierForPurchaseOrder(Model);
                if (result.Succeeded)
                {
                    DialogService.Close(result.Data);
                }
                else
                {
                    Model.ValidationErrors = result.Messages;
                }
            }

        }

        private void CancelAsync()
        {
            DialogService.Close(false);
        }

    }
}
