#nullable disable
using Client.Infrastructure.Managers.VersionSoftwares;
using Shared.NewModels.SoftwareVersion;

namespace ClientRadzen.Pages.Authetication
{
    public partial class NewSoftwareVersionPage
    {
        [CascadingParameter]
        private App MainApp { get; set; }
        NewSoftwareVersionCreateRequest Model { get; set; } = new();

        [Inject]
        private INewSoftwareVersionService Service { get; set; } = null!;


        private async Task SaveAsync()
        {


            var result = await Service.CreateVersion(Model);
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
            NotValidated = !await _fluentValidationValidator.ValidateAsync();
            return NotValidated;
        }
        bool NotValidated = true;

        public async Task ChangeName(string name)
        {
            Model.Name = name;
            await ValidateAsync();
        }

    }

}

