using Client.Infrastructure.Managers.UserManagement;
using Microsoft.AspNetCore.Components;
#nullable disable
namespace ClientRadzen.Pages.UserManagements;
public partial class Logout
{
    [Inject]
    public IAuthenticationService AuthenticationService { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    protected override async Task OnInitializedAsync()
    {
        await AuthenticationService.Logout();
        NavigationManager.NavigateTo("/");
    }
}
