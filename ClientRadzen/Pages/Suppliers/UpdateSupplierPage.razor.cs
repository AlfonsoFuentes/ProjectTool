#nullable disable
using Blazored.FluentValidation;
using Client.Infrastructure.Managers.Suppliers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.Suppliers;
using System.ComponentModel.DataAnnotations;

namespace ClientRadzen.Pages.Suppliers
{
    public partial class UpdateSupplierPage
    {
        [Parameter]
        public Guid SupplierId { get; set; }

        [CascadingParameter]
        private App MainApp { get; set; }
        UpdateSupplierRequest Model { get; set; } = new();

        [Inject]
        private ISupplierService Service { get; set; } = null!;
        protected override async void OnInitialized()
        {
            var result=await Service.GetSupplierById(SupplierId);
            if(result.Succeeded)
            {
                Model = result.Data;
            }
         
            StateHasChanged();
        }

        private async Task SaveAsync()
        {


            var result = await Service.UpdateSupplier(Model);
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
            NotValidated = !(await _fluentValidationValidator.ValidateAsync());
            return NotValidated;
        }
        bool NotValidated = true;

       
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
        public async Task ChangeEmail(string name)
        {
            Model.ContactEmail = name;
            await ValidateAsync();
        }
        public async Task ChangeSupplierCurrency()
        {
            await ValidateAsync();
        }
    }
}
