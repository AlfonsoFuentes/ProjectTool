#nullable disable
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
       
     
        void ChangeIndex(int index)
        {
            MainApp.TabIndexMWO = index;
        }
        protected override async Task OnInitializedAsync()
        {
       
            await UpdateAll();
        }
        async Task UpdateAll()
        {
            var result = await Service.GetAllMWO();
            if (result.Succeeded)
            {
                Response = result.Data;

            }
        }
        private void AddNew()
        {
            nameFilter = string.Empty;
            _NavigationManager.NavigateTo("/CreateMWO");


        }
        async Task ExporToExcel()
        {
            var result = MainApp.TabIndexMWO == 0 ? await Service.ExportMWOsCreated() : 
                MainApp.TabIndexMWO == 1 ? await Service.ExportMWOsApproved() : 
                await Service.ExportMWOsClosed();
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
       public async Task UnApproveMWO(MWOApprovedResponse Response)
        {
            UnApproveMWORequest UnApproveMWORequest = new()
            {
                MWOId = Response.Id,
                Name = Response.Name,
                CECName = Response.CECName,
            };
            var resultDialog = await DialogService.Confirm($"Are you sure Un Approve {UnApproveMWORequest.Name}?", "Confirm",
               new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (resultDialog.Value)
            {

                var result = await Service.UnApproveMWO(UnApproveMWORequest);
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
