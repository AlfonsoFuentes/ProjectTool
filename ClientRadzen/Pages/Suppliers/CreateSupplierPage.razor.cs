#nullable disable
using Blazored.FluentValidation;
using Client.Infrastructure.Managers.Suppliers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.Suppliers;

namespace ClientRadzen.Pages.Suppliers
{
    public partial class CreateSupplierPage
    {
        CreateSupplierRequest Model { get; set; } = new();




        [Inject]
        private ISupplierService Service { get; set; } = null!;

        private async Task SaveAsync()
        {


            var result = await Service.CreateSupplier(Model);
            if (result.Succeeded)
            {
                NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                _NavigationManager.NavigateTo("/SupplierDatalist");
            }
            else
            {
                NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }


        }

        private void CancelAsync()
        {
            _NavigationManager.NavigateTo("/SupplierDatalist");
        }

    }
}

