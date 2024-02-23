#nullable disable
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Radzen;
using Client.Infrastructure.Managers.MWOS;
namespace ClientRadzen.Pages.MWOPages
{
    public partial class CreateMWOPage
    {
        CreateMWORequest Model { get; set; } = new();
    


        FluentValidationValidator _fluentValidationValidator = null!;
        [Inject]
        private IMWOService Service { get; set; } = null!;

        private async Task SaveAsync()
        {
            if (await _fluentValidationValidator!.ValidateAsync())
            {

                var result = await Service.CreateMWO(Model);
                if (result.Succeeded)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Summary = "Success",
                        Detail = result.Message,
                        Duration = 4000
                    });

                    _NavigationManager.NavigateTo("/mwotable");
                }
                else
                {
                    Model.ValidationErrors = result.Messages;
                }
            }

        }
       
        private void CancelAsync()
        {
            _NavigationManager.NavigateTo("/mwotable");
        }

    }
}
