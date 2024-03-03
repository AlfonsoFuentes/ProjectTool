#nullable disable
using Azure;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.BudgetItems;
using Shared.Models.MWOTypes;

namespace ClientRadzen.Pages.MWOPages
{
    public partial class MWODataList
    {
        [Inject]
        public IMWOService Service { get; set; }

        List<MWOResponse> OriginalData { get; set; } = new();

        string nameFilter = string.Empty;
        Func<MWOResponse, bool> fiterexpresion => x =>
       x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||

       x.Type.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase);
        IQueryable<MWOResponse> FilteredItems => OriginalData?.Where(fiterexpresion).AsQueryable();
        protected override async Task OnInitializedAsync()
        {


            await UpdateAll();
        }

        public async Task UpdateAll()
        {
            var result = await Service.GetAllMWO();
            if (result.Succeeded)
            {
                OriginalData = result.Data;

            }
            StateHasChanged();
        }



        private void GoToHome()
        {
            _NavigationManager.NavigateTo("/");
        }

        


        MWOResponse selectedRow = null!;

      
        async Task CancelAsync()
        {
            await UpdateAll();
        }
      
        private void AddNew()
        {
            nameFilter = string.Empty;
           
       
            _NavigationManager.NavigateTo("/CreateMWO");

      
        }
    }
}
