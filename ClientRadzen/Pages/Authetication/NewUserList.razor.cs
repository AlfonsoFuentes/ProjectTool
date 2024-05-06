using Client.Infrastructure.Managers.UserManagement;
using Microsoft.AspNetCore.Components;
using Shared.Models.UserAccounts.Reponses;
#nullable disable

namespace ClientRadzen.Pages.Authetication
{
    public partial class NewUserList
    {
        [CascadingParameter]
        public App MainApp { get; set; }
        [CascadingParameter]
        public MainUserVersionManagement MainPage { get; set; }
        IAuthenticationService Service => MainPage.AuthService;
        UserResponseList Response = new();
        string nameFilter = string.Empty;
        IQueryable<UserResponse> FilteredItems => Response.Users?.Where(x => x.Email.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)).AsQueryable();
        protected override async Task OnInitializedAsync()
        {
            await UpdateAll();
        }
        async Task UpdateAll()
        {
            var result = await Service.GetUserList();
            if (result != null)
            {
                Response = result;
            }
        }
        private void AddNew()
        {
            nameFilter = string.Empty;


            _NavigationManager.NavigateTo("/registration");


        }
        async Task Delete(UserResponse data)
        {
            var result = await Service.DeleteUser(data.Email);
            if (result)
            {
                MainApp.NotifyMessage(Radzen.NotificationSeverity.Success,
                    "Delete PassWord",
                    new() { $"Password for {data.Email} deleted succesfully" });
                await UpdateAll();
            }
            else
            {
                MainApp.NotifyMessage(Radzen.NotificationSeverity.Error,
                    "Delete PassWord",
                    new() { $"Password for {data.Email} couldnt deleted" });
            }

        }
        async Task UnConfirmUser(UserResponse data)
        {
            var result = await Service.ResetPassword(data.Email);
            if (result)
            {
                MainApp.NotifyMessage(Radzen.NotificationSeverity.Success,
                    "Reset PassWord",
                    new() { $"Password for {data.Email} reseted succesfully" });
                await UpdateAll();
            }
            else
            {
                MainApp.NotifyMessage(Radzen.NotificationSeverity.Error,
                    "Reset PassWord",
                    new() { $"Password for {data.Email} couldnt reseted" });
            }
        }
    }
}
