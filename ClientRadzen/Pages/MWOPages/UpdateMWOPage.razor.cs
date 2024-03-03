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
            var result = await Service.UpdateMWO(Model);
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
