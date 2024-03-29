#nullable disable
using Azure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using Shared.Models.BudgetItems;
using Shared.Models.MWO;
using Shared.Models.MWOTypes;
using Shared.Models.Views;

namespace ClientRadzen.Pages.MWOPages
{
    public partial class MWODataMain
    {
        [CascadingParameter]
        public App MainApp { get; set; }
        [Inject]
        public IMWOService Service { get; set; }

        public MWOResponseList Response { get; set; } = new();
       

        string nameFilter = string.Empty;
       
        protected override async Task OnInitializedAsync()
        {
            var result = await Service.GetAllMWO();
            if (result.Succeeded)
            {
                Response = result.Data;

            }
            StateHasChanged();
        }

        private void AddNew()
        {
            nameFilter = string.Empty;
           _NavigationManager.NavigateTo("/CreateMWO");

      
        }
       
      
        public void EditByForm(MWOResponse Response)
        {
            _NavigationManager.NavigateTo($"/UpdateMWO/{Response.Id}");

        }
        public void AddItemToMWO(MWOResponse Response)
        {
            _NavigationManager.NavigateTo($"/CreateBudgetItem/{Response.Id}");

        }
        public void ShowItemsofMWO(MWOResponse Response)
        {
            _NavigationManager.NavigateTo($"/BudgetItemsDataList/{Response.Id}");

        }
        public void ShowApprovedItemsofMWO(MWOResponse Response)
        {
            _NavigationManager.NavigateTo($"/MWOApproved/{Response.Id}");

        }
        public void ShowSapAlignmentofMWO(MWOResponse Response)
        {
            _NavigationManager.NavigateTo($"/SapAdjustListByMWO/{Response.Id}");

        }


    }
}
