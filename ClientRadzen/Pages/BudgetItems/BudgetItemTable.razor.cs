#nullable disable
using Client.Infrastructure.Managers.BudgetItems;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using Shared.Models.BudgetItems;
using Azure;

namespace ClientRadzen.Pages.BudgetItems
{
    public partial class BudgetItemTable
    {
        [Inject]
        private IBudgetItemService Service { get; set; }
        IList<BudgetItemResponse> selectedBudgetItems;


        ListBudgetItemResponse Response { get; set; } = new();
        string nameFilter = string.Empty;

        [Parameter]
        public Guid MWOId { get; set; }
        IQueryable<BudgetItemResponse> FilteredItems => Response.BudgetItems?.Where(x => x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)).AsQueryable();
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
            var result = await Service.GetAllBudgetItemByMWO(MWOId);

            if (result.Succeeded)
            {
                Response = result.Data;


            }
        }
        private void AddNewBudgetItem()
        {
            _NavigationManager.NavigateTo($"/CreateBudgetItem/{MWOId}");
        }

        void Edit(BudgetItemResponse BudgetItemResponse)
        {
            _NavigationManager.NavigateTo($"/UpdateBudgetItem/{BudgetItemResponse.Id}");
        }
        void GotoMWOTable()
        {
            _NavigationManager.NavigateTo($"/mwotable");
        }
        void GoToCreatePurchaseOrder(BudgetItemResponse BudgetItemResponse)
        {
            _NavigationManager.NavigateTo($"/CreatePurchaseOrder/{BudgetItemResponse.Id}");
        }
        async Task Delete(BudgetItemResponse BudgetItemResponse)
        {
            if (BudgetItemResponse.IsNotAbleToEditDelete)
            {
                await DialogService.Alert($"Can not remove {BudgetItemResponse.Name}", "Project Tool", new AlertOptions() { OkButtonText = "Yes" });

                return;
            }
            var resultDialog = await DialogService.Confirm($"Are you sure delete {BudgetItemResponse.Name}?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (resultDialog.Value)
            {
                var result = await Service.Delete(BudgetItemResponse);
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
        RadzenDataGrid<BudgetItemResponse> grid;
        BudgetItemResponse selectedRow = null!;
        void OnRowClick(DataGridRowMouseEventArgs<BudgetItemResponse> args)
        {

            selectedRow = args.Data == null ? null! : args.Data == selectedRow ? null! : args.Data;
        }
        bool DisableCreatePurchaseOrderButton => selectedRow == null ? true : selectedRow.IsMainItemTaxesNoProductive;
    }
}
