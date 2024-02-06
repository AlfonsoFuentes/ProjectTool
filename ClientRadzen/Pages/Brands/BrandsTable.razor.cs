#nullable disable
using Client.Infrastructure.Managers.Brands;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Shared.Models.Brands;

namespace ClientRadzen.Pages.Brands
{
    public partial class BrandsTable
    {
        [Inject]
        private IBrandService Service { get; set; }
        IList<BrandResponse> selectedBrands;
        List<BrandResponse> OriginalData { get; set; } = new();
        private bool _trapFocus = true;
        private bool _modal = true;
        string nameFilter = string.Empty;
        IQueryable<BrandResponse>? FilteredItems => OriginalData?.Where(x => x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)).AsQueryable();
        protected override async Task OnInitializedAsync()
        {
            //var user = CurrentUser.UserId;
            await ShowLoading();
            await UpdateAll();
        }
        bool isLoading = false;

        async Task ShowLoading()
        {
            isLoading = true;

            await Task.Yield();

            isLoading = false;
        }
        async Task UpdateAll()
        {
            var result = await Service.GetAllBrand();
            if (result.Succeeded)
            {
                OriginalData = result.Data;

            }
        }
        private void AddNewBrand()
        {
            _NavigationManager.NavigateTo("/CreateBrand");
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
        RadzenDataGrid<BrandResponse> grid;
        BrandResponse selectedRow = null!;
        void OnRowClick(DataGridRowMouseEventArgs<BrandResponse> args)
        {

            selectedRow = args.Data == null ? null! : args.Data == selectedRow ? null! : args.Data;
        }
    }
}

