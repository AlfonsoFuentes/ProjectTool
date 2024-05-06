
#nullable disable


using Shared.NewModels.BudgetItems.Responses;

namespace ClientRadzen.NewPages.BudgetItems.MWOCreated;
public partial class NewBudgetItemsMWOCreatedMain
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public Guid MWOId { get; set; }
    [Inject]
    private INewBudgetItemService Service { get; set; }
    public NewMWOCreatedWithItemsResponse Response { get; set; } = new();
   public string nameFilter { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await UpdateAll();
    }
    async Task UpdateAll()
    {
        var result = await Service.GetAllMWOCreatedWithItems(MWOId);
        if (result.Succeeded)
        {
            Response = result.Data;
        }
        StateHasChanged();
    }
    Func<NewBudgetItemMWOCreatedResponse, bool> fiterexpresion => x =>
       x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
       x.Nomeclatore.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
       x.BrandName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
       x.Type.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase);
    public List<NewBudgetItemMWOCreatedResponse> FilteredItems => Response.BudgetItems.Count == 0 ? new() : 
        Response.BudgetItems?.Where(fiterexpresion).ToList();

    public void ChangeFilter(string arg)
    {
        nameFilter = arg;
    }
    public void AddNewBudgetItem()
    {
        _NavigationManager.NavigateTo($"/{PageName.BudgetItems.Create}/{MWOId}");
    }
    public async Task Approve()
    {
        var resultDialog = await DialogService.Confirm($"Are you want to approved {Response.Name}?", "Confirm",
            new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {
            _NavigationManager.NavigateTo($"/{PageName.MWO.Approve}/{Response.MWOId}");

        }

    }
    public async Task ExporNotApprovedToExcel()
    {
        //var result = await Service.ExporNotApprovedToExcel(MWOId);
        //if (result.Succeeded)
        //{
        //    var downloadresult = await blazorDownloadFileService.DownloadFile(result.Data.ExportFileName,
        //       result.Data.Data, contentType: result.Data.ContentType);
        //    if (downloadresult.Succeeded)
        //    {
        //        MainApp.NotifyMessage(NotificationSeverity.Success, "Export Excel", new() { "Export excel succesfully" });


        //    }
        //}


    }
    public async Task Delete(NewBudgetItemMWOCreatedResponse BudgetItemResponse)
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
                await UpdateAll();
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);
            

            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }

        }

    }

    public void Edit(NewBudgetItemMWOCreatedResponse Response)
    {
        _NavigationManager.NavigateTo($"/{PageName.BudgetItems.Update}/{Response.BudgetItemId}");

    }
}
