

#nullable disable
namespace ClientRadzen.NewPages.Brands
{
    public partial class NewCreateBrandDialog
    {
        [CascadingParameter]
        private App MainApp { get; set; }
        NewBrandCreateRequest Model { get; set; } = new();

        [Inject]
        private INewBrandService Service { get; set; } = null!;


        private async Task SaveAsync()
        {


            var result = await Service.CreateBrandForBudgetItem(Model);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                DialogService.Close(result.Data);
            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }


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

        private void CancelAsync()
        {
            DialogService.Close(false);
        }
    }
}
