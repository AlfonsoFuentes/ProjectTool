#nullable disable
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClientRadzen.Pages.MWOPages
{
    public partial class UpdateMWOPage
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        private IMWOService Service { get; set; }
      
        UpdateMWORequest Model { get; set; } = new();
        FluentValidationValidator _fluentValidationValidator = null!;
        string mwoName=string.Empty;
        protected override async Task OnInitializedAsync()
        {
            var result=await Service.GetMWOById(Id);
            if(result.Succeeded)
            {
                Model = result.Data;
                mwoName=Model.Name;
            }
        }
        private async Task SaveAsync()
        {
            if (await _fluentValidationValidator!.ValidateAsync())
            {

                var result = await Service.UpdateMWO(Model);
                if (result.Succeeded)
                {
                    ShowNotification(new NotificationMessage
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
                    ShowNotification(new NotificationMessage { Severity = NotificationSeverity.Error,
                        Summary = "Error Summary", Detail = result.Message, Duration = 4000 });
                }
            }

        }
        void ShowNotification(NotificationMessage message)
        {
            NotificationService.Notify(message);

           
        }

        private void CancelAsync()
        {
            _NavigationManager.NavigateTo("/mwotable");
        }
    }
}
