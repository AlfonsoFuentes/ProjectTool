#nullable disable
using Blazored.FluentValidation;
using Client.Infrastructure.Managers.UserAccount;
using Microsoft.AspNetCore.Components;
using Shared.Models.UserAccounts.Registers;

namespace ClientRadzen.Pages.Authetication
{
    public partial class NewRegister:IDisposable
    {
        [CascadingParameter]
        public App MainApp { get; set; }

        RegisterUserRequest Model = new();
        [Inject]
        private IUserAccountService Service { get; set; }

        protected override void OnInitialized()
        {
            Model.Validator += ValidateAsync;
        }
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

        public void Dispose()
        {
            Model.Validator -= ValidateAsync;
        }
    }
}
