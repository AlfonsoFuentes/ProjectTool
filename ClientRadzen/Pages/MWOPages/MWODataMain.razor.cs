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
        async Task ExporToExcel()
        {
            var result = TabIndex == 0 ? await Service.ExportMWOsCreated() : TabIndex == 1 ? await Service.ExportMWOsApproved() : await Service.ExportMWOsClosed();
            if (result.Succeeded)
            {
                var downloadresult = await blazorDownloadFileService.DownloadFile(result.Data.ExportFileName,
                   result.Data.Data, contentType: result.Data.ContentType);
                if (downloadresult.Succeeded)
                {
                    MainApp.NotifyMessage(NotificationSeverity.Success, "Export Excel", new() { "Export excel succesfully" });


                }
            }


        }

        public void EditByForm(MWOCreatedResponse Response)
        {
            _NavigationManager.NavigateTo($"/UpdateMWO/{Response.Id}");

        }
        public void AddItemToMWO(MWOCreatedResponse Response)
        {
            _NavigationManager.NavigateTo($"/CreateBudgetItem/{Response.Id}");

        }
        public void ShowItemsofMWO(MWOCreatedResponse Response)
        {
            _NavigationManager.NavigateTo($"/BudgetItemsDataList/{Response.Id}");

        }
        public void ShowApprovedItemsofMWO(MWOApprovedResponse Response)
        {
            _NavigationManager.NavigateTo($"/MWOApproved/{Response.Id}");

        }
        public void ShowSapAlignmentofMWO(MWOApprovedResponse Response)
        {
            _NavigationManager.NavigateTo($"/SapAdjustListByMWO/{Response.Id}");

        }


    }
}
