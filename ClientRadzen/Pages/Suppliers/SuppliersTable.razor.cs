#nullable disable
using ClientRadzen.Managers.Suppliers;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using Shared.Models.Suppliers;

namespace ClientRadzen.Pages.Suppliers
{
    public partial class SuppliersTable
    {
        [Inject]
        private ISupplierService Service { get; set; }
        IList<SupplierResponse> selectedSuppliers;
        List<SupplierResponse> OriginalData { get; set; } = new();
        private bool _trapFocus = true;
        private bool _modal = true;
        string nameFilter = string.Empty;
        IQueryable<SupplierResponse>? FilteredItems => OriginalData?.Where(x => x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)).AsQueryable();
        protected override async Task OnInitializedAsync()
        {
            var user = CurrentUser.UserId;
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
            var result = await Service.GetAllSupplier();
            if (result.Succeeded)
            {
                OriginalData = result.Data;

            }
        }
        private void AddNewSupplier()
        {
            _NavigationManager.NavigateTo("/CreateSupplier");
        }

        void Edit(SupplierResponse SupplierResponse)
        {
            _NavigationManager.NavigateTo($"/UpdateSupplier/{SupplierResponse.Id}");
        }
        async Task Delete(SupplierResponse SupplierResponse)
        {
            var resultDialog = await DialogService.Confirm($"Are you sure delete {SupplierResponse.Name}?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (resultDialog.Value)
            {
                var result = await Service.Delete(SupplierResponse);
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
        RadzenDataGrid<SupplierResponse> grid;
        SupplierResponse selectedRow = null!;
        void OnRowClick(DataGridRowMouseEventArgs<SupplierResponse> args)
        {

            selectedRow = args.Data == null ? null! : args.Data == selectedRow ? null! : args.Data;
        }

    }
}
