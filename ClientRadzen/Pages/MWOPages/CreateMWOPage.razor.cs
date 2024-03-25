#nullable disable
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Radzen;
using Client.Infrastructure.Managers.MWOS;
namespace ClientRadzen.Pages.MWOPages
{
    public partial class CreateMWOPage
    {
        [CascadingParameter]
        private App MainApp { get; set; }
        CreateMWORequest Model { get; set; } = new();

        protected override void OnInitialized()
        {
            Model.Validator += ValidateAsync;
        }
        [Inject]
        private IMWOService Service { get; set; } = null!;

        private async Task SaveAsync()
        {
            var result = await Service.CreateMWO(Model);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                CancelAsync();
            }
            else
            {

                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }

        }

        private void CancelAsync()
        {
            Navigation.NavigateBack();

        }
        FluentValidationValidator _fluentValidationValidator = null!;
        async Task<bool> ValidateAsync()
        {
            try
            {
                NotValidated = !(await _fluentValidationValidator.ValidateAsync());
                return NotValidated;
            }
            catch (Exception ex)
            {
                string exm = ex.Message;
            }
            return false;

        }
        bool NotValidated = true;

        public void Dispose()
        {
            Model.Validator -= ValidateAsync;
        }

    }
}
