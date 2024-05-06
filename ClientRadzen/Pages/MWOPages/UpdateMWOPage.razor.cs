#nullable disable
using Shared.Enums.MWOTypes;

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

       
        public async Task ChangeName(string name)
        {

            Model.Name = name;

            await ValidateAsync();
        }
        public async Task ChangeType(MWOTypeEnum type)
        {
            Model.Type = type;
            await ValidateAsync();
        }

        public async Task ChangePercentageTaxes(string stringpercentage)
        {

            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
            {

            }

            Model.PercentageAssetNoProductive = percentage;
            await ValidateAsync();
        }
        public async Task ChangePercentageEngineering(string stringpercentage)
        {

            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
            {

            }

            Model.PercentageEngineering = percentage;
            await ValidateAsync();

        }
        public async Task ChangePercentageContingency(string stringpercentage)
        {

            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
            {

            }


            Model.PercentageContingency = percentage;
            await ValidateAsync();

        }
        public async Task ChangeTaxForAlterations(string stringpercentage)
        {

            double percentage = 0;
            if (!double.TryParse(stringpercentage, out percentage))
            {

            }

            Model.PercentageTaxForAlterations = percentage;
            await ValidateAsync();

        }
        public async Task ChangeType()
        {

            await ValidateAsync();
        }
    }
}
