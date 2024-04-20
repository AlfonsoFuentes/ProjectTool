using Microsoft.AspNetCore.Components;
using Shared.Models.UserAccounts.Reponses;
#nullable disable
namespace ClientRadzen.Pages.Authetication;
public partial class NewUserData
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public UserResponse Data { get; set; }
    [CascadingParameter]
    private NewUserList MainPage { get; set; }
    async Task Delete(UserResponse data)
    {

    }
    async Task UnConfirmUser(UserResponse data)
    {

    }
}
