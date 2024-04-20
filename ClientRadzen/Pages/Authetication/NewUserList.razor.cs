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
        [Inject]
        private IAuthenticationService Service { get; set; }
        UserResponseList Response = new();
        string nameFilter = string.Empty;
        IQueryable<UserResponse> FilteredItems => Response.Users?.Where(x => x.Email.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)).AsQueryable();
        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetUserList();
            if (result!=null)
            {
                Response = result;
            }
        }
        private void AddNew()
        {
            nameFilter = string.Empty;


            _NavigationManager.NavigateTo("/registration");


        }
    }
}
