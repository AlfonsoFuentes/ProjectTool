using Client.Infrastructure.Managers.Brands;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Shared.Commons.Results;
using Shared.Models.Brands;
using Shared.Models.PurchaseOrders.Requests.Create;
using static System.Net.Mime.MediaTypeNames;
#nullable disable
namespace ClientRadzen.Pages.Brands
{
    public partial class BrandListData
    {
        [Inject]
        private IBrandService Service { get; set; }
  
        List<BrandResponse> OriginalData { get; set; } = new();

        string nameFilter = string.Empty;
        IQueryable<BrandResponse> FilteredItems => OriginalData?.Where(x => x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)).AsQueryable();
        protected override async Task OnInitializedAsync()
        {
            await UpdateAll();
        }
       

      
        async Task UpdateAll()
        {
            var result = await Service.GetAllBrand();
            if (result.Succeeded)
            {
                OriginalData = result.Data;

            }
        }
      
        private void GoToHome()
        {
            _NavigationManager.NavigateTo("/");
        }

        void Edit(BrandResponse BrandResponse)
        {
            OnDoubleClick(BrandResponse);

        }
        async Task Delete(BrandResponse BrandResponse)
        {
            var resultDialog = await DialogService.Confirm($"Are you sure delete {BrandResponse.Name}?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (resultDialog.Value)
            {
                var result = await Service.Delete(BrandResponse);
                if (result.Succeeded)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Summary = "Success",
                        Detail = result.Message,
                        Duration = 4000
                    });

                    await UpdateAll();
                }
                else
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Error",
                        Detail = result.Message,
                        Duration = 4000
                    });
                }

            }

        }
        BrandResponse selectedRow = null!;
        void OnClick(BrandResponse _selectedRow)
        {
            selectedRow = _selectedRow;
            EditRow = false;
        }
        bool EditRow = false;
         
        void OnDoubleClick(BrandResponse _selectedRow)
        {
            EditRow = true;
            selectedRow = _selectedRow;
            
          
        }
        async Task AddAsync(BrandResponse order)
        {
            CreateBrandRequest Model = new()
            {
                Name = order.Name,
              
            };
            var result = await Service.CreateBrand(Model);
            if (result.Succeeded)
            {
                 NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);
               
                await UpdateAll();
                EditRow = false;
                Add = false;
                selectedRow = null!;
            }
            else
            {
                NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
               

            }
        }
       
        async Task UpdateAsync(BrandResponse order)
        {
            UpdateBrandRequest Model = new()
            {
                Name = order.Name,
                Id = order.Id,
            };
            var result = await Service.UpdateBrand(Model);
            if (result.Succeeded)
            {
                NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);
                await UpdateAll();
                EditRow = false;
                selectedRow = null!;
            }
            else
            {
                NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);

            }
        }
        async Task CancelAsync()
        {
            await UpdateAll();
        }
        async Task SaveAsync(BrandResponse order)
        {
            if (Add)
            {
                await AddAsync(order);
            }
            else
            {
                await UpdateAsync(order);
            }
        }
        async Task OnKeyDown(KeyboardEventArgs arg, BrandResponse order)
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
        
        bool Add = false;
        private void AddNew()
        {
            nameFilter = string.Empty;
            Add = true;
            EditRow = true;
            selectedRow = new();
          
            OriginalData.Insert(0,selectedRow);
        }
    }
}
