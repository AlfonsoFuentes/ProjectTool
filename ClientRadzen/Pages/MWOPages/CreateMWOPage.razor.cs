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
    


      
        [Inject]
        private IMWOService Service { get; set; } = null!;

        private async Task SaveAsync()
        {
            var result = await Service.CreateMWO(Model);
            if (result.Succeeded)
            {
                NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                _NavigationManager.NavigateTo("/MWODataList");
            }
            else
            {
                Model.ValidationErrors = result.Messages;
                NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }

        }
       
        private void CancelAsync()
        {
            _NavigationManager.NavigateTo("/MWODataList");
        }

    }
}
