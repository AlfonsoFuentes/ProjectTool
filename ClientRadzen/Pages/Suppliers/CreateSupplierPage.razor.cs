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
                    Model.ValidationErrors = result.Messages;
                }
            }

        }

        private void CancelAsync()
        {
            _NavigationManager.NavigateTo("/Supplierstable");
        }

    }
}

