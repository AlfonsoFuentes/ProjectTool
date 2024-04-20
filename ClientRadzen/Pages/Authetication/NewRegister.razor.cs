#nullable disable
using Blazored.FluentValidation;
using Client.Infrastructure.Managers.UserManagement;
using Microsoft.AspNetCore.Components;
using Shared.Models.UserAccounts.Registers;
using Shared.Models.UserManagements;

namespace ClientRadzen.Pages.Authetication
{
    public partial class NewRegister
    {
        [CascadingParameter]
        public App MainApp { get; set; }

        RegisterUserRequest Model = new();
        

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }
        async Task RegisterUserAsync()
        {
           
            UserForRegistrationDto _userForRegistration = new UserForRegistrationDto()
            {
                Email = Model.Email,
                Password = Model.Password,
                ConfirmPassword = Model.Password
                
            };
            var result2 = await AuthenticationService.RegisterUser(_userForRegistration);
            if (result2.IsSuccessfulRegistration)
            {
                _NavigationManager.NavigateTo("/NewUserList");
            }
        }
        FluentValidationValidator _fluentValidationValidator = null!;
        bool NotValidated = true;
        async Task<bool> ValidateAsync()
        {
            NotValidated=!(await _fluentValidationValidator.ValidateAsync());

            return NotValidated;
        }

       
        public async Task ChangeRole()
        {
            await ValidateAsync();
        }
        public async Task ChangeEmail(string email)
        {
            Model.Email = email;
            await ValidateAsync();
        }
    }
}
