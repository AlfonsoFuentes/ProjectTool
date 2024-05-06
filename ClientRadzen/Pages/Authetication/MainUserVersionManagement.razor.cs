using Client.Infrastructure.Managers.UserManagement;
using Client.Infrastructure.Managers.VersionSoftwares;
#nullable disable
namespace ClientRadzen.Pages.Authetication;
public partial class MainUserVersionManagement
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Inject]
    public IAuthenticationService AuthService { get; set; }
    [Inject]
    public INewSoftwareVersionService SoftwareVersionService { get; set; }

}
