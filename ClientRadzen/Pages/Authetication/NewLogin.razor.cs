using Blazored.FluentValidation;
using Client.Infrastructure.Managers.UserManagement;
using Client.Infrastructure.Managers.VersionSoftwares;
using Microsoft.AspNetCore.Components;
using Shared.Models.UserAccounts.Logins;
using Shared.Models.UserAccounts.Registers;
using Shared.Models.UserManagements;
#nullable disable
namespace ClientRadzen.Pages.Authetication
{
    public partial class NewLogin
    {
        [CascadingParameter]
        private App MainApp { get; set; }
       
        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        [Inject]
        private INewSoftwareVersionService VersionSoftwareService { get; set; }
        protected override void OnInitialized()
        {
        
            Model.EmailConfirmed = true;
        }

        FluentValidationValidator _fluentValidationValidator = null!;
        bool NotValidated = false;
        async Task<bool> ValidateAsync()
        {
            NotValidated = !(await _fluentValidationValidator.ValidateAsync());
            return NotValidated;
        }
        LoginUserRequest Model = new();
        async Task LoginAsync()
        {
            var model = new UserForAuthenticationDto()
            {
                 Email=Model.Email,
                 Password=Model.Password,
            };
            var result = await AuthenticationService.Login(model);
            if (result.IsAuthSuccessful)
            {
                var resultVersion = await VersionSoftwareService.CheckVersionForUser(result);
                _NavigationManager.NavigateTo("/");
            }

        }
        async Task ChangePasswordAsync()
        {
            ChangePasswordUserRequest modelchangepassword = new()
            {
                Email = Model.Email,
                Password = Model.NewPassword,
            };
            var result = await AuthenticationService.ChangePasswordUser(modelchangepassword);
            if (result)
            {

                _NavigationManager.NavigateTo("/NewLogin", forceLoad: true);
            }

        }
        bool ShowButtons = false;
        async Task ValidateConfirmEmailPassword(string email)
        {
            var result = await AuthenticationService.ValidateIfEmailExist(email);
            if (result)
            {
                Model.EmailConfirmed = true;

                var resulpassword = await AuthenticationService.ValidateIfPasswordConfirmed(email);
                Model.PasswordConfirmed = resulpassword;
                ShowButtons = true;

            }
            else
            {
              
                Model.EmailConfirmed = false;
                Model.PasswordConfirmed = false;
                ShowButtons = true;
            }
          
            StateHasChanged();


        }

        
        public async Task ChangeEmail(string email)
        {
            Model.Email = email;
            await ValidateAsync();
        }
        public async Task ChangePassword(string password)
        {
            Model.Password = password;
            await ValidateAsync();
        }
        public async Task ChangeNewPassword(string password)
        {
            Model.Password = password;
            await ValidateAsync();
        }
    }
}
