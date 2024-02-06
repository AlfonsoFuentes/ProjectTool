using Blazored.FluentValidation;
using Client.Infrastructure.Managers.Brands;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.Brands;

namespace ClientRadzen.Pages.Brands
{
    public partial class CreateBrandPage
    {
        CreateBrandRequest Model { get; set; } = new();



        FluentValidationValidator _fluentValidationValidator = null!;
        [Inject]
        private IBrandService Service { get; set; } = null!;

        private async Task SaveAsync()
        {
            if (await _fluentValidationValidator!.ValidateAsync())
            {

                var result = await Service.CreateBrand(Model);
                if (result.Succeeded)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Summary = "Success",
                        Detail = result.Message,
                        Duration = 4000
                    });

                    _NavigationManager.NavigateTo("/brandstable");
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
            _NavigationManager.NavigateTo("/brandstable");
        }

    }

}

