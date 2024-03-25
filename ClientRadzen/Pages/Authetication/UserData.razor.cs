using ClientRadzen.Pages.MWOPages;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.MWOStatus;
using Shared.Models.Registers;
using Client.Infrastructure.IdentityModels;

namespace ClientRadzen.Pages.Authetication;
#nullable disable
public partial class UserData
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public CurrentUser Data { get; set; }
    [CascadingParameter]
    private UserDataList MainPage { get; set; }

    CurrentUser selectedRow { get; set; }
    bool EditRow = false;

    [Inject]
    private ICookieAuthenticationStateProvider Acct { get; set; }

    void OnDoubleClick(CurrentUser _selectedRow)
    {
        selectedRow = _selectedRow;
        EditRow = true;




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
    async Task SaveAsync(CurrentUser order)
    {
        if (EditRow)
        {
            await UpdateAsync(order);
        }

    }
    async Task RegisterAsync(CurrentUser order)
    {
        RegisterRequest Input = new()
        {
            Email = order.UserName,
            Role = order.Role,
        };

        var result = await Acct.RegisterAsync(Input);
        if(result.Succeeded)
        {
            MainPage.AddRow = false;
            MainPage.selectedRow = null;
            await MainPage.UpdateAll();
        }
        

    }
    async Task CancelAsync()
    {
        EditRow = false;
        await MainPage.UpdateAll();
    }


    async Task UpdateAsync(CurrentUser order)
    {
        //UpdateMWOMinimalRequest Model = new()
        //{
        //    Id = order.Id,
        //    Name = order.Name,
        //    Type = order.MWOType,


        //};
        //var result = await MainPage.Service.UpdateMWOMinimal(Model);
        //if (result.Succeeded)
        //{
        //    NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);
        //    EditRow = false;

        //    selectedRow = null!;
        //    await MainPage.UpdateAll();

        //}
        //else
        //{
        //    NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);

        //}
    }
    void Edit(CurrentUser Response)
    {
        OnDoubleClick(Response);

    }

    async Task Delete(CurrentUser response)
    {

        var resultDialog = await DialogService.Confirm($"Are you sure delete {response.UserName}?", "Confirm Delete",
            new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No", CloseDialogOnEsc = true, CloseDialogOnOverlayClick = true });
        if (resultDialog.Value)
        {
            var result = await MainPage.Service.Delete(response);
            if (result.Succeeded)
            {
                MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

                await MainPage.UpdateAll();
            }
            else
            {
                MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);
            }

        }


    }

}
