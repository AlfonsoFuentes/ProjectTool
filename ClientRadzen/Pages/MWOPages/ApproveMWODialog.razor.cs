﻿#nullable disable
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace ClientRadzen.Pages.MWOPages
{
    public partial class ApproveMWODialog
    {
        [CascadingParameter]
        private App MainApp { get; set; }
        [Parameter]
        public Guid MWOId { get; set; }
        [Inject]
        private IMWOService Service { get; set; } = null!;
        public ApproveMWORequest Model { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetMWOByIdToApprove(MWOId);
            if (result.Succeeded)
            {
                Model = result.Data;
                Model.Validator += ValidateAsync;
            }

        }
      
        public async Task SaveAsync()
        {
            var result = await Service.ApproveMWO(Model);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                _NavigationManager.NavigateTo($"/MWOApproved/{MWOId}");
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
