#nullable disable
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.ComponentModel.DataAnnotations;

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
               
            }
  
            if(string.IsNullOrEmpty(Model.MWONumber))
            {
                NotValidated = true;
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
        bool NotValidated = false;

       
        public async Task ChangeMWONumber(string _mwonumber)
        {

            Model.MWONumber = _mwonumber;
            await ValidateAsync();
        }

        public async Task ChangePercentageTaxes(string stringpercentage)
        {

            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
                return;

            Model.PercentageAssetNoProductive = percentage;
            await ValidateAsync();
        }
        public async Task ChangePercentageEngineering(string stringpercentage)
        {

            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
                return;

            Model.PercentageEngineering = percentage;
            await ValidateAsync();
        }
        public async Task ChangeTaxForAlterations(string stringpercentage)
        {

            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
                return;

            Model.PercentageTaxForAlterations = percentage;
            await ValidateAsync();

        }
        public async Task ChangePercentageContingency(string stringpercentage)
        {

            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
                return;


            Model.PercentageContingency = percentage;
            await ValidateAsync();

        }
        public async Task ChangeName(string name)
        {

            Model.Name = name;
            await ValidateAsync();

        }
        public async Task ChangeCostCenter()
        {
            await ValidateAsync();
        }
        public async Task ChangeType()
        {

            await ValidateAsync();
        }
    }
}
