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



        
        [Inject]
        private IBrandService Service { get; set; } = null!;
     
        private async Task SaveAsync()
        {
          

                var result = await Service.CreateBrand(Model);
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

