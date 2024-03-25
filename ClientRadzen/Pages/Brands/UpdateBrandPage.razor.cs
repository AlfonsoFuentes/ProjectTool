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
        public Guid BrandId { get; set; }
        [CascadingParameter]
        private App MainApp { get; set; }
        UpdateBrandRequest Model { get; set; } = new();

        [Inject]
        private IBrandService Service { get; set; } = null!;
        protected override async void OnInitialized()
        {
            var result=await Service.GetBrandById(BrandId);
            if(result.Succeeded)
            {
                Model = result.Data;
            }
            Model.Validator += ValidateAsync;
            StateHasChanged();
        }

        private async Task SaveAsync()
        {


            var result = await Service.UpdateBrand(Model);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                CancelAsync();
            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }


        }

        private void CancelAsync()
        {
            Navigation.NavigateBack();

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
    }
}

