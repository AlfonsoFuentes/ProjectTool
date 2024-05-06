#nullable disable
namespace ClientRadzen.NewPages.Brands
{
    public partial class NewUpdateBrandPage
    {
        [Parameter]
        public Guid BrandId { get; set; }
        [CascadingParameter]
        private App MainApp { get; set; }
        NewBrandUpdateRequest Model { get; set; } = new();

        [Inject]
        private INewBrandService Service { get; set; } = null!;
        protected override async void OnInitialized()
        {
            var result = await Service.GetBrandToUpdateById(BrandId);
            if (result.Succeeded)
            {
                Model = result.Data;
            }

            StateHasChanged();
        }

        private async Task SaveAsync()
        {


            var result = await Service.UpdateBrand(Model);
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
            NotValidated = !await _fluentValidationValidator.ValidateAsync();
            return NotValidated;
        }
        bool NotValidated = true;


        public async Task ChangeName(string name)
        {
            Model.Name = name;
            await ValidateAsync();
        }
    }
}

