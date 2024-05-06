
#nullable disable
namespace ClientRadzen.NewPages.Suppliers
{
    public partial class NewCreateSupplierDialog
    {
        [CascadingParameter]
        private App MainApp { get; set; }
        NewSupplierCreateBasicRequest Model { get; set; } = new();

        [Inject]
        private INewSupplierService Service { get; set; } = null!;

        private async Task SaveAsync()
        {


            var result = await Service.CreateSupplierAndReponse(Model);
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



        private void CancelAsync()
        {
            DialogService.Close(false);
        }
        public async Task ChangeName(string name)
        {
            Model.Name = name;
            await ValidateAsync();
        }
        public async Task ChangeNickName(string name)
        {
            Model.NickName = name;
            await ValidateAsync();
        }
        public async Task ChangeVendorCode(string name)
        {
            Model.VendorCode = name;
            await ValidateAsync();
        }
        public async Task ChangeSupplierCurrency()
        {
            await ValidateAsync();
        }
    }
}
