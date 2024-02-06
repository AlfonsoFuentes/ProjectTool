#nullable disable
using Blazored.FluentValidation;
using Client.Infrastructure.Managers.Suppliers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.Suppliers;

namespace ClientRadzen.Pages.Suppliers
{
    public partial class UpdateSupplierPage
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        private ISupplierService Service { get; set; }

        UpdateSupplierRequest Model { get; set; } = new();
        FluentValidationValidator _fluentValidationValidator = null!;
        string SupplierName = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetSupplierById(Id);
            if (result.Succeeded)
            {
                Model = result.Data;
                SupplierName = Model.Name;
            }
        }
        private async Task SaveAsync()
        {
            if (await _fluentValidationValidator!.ValidateAsync())
            {

                var result = await Service.UpdateSupplier(Model);
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
