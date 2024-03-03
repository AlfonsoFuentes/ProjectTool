#nullable disable
using Blazored.FluentValidation;
using Client.Infrastructure.Managers.Suppliers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.Suppliers;

namespace ClientRadzen.Pages.Suppliers
{
    public partial class UpdateSupplierPage
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        private ISupplierService Service { get; set; }

        UpdateSupplierRequest Model { get; set; } = new();
     
        string SupplierName = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetSupplierById(Id);
            if (result.Succeeded)
            {
                Model = result.Data;
                SupplierName = Model.Name;
            }
        }
        private async Task SaveAsync()
        {
            

                var result = await Service.UpdateSupplier(Model);
                if (result.Succeeded)
                {
                    NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);
                    _NavigationManager.NavigateTo("/SupplierDatalist");
                }
                else
                {
                    NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
                }
           

        }

        private void CancelAsync()
        {
            _NavigationManager.NavigateTo("/SupplierDatalist");
        }
    }
}
