using Blazored.FluentValidation;
using Client.Infrastructure.Managers.Brands;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Shared.Models.Brands;
#nullable disable
namespace ClientRadzen.Pages.Brands
{
    public partial class BrandListData
    {
        [CascadingParameter]
        private App MainApp { get; set; }
    
        [Inject]
        private IBrandService Service { get; set; }
  
        List<BrandResponse> OriginalData { get; set; } = new();

        string nameFilter = string.Empty;
        IQueryable<BrandResponse> FilteredItems => OriginalData?.Where(x => x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)).AsQueryable();
        protected override async Task OnInitializedAsync()
        {
            await UpdateAll();
    
        }
       

      
        async Task UpdateAll()
        {
            var result = await Service.GetAllBrand();
            if (result.Succeeded)
            {
                OriginalData = result.Data;

            }
        }
      
       
        void Edit(BrandResponse BrandResponse)
        {
            _NavigationManager.NavigateTo($"/UpdateBrand/{BrandResponse.Id}");
        }
        async Task Delete(BrandResponse BrandResponse)
        {
            var resultDialog = await DialogService.Confirm($"Are you sure delete {BrandResponse.Name}?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (resultDialog.Value)
            {
                var result = await Service.Delete(BrandResponse);
                if (result.Succeeded)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Summary = "Success",
                        Detail = result.Message,
                        Duration = 4000
                    });

                    await UpdateAll();
                }
                else
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Error",
                        Detail = result.Message,
                        Duration = 4000
                    });
                }

            }

        }
        
        private void AddNew()
        {
            _NavigationManager.NavigateTo($"/CreateBrand");
        }

        
    }
}
