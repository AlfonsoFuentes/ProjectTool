using Client.Infrastructure.Managers.Suppliers;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using Shared.Models.Suppliers;
using Client.Infrastructure.Managers.Brands;
using Microsoft.AspNetCore.Components.Web;
using Shared.Models.Brands;
using Azure;
#nullable disable
namespace ClientRadzen.Pages.Suppliers
{
    public partial class SuppliersDataList
    {
        [CascadingParameter]
        private App MainApp { get; set; }
        [Inject]
        private ISupplierService Service { get; set; }

        List<SupplierResponse> OriginalData { get; set; } = new();

        string nameFilter = string.Empty;
        IQueryable<SupplierResponse> FilteredItems => OriginalData?.Where(x => 
        x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)||
        x.NickName.Contains(nameFilter,StringComparison.CurrentCultureIgnoreCase)).AsQueryable();
        protected override async Task OnInitializedAsync()
        {

           
            await UpdateAll();
        }
        
        async Task UpdateAll()
        {
            var result = await Service.GetAllSupplier();
            if (result.Succeeded)
            {
                OriginalData = result.Data;

            }
        }



      
        void EditByForm(SupplierResponse Response)
        {
            _NavigationManager.NavigateTo($"/UpdateSupplier/{Response.Id}");

        }
        async Task Delete(SupplierResponse response)
        {
            var resultDialog = await DialogService.Confirm($"Are you sure delete {response.Name}?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (resultDialog.Value)
            {
                var result = await Service.Delete(response);
                if (result.Succeeded)
                {
                   MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                    await UpdateAll();
                }
                else
                {
                    MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
                }

            }

        }
      
        private void AddNew()
        {
            _NavigationManager.NavigateTo($"/CreateSupplier");
        }

    }
}
