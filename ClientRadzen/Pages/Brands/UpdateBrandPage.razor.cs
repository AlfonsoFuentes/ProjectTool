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
           

                var result = await Service.UpdateBrand(Model);
                if (result.Succeeded)
                {
                    NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                    _NavigationManager.NavigateTo("/BrandDatalist");
                }
                else
                {
                    NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
                }
            

        }
       
        private void CancelAsync()
        {
            _NavigationManager.NavigateTo("/BrandDatalist");
        }
    }
}

