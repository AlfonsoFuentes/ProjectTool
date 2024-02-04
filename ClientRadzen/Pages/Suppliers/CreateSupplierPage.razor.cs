#nullable disable
using Blazored.FluentValidation;
using ClientRadzen.Managers.Suppliers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.Suppliers;

namespace ClientRadzen.Pages.Suppliers
{
    public partial class CreateSupplierPage
    {
        CreateSupplierRequest Model { get; set; } = new();



        FluentValidationValidator _fluentValidationValidator = null!;
        [Inject]
        private ISupplierService Service { get; set; } = null!;

        private async Task SaveAsync()
        {
            if (await _fluentValidationValidator!.ValidateAsync())
            {

                var result = await Service.CreateSupplier(Model);
                if (result.Succeeded)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Summary = "Success",
                        Detail = result.Message,
                        Duration = 4000
                    });

                    _NavigationManager.NavigateTo("/Supplierstable");
                }
                else
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Error Summary",
                        Detail = result.Message,
                        Duration = 4000
                    });
                }
            }

        }

        private void CancelAsync()
        {
            _NavigationManager.NavigateTo("/Supplierstable");
        }

    }
}

