#nullable disable
namespace ClientRadzen.NewPages.Suppliers;
public partial class NewSupplierMain
{
    [CascadingParameter]
    private App MainApp { get; set; }
    [Inject]
    private INewSupplierService Service { get; set; }

    NewSupplierListResponse NewSupplierResponseList { get; set; } = new NewSupplierListResponse();
    public List<NewSupplierResponse> OriginalData => NewSupplierResponseList.Suppliers;

    string nameFilter = string.Empty;
    public IQueryable<NewSupplierResponse> FilteredItems => OriginalData?.Where(x => x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)).AsQueryable();
    protected override async Task OnInitializedAsync()
    {
        await UpdateAll();

    }



    async Task UpdateAll()
    {
        var result = await Service.GetAllSupplier();
        if (result.Succeeded)
        {
            NewSupplierResponseList = result.Data;

        }
    }


    public void Edit(NewSupplierResponse NewSupplierResponse)
    {
        _NavigationManager.NavigateTo($"/{PageName.Supplier.Update}/{NewSupplierResponse.SupplierId}");
    }
    public async Task Delete(NewSupplierResponse NewSupplierResponse)
    {
        var resultDialog = await DialogService.Confirm($"Are you sure delete {NewSupplierResponse.Name}?", "Confirm Delete",
            new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {
            var result = await Service.Delete(NewSupplierResponse);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);


            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);


            }

        }

    }

    public void AddNew()
    {
        _NavigationManager.NavigateTo($"/{PageName.Supplier.Create}");
    }
    async Task ExporToExcel()
    {
        var result = await Service.ExporToExcel();
        if (result.Succeeded)
        {
            var downloadresult = await blazorDownloadFileService.DownloadFile(result.Data.ExportFileName,
               result.Data.Data, contentType: result.Data.ContentType);
            if (downloadresult.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Export Excel", new() { "Export excel succesfully" });


            }
        }


    }
}
