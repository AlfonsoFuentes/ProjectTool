using Client.Infrastructure.Managers.ChangeUser;
using Microsoft.AspNetCore.Components;

namespace ClientRadzen.Pages;
public partial class Home
{
    [Inject]
    private IChangeUserManager changeUserManager { get; set; }
    protected override async Task OnInitializedAsync()
    {
        //if (CurrentUser.UserName == "alfonso_fuentes@colpal.com")
        //{
        //Este codigo se creo para cambiar de usuario de alfonsofuen@gmail.com (Superadmin) a regular user(alfonso_fuentes)
        //    await changeUserManager.ChangeUser();
        //}
    }

}
