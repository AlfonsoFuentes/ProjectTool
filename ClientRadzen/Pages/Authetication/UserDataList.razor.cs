#nullable disable
using Client.Infrastructure.Managers.Suppliers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.Suppliers;
using Client.Infrastructure.Managers.Accounts;

namespace ClientRadzen.Pages.Authetication
{
    public partial class UserDataList
    {
        [Inject]
        private IAccountManager Service { get; set; } = null!;
        UsersResponse Model = new();

        List<CurrentUser> OriginalData => Model.Users;

        string nameFilter = string.Empty;
        IQueryable<CurrentUser> FilteredItems => OriginalData?.Where(x => x.UserName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)).AsQueryable();
        protected override async Task OnInitializedAsync()
        {


            await UpdateAll();
        }

        async Task UpdateAll()
        {
            var result = await Service.GetUsersAsync();
            if (result.Succeeded)
            {
                Model = result.Data;
             

            }
        }



        private void GoToHome()
        {
            _NavigationManager.NavigateTo("/");
        }

        void Edit(SupplierResponse Response)
        {
            OnDoubleClick(Response);

        }
        void EditByForm(SupplierResponse Response)
        {
            _NavigationManager.NavigateTo($"/UpdateSupplier/{Response.Id}");

        }
        async Task Delete(SupplierResponse response)
        {
            var resultDialog = await DialogService.Confirm($"Are you sure delete {response.Name}?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (resultDialog.Value)
            {
                //var result = await Service.Delete(response);
                //if (result.Succeeded)
                //{
                //    NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                //    await UpdateAll();
                //}
                //else
                //{
                //    NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
                //}

            }

        }
        SupplierResponse selectedRow = null!;
        void OnClick(SupplierResponse _selectedRow)
        {
            selectedRow = _selectedRow;
            EditRow = false;
        }
        bool EditRow = false;

        void OnDoubleClick(SupplierResponse _selectedRow)
        {
            EditRow = true;
            selectedRow = _selectedRow;


        }
        async Task AddAsync(SupplierResponse order)
        {
            CreateSupplierRequest Model = new()
            {
                Name = order.Name,

                Address = order.Address,
                ContactEmail = order.ContactEmail,
                ContactName = order.ContactName,
                PhoneNumber = order.PhoneNumber,
                SupplierCurrency = order.SupplierCurrency,
                TaxCodeLD = order.TaxCodeLD,
                TaxCodeLP = order.TaxCodeLP,
                VendorCode = order.VendorCode,

            };
            //var result = await Service.CreateSupplier(Model);
            //if (result.Succeeded)
            //{
            //    NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

            //    await UpdateAll();
            //    EditRow = false;
            //    AddRow = false;
            //    selectedRow = null!;
            //}
            //else
            //{
            //    NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);


            //}
        }

        async Task UpdateAsync(SupplierResponse order)
        {
            UpdateSupplierRequest Model = new()
            {
                Id = order.Id,
                Name = order.Name,
                Address = order.Address,
                ContactEmail = order.ContactEmail,
                ContactName = order.ContactName,
                PhoneNumber = order.PhoneNumber,
                SupplierCurrency = order.SupplierCurrency,
                TaxCodeLD = order.TaxCodeLD,
                TaxCodeLP = order.TaxCodeLP,
                VendorCode = order.VendorCode,
            };
            //var result = await Service.UpdateSupplier(Model);
            //if (result.Succeeded)
            //{
            //    NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);
            //    await UpdateAll();
            //    EditRow = false;
            //    selectedRow = null!;
            //}
            //else
            //{
            //    NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);

            //}
        }
        async Task CancelAsync()
        {
            await UpdateAll();
        }
        async Task SaveAsync(SupplierResponse order)
        {
            if (AddRow)
            {
                await AddAsync(order);
            }
            else
            {
                await UpdateAsync(order);
            }
        }
        async Task OnKeyDown(KeyboardEventArgs arg, SupplierResponse order)
        {
            if (arg.Key == "Enter")
            {
                await SaveAsync(order);


            }
            else if (arg.Key == "Escape")
            {
                EditRow = false;
            }
        }

        bool AddRow = false;
        private void AddNew()
        {
            nameFilter = string.Empty;
            AddRow = true;
            EditRow = true;
            selectedRow = new();

            //OriginalData.Insert(0, selectedRow);
        }
    }
}
