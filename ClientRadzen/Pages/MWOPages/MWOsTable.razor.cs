#nullable disable
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Shared.Models.MWOTypes;


namespace ClientRadzen.Pages.MWOPages
{
    public partial class MWOsTable
    {
        [Inject]
        private IMWOService Service { get; set; }

        List<MWOResponse> OriginalData { get; set; } = new();
        private bool _trapFocus = true;
        private bool _modal = true;
        string nameFilter = string.Empty;
        IQueryable<MWOResponse> FilteredItems => OriginalData?.Where(x => x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)).AsQueryable();
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
            var result = await Service.GetAllMWO();
            if (result.Succeeded)
            {
                OriginalData = result.Data;

            }
        }
        private void AddNewMWO()
        {
            _NavigationManager.NavigateTo("/CreateMWO");
        }

        void Edit(MWOResponse mWOResponse)
        {
            _NavigationManager.NavigateTo($"/UpdateMWO/{mWOResponse.Id}");
        }
        void ShowBudgetItems(MWOResponse mWOResponse)
        {
            _NavigationManager.NavigateTo($"/BudgetItemtable/{mWOResponse.Id}");
        }
        async Task Approve(MWOResponse mWOResponse)
        {
            var resultDialog = await DialogService.Confirm($"Are you sure Approved {mWOResponse.Name}?", "Confirm",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (resultDialog.Value)
            {
                _NavigationManager.NavigateTo($"/ApproveMWO/{mWOResponse.Id}");
                
            }

        }
        async Task Delete(MWOResponse mWOResponse)
        {
            var resultDialog =await DialogService.Confirm($"Are you sure delete {mWOResponse.Name}?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if(resultDialog.Value)
            {
                var result = await Service.Delete(mWOResponse);
                if(result.Succeeded)
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
        RadzenDataGrid<MWOResponse> grid;
        IEnumerable<string> selectedTypes;
        List<string> titles = MWOTypeEnum.List.Select(x => x.Name).ToList();
        IList<MWOResponse> selectedMWOs;

        async Task OnSelectedTypesChange(object value)
        {
            if (selectedTypes != null && !selectedTypes.Any())
            {
                selectedTypes = null!;
            }

            await grid.FirstPage();
        }
       
        MWOResponse selectedRow = null!;
        void OnRowClick(DataGridRowMouseEventArgs<MWOResponse> args)
        {

            selectedRow = args.Data == null ? null! : args.Data == selectedRow ? null! : args.Data;
        }
        async Task FilterCleared()
        {
            selectedTypes = null;

            await grid.FirstPage();
        }
    }
}
