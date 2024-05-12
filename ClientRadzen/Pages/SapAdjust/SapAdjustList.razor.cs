#nullable disable
using Client.Infrastructure.Managers.PurchaseOrders;
using Client.Infrastructure.Managers.SapAdjusts;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.BudgetItems;
using Shared.Models.SapAdjust;

namespace ClientRadzen.Pages.SapAdjust;
public partial class SapAdjustList
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Inject]
    private ISapAdjustService Service { get; set; }

    string nameFilter { get; set; } = string.Empty;
    SapAdjustResponseList Response { get; set; } = new();
    [Parameter]
    public Guid MWOId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await UpdateAll();
    }
    async Task UpdateAll()
    {
        var result = await Service.GetAllSapAdjustByMWO(MWOId);
        if (result.Succeeded)
        {
            Response = result.Data;
        }

    }
    SapAdjustResponseList seletedRow = null!;
    async Task CreateAdjust()
    {
        DateTime NOW = DateTime.Now;
        if (Response.Adjustments.Any(x => x.Date.Date == NOW.Date))
        {
            var resultDialog = await DialogService.Confirm($"Data for this date {NOW.Date.ToShortDateString()} already exist, do you want to update this date?", "Confirm Delete",
              new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (resultDialog.Value)
            {
                var currentDate = Response.Adjustments.SingleOrDefault(x => x.Date.Date == NOW.Date);
                _NavigationManager.NavigateTo($"/UpdateAdjustForMWO/{currentDate.SapAdjustId}");
            }
        }
        else
        {
            _NavigationManager.NavigateTo($"/CreateAdjustForMWO/{MWOId}");
        }

    }


    void Edit(SapAdjustResponse approvedResponse)
    {
        _NavigationManager.NavigateTo($"/UpdateAdjustForMWO/{approvedResponse.SapAdjustId}");

    }
    async Task Remove(SapAdjustResponse approvedResponse)
    {
        var resultDialog = await DialogService.Confirm($"Are you sure delete {approvedResponse.Date.ToShortDateString()}?", "Confirm Delete",
               new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {
            var result = await Service.Delete(approvedResponse);
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
