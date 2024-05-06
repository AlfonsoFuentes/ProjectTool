namespace ClientRadzen.NewPages.MWOS;
#nullable disable
public partial class NewMWOMain
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Inject]
    public INewMWOService Service { get; set; }

    public List<NewMWOCreatedResponse> MWOsCreated => MWOCreatedList.MWOsCreated;
    public List<NewMWOApprovedReponse> MWOsApproved => MWOApprovedList.MWOsApproved;
    NewMWOCreatedListResponse MWOCreatedList { get; set; } = new();
    NewMWOApprovedListReponse MWOApprovedList { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        await UpdateAllCreated();
    }
    async Task UpdateAllCreated()
    {
        var resultCreated = await Service.GetAllMWOCreated();
        if (resultCreated.Succeeded)
        {
            MWOCreatedList = resultCreated.Data;
        }
        var resultApproved = await Service.GetAllMWOApproved();
        if (resultApproved.Succeeded)
        {
            MWOApprovedList = resultApproved.Data;
        }
        StateHasChanged();
    }
    public string nameFilterCreated { get; set; } = string.Empty;
    public string nameFilterApproved { get; set; } = string.Empty;
    public string nameFilterClosed { get; set; } = string.Empty;
    string _nameFilter = string.Empty;
    public string nameFilter
    {
        get
        {
            switch (MainApp.TabIndexMWO)
            {
                case 0:
                    _nameFilter = nameFilterCreated;

                    break;
                case 1:
                    _nameFilter = nameFilterApproved ;
                    break;
                case 2:
                    _nameFilter = nameFilterClosed ;
                    break;

            }
            return _nameFilter;
        }
        set
        {
            _nameFilter = value;

        }
    }

    void ChangeNameFilter(string filter)
    {
        _nameFilter = filter;
        switch (MainApp.TabIndexMWO)
        {
            case 0:
                nameFilterCreated = filter;

                break;
            case 1:
                nameFilterApproved = filter;
                break;
            case 2:
                nameFilterClosed = filter;
                break;

        }
    }
    void ChangeIndex(int index)
    {
        MainApp.TabIndexMWO = index;
    }
    private void AddNew()
    {
        nameFilter = string.Empty;
        _NavigationManager.NavigateTo($"/{PageName.MWO.Create}");


    }
    async Task ExporToExcel()
    {
        //var result = MainApp.TabIndexMWO == 0 ? await Service.ExportMWOsCreated() :
        //    MainApp.TabIndexMWO == 1 ? await Service.ExportMWOsApproved() :
        //    await Service.ExportMWOsClosed();
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
    public async Task Delete(NewMWOCreatedResponse response)
    {

        var resultDialog = await DialogService.Confirm($"Are you sure delete {response.Name}?", "Confirm Delete",
            new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No", CloseDialogOnEsc = true, CloseDialogOnOverlayClick = true });
        if (resultDialog.Value)
        {
            NewMWODeleteRequest request = new NewMWODeleteRequest()
            {
                MWOId = response.MWOId,
                Name = response.Name,
            };
            var result = await Service.DeleteMWO(request);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                await UpdateAllCreated();
            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }

        }


    }
    public async Task ApproveMWO(NewMWOCreatedResponse Response)
    {
        var resultDialog = await DialogService.Confirm($"Are you sure Approved {Response.Name}?", "Confirm",
            new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {
            _NavigationManager.NavigateTo($"/{PageName.MWO.Approve}/{Response.MWOId}");

        }

    }
    public void EditMWOCreated(NewMWOCreatedResponse Response)
    {
        _NavigationManager.NavigateTo($"/{PageName.MWO.Update}/{Response.MWOId}");


    }
    public void AddItemToMWO(NewMWOCreatedResponse Response)
    {
        _NavigationManager.NavigateTo($"/{PageName.BudgetItems.Create}/{Response.MWOId}");

    }
    public void ShowItemsofMWO(NewMWOCreatedResponse Response)
    {
        _NavigationManager.NavigateTo($"{PageName.BudgetItems.BudgetItemsCreated}/{Response.MWOId}");

    }
    public void ShowApprovedItemsofMWO(NewMWOApprovedReponse Response)
    {
        _NavigationManager.NavigateTo($"{PageName.BudgetItems.BudgetItemsApproved}/{Response.MWOId}");

    }
    public void ShowSapAlignmentofMWO(NewMWOApprovedReponse Response)
    {
        //_NavigationManager.NavigateTo($"/SapAdjustListByMWO/{Response.MWOId}");

    }
    public async Task UnApproveMWO(NewMWOApprovedReponse Response)
    {
        NewMWOUnApproveRequest UnApproveMWORequest = new()
        {
            MWOId = Response.MWOId,
            Name = Response.Name,
           
        };
        var resultDialog = await DialogService.Confirm($"Are you sure Un Approve {UnApproveMWORequest.Name}?", "Confirm",
           new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {

            var result = await Service.UnApprovedMWO(UnApproveMWORequest);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);
                await UpdateAllCreated();
            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }
        }
    }
}
