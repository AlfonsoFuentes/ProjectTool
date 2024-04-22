using Client.Infrastructure.Managers.BudgetItems;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.PurchaseOrders.Responses;
using Microsoft.AspNetCore.Components.Forms;
using Shared.Models.Suppliers;
using Microsoft.AspNetCore.Components.Web;
using System.Linq.Expressions;
#nullable disable
namespace ClientRadzen.Pages.BudgetItems
{
    public partial class BudgetItemsDataList
    {
        [CascadingParameter]
        public App MainApp { get; set; }
        [Inject]
        private IBudgetItemService Service { get; set; }


        ListBudgetItemResponse Response { get; set; } = new();
        string nameFilter = string.Empty;

        [Parameter]
        public Guid MWOId { get; set; }
        Func<BudgetItemResponse, bool> fiterexpresion => x =>
        x.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
        x.Nomenclatore.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
        x.Brand.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
        x.Type.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase);
        IQueryable<BudgetItemResponse> FilteredItems => Response.BudgetItems?.Where(fiterexpresion).AsQueryable();
        protected override async Task OnInitializedAsync()
        {


          

            await UpdateAll();
        }

        async Task UpdateAll()
        {
            var result = await Service.GetAllBudgetItemByMWO(MWOId);

            if (result.Succeeded)
            {
                Response = result.Data;


            }
        }
        private void AddNewBudgetItem()
        {
            _NavigationManager.NavigateTo($"/CreateBudgetItem/{MWOId}");
        }





        async Task Delete(BudgetItemResponse BudgetItemResponse)
        {
            if (BudgetItemResponse.IsNotAbleToEditDelete)
            {
                await DialogService.Alert($"Can not remove {BudgetItemResponse.Name}", "Project Tool", new AlertOptions() { OkButtonText = "Yes" });

                return;
            }
            var resultDialog = await DialogService.Confirm($"Are you sure delete {BudgetItemResponse.Name}?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (resultDialog.Value)
            {
                var result = await Service.Delete(BudgetItemResponse);
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


        async Task Approve()
        {
            var resultDialog = await DialogService.Confirm($"Are you want to approved {Response.MWO.Name}?", "Confirm",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (resultDialog.Value)
            {
                _NavigationManager.NavigateTo($"/ApproveMWO/{Response.MWO.Id}");

            }

        }

        void EditByForm(BudgetItemResponse Response)
        {
            _NavigationManager.NavigateTo($"/UpdateBudgetItem/{Response.Id}");

        }
        async Task ExporNotApprovedToExcel()
        {
            var result = await Service.ExporNotApprovedToExcel(MWOId);
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

    }
}
