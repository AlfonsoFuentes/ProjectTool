
#nullable disable
namespace ClientRadzen.NewPages.Brands;
public partial class NewBrandMain
{
    [CascadingParameter]
    private App MainApp { get; set; }

    [Inject]
    private INewBrandService Service { get; set; }

    NewBrandListResponse NewBrandResponseList { get; set; } = new NewBrandListResponse();
    public List<NewBrandResponse> OriginalData => NewBrandResponseList.Brands;

    string nameFilter = string.Empty;
   public IQueryable<NewBrandResponse> FilteredItems => OriginalData?.Where(x => x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)).AsQueryable();
    protected override async Task OnInitializedAsync()
    {
        await UpdateAll();

    }



    async Task UpdateAll()
    {
        var result = await Service.GetAllBrand();
        if (result.Succeeded)
        {
            NewBrandResponseList = result.Data;

        }
    }


    public void Edit(NewBrandResponse BrandResponse)
    {
        _NavigationManager.NavigateTo($"/{PageName.Brand.Update}/{BrandResponse.BrandId}");
    }
    public async Task Delete(NewBrandResponse BrandResponse)
    {
        var resultDialog = await DialogService.Confirm($"Are you sure delete {BrandResponse.Name}?", "Confirm Delete",
            new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {
            var result = await Service.Delete(BrandResponse);
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
        _NavigationManager.NavigateTo($"/{PageName.Brand.Create}");
    }

}
