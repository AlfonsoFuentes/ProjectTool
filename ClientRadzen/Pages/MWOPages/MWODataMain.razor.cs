#nullable disable
using Azure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using Shared.Models.BudgetItems;
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

        List<MWOResponse> OriginalData { get; set; } = new();

        string nameFilter = string.Empty;
        Func<MWOResponse, bool> fiterexpresion => x =>
       x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||

       x.Type.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase);
        public IQueryable<MWOResponse> FilteredItems => OriginalData?.Where(fiterexpresion).AsQueryable();
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

        private void AddNew()
        {
            nameFilter = string.Empty;
           
       
            _NavigationManager.NavigateTo("/CreateMWO");

      
        }
       
        public async Task Approve(MWOResponse Response)
        {
            var resultDialog = await DialogService.Confirm($"Are you sure Approved {Response.Name}?", "Confirm",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (resultDialog.Value)
            {
                _NavigationManager.NavigateTo($"/ApproveMWO/{Response.Id}");

            }

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
        public async Task Delete(MWOResponse response)
        {

            var resultDialog = await DialogService.Confirm($"Are you sure delete {response.Name}?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No", CloseDialogOnEsc = true, CloseDialogOnOverlayClick = true });
            if (resultDialog.Value)
            {
                var result = await Service.Delete(response);
                if (result.Succeeded)
                {
                    MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                    await UpdateAll();
                }
                else
                {
                    MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
                }

            }


        }
        
    }
}
