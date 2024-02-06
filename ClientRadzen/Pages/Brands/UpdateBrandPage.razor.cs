using Blazored.FluentValidation;
using Client.Infrastructure.Managers.Brands;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.Brands;
#nullable disable
namespace ClientRadzen.Pages.Brands
{
    public partial class UpdateBrandPage
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        private IBrandService Service { get; set; }

        UpdateBrandRequest Model { get; set; } = new();
        FluentValidationValidator _fluentValidationValidator = null!;
        string BrandName = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetBrandById(Id);
            if (result.Succeeded)
            {
                Model = result.Data;
                BrandName = Model.Name;
            }
        }
        private async Task SaveAsync()
        {
            if (await _fluentValidationValidator!.ValidateAsync())
            {

                var result = await Service.UpdateBrand(Model);
                if (result.Succeeded)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Summary = "Success",
                        Detail = result.Message,
                        Duration = 4000
                    });

                    _NavigationManager.NavigateTo("/Brandstable");
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
            _NavigationManager.NavigateTo("/Brandstable");
        }
    }
}

