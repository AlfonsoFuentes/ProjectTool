using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Shared.Models.MWOStatus;
using Radzen;

#nullable disable
namespace ClientRadzen.Pages.MWOPages;
public partial class MWODataCreated
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public MWODataMain DataMain { get; set; }
    [Parameter]
    public MWOCreatedResponse Data { get; set; }
    [CascadingParameter]
    private MWODataMain MainPage { get; set; }
    public async Task Delete(MWOCreatedResponse response)
    {

        var resultDialog = await DialogService.Confirm($"Are you sure delete {response.Name}?", "Confirm Delete",
            new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No", CloseDialogOnEsc = true, CloseDialogOnOverlayClick = true });
        if (resultDialog.Value)
        {
            var result = await MainPage.Service.Delete(response);
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
    async Task UpdateAll()
    {
        var result = await MainPage.Service.GetAllMWO();
        if(result.Succeeded)
        {
            MainPage.Response = result.Data;
        }

    }
    public async Task Approve(MWOCreatedResponse Response)
    {
        var resultDialog = await DialogService.Confirm($"Are you sure Approved {Response.Name}?", "Confirm",
            new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
        if (resultDialog.Value)
        {
            _NavigationManager.NavigateTo($"/ApproveMWO/{Response.Id}");

        }

    }
}
