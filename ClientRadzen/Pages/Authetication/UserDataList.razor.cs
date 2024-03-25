#nullable disable
using Client.Infrastructure.Managers.Suppliers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.Suppliers;
using Client.Infrastructure.Managers.Accounts;
using Shared.Models.Brands;

namespace ClientRadzen.Pages.Authetication
{
    public partial class UserDataList
    {
        [CascadingParameter]
        public App MainApp { get; set; }
        [Inject]
        public IAccountManager Service { get; set; } = null!;
        UsersResponse Model = new();

        List<CurrentUser> OriginalData => Model.Users;

        string nameFilter = string.Empty;
        IQueryable<CurrentUser> FilteredItems => OriginalData?.Where(x => x.UserName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)).AsQueryable();
        protected override async Task OnInitializedAsync()
        {


            await UpdateAll();
        }

        public async Task UpdateAll()
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


        public CurrentUser selectedRow = null!;
        void OnClick(CurrentUser _selectedRow)
        {
            selectedRow = _selectedRow;
            EditRow = false;
        }
        bool EditRow = false;

        void OnDoubleClick(CurrentUser _selectedRow)
        {
            EditRow = true;
            selectedRow = _selectedRow;


        }
        async Task AddAsync(CurrentUser order)
        {
            //CreateBrandRequest Model = new()
            //{
            //    Name = order.Name,

            //};
            //var result = await Service.CreateBrand(Model);
            //if (result.Succeeded)
            //{
            //    NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

            //    await UpdateAll();
            //    EditRow = false;
            //    Add = false;
            //    selectedRow = null!;
            //}
            //else
            //{
            //    NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);


            //}
        }

        async Task UpdateAsync(CurrentUser order)
        {
            //UpdateBrandRequest Model = new()
            //{
            //    Name = order.Name,
            //    Id = order.Id,
            //};
            //var result = await Service.UpdateBrand(Model);
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
        async Task SaveAsync(CurrentUser order)
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
        async Task OnKeyDown(KeyboardEventArgs arg, CurrentUser order)
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

        public bool AddRow = false;
        private void AddNew()
        {
            nameFilter = string.Empty;


            _NavigationManager.NavigateTo("/register");


        }

    }
}
