using Client.Infrastructure.Managers.UserManagement;
using Client.Infrastructure.Managers.VersionSoftwares;
using Shared.Models.UserAccounts.Reponses;
using Shared.NewModels.SoftwareVersion;

namespace ClientRadzen.Pages.Authetication;
public partial class NewVersionList
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public MainUserVersionManagement MainPage { get; set; }
    INewSoftwareVersionService Service => MainPage.SoftwareVersionService;
    NewSoftwareVersionListResponse Response = new();
    string nameFilter = string.Empty;
    IQueryable<NewSoftwareVersionResponse> FilteredItems => Response.SoftwareVersion.Where(x => 
    x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)).AsQueryable();
    protected override async Task OnInitializedAsync()
    {
        await UpdateAll();
    }
    async Task UpdateAll()
    {
        var result = await Service.GetAll();
        if (result.Succeeded)
        {
            Response = result.Data;
        }
    }
    private void AddNew()
    {
        nameFilter = string.Empty;


        _NavigationManager.NavigateTo("/NewVersionCreate");


    }
    
}
