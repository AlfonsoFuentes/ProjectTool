#nullable disable
using Blazored.FluentValidation;
using Client.Infrastructure.Managers.UserAccount;
using Microsoft.AspNetCore.Components;
using Shared.Models.UserAccounts.Registers;
using System.ComponentModel.DataAnnotations;

namespace ClientRadzen.Pages.Authetication
{
    public partial class NewRegister
    {
        [CascadingParameter]
        public App MainApp { get; set; }

        RegisterUserRequest Model = new();
        [Inject]
        private IUserAccountService Service { get; set; }

       
        async Task RegisterUserAsync()
        {
            var result = await Service.RegisterUser(Model);
            if(result.Succeeded)
            {
                _NavigationManager.NavigateTo("/");
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
