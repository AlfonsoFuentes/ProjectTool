using Client.Infrastructure.Managers.UserAccount;
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
        private IUserAccountService Service { get; set; }
        UserReponseList Response = new();
        string nameFilter = string.Empty;
        IQueryable<UserReponse> FilteredItems => Response.Users?.Where(x => x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)).AsQueryable();
        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetUserList();
            if (result.Succeeded)
            {
                Response = result.Data;
            }
        }
        private void AddNew()
        {
            nameFilter = string.Empty;


            _NavigationManager.NavigateTo("/NewRegister");


        }
    }
}
