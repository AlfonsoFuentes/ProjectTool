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
        [CascadingParameter]
        private App MainApp { get; set; }
        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        private IMWOService Service { get; set; }
      
        UpdateMWORequest Model { get; set; } = new();
      
       
        protected override async Task OnInitializedAsync()
        {
            var result=await Service.GetMWOToUpdateById(Id);
            if(result.Succeeded)
            {
                Model = result.Data;
                Model.Validator += ValidateAsync;
            }
        }
        private async Task SaveAsync()
        {
            var result = await Service.UpdateMWO(Model);
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
