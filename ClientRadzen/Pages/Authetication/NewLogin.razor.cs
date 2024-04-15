using Blazored.FluentValidation;
using Client.Infrastructure.Authentication;
using Client.Infrastructure.Managers.UserAccount;
using Microsoft.AspNetCore.Components;
using Shared.Models.UserAccounts.Logins;
using Shared.Models.UserAccounts.Registers;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace ClientRadzen.Pages.Authetication
{
    public partial class NewLogin
    {
        [CascadingParameter]
        private App MainApp { get; set; }
        [Inject]
        private IUserAccountService Service { get; set; }

        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; }
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
            var result = await Service.LoginUser(Model);
            if (result.Succeeded)
            {
                var customAuthStateProvider = (CustomAuthenticationStateProvider)AuthStateProvider;
                await customAuthStateProvider.UpdateAuthenticationState(result.Data.Token);
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
            var result = await Service.ChangePasswordUser(modelchangepassword);
            if (result.Succeeded)
            {

                _NavigationManager.NavigateTo("/NewLogin", forceLoad: true);
            }

        }
        async Task RegisterSuperAdminAsync()
        {
            RegisterSuperAdminUserRequest model = new();
            var result = await Service.RegisterSuperAdminUser(model);
            if (result.Succeeded)
            {

            }

        }
        async Task ValidateConfirmEmailPassword(string email)
        {
            var result = await Service.ValidateIfEmailExist(email);
            if (result.Succeeded)
            {
                Model.EmailConfirmed = true;

                var resulpassword = await Service.ValidateIfPasswordConfirmed(email);
                Model.PasswordConfirmed = resulpassword.Succeeded;
                

            }
            else
            {
                Model.EmailConfirmed = false;
                Model.PasswordConfirmed = false;
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
