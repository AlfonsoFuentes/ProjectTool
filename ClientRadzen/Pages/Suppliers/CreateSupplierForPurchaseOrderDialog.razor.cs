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



      
        [Inject]
        private ISupplierService Service { get; set; } = null!;

        private async Task SaveAsync()
        {
            

                var result = await Service.CreateSupplierForPurchaseOrder(Model);
                if (result.Succeeded)
                {
                    DialogService.Close(result.Data);
                }
                else
                {
                    NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
                }
           

        }

        private void CancelAsync()
        {
            DialogService.Close(false);
        }

    }
}
