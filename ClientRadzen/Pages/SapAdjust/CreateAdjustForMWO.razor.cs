using Blazored.FluentValidation;
using Client.Infrastructure.Managers.SapAdjusts;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.SapAdjust;
#nullable disable
namespace ClientRadzen.Pages.SapAdjust;
public partial class CreateAdjustForMWO
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Inject]
    private ISapAdjustService Service { get; set; }
    [Inject]
    private IMWOService MWOService { get; set; }
    [Parameter]
    public Guid MWOId { get; set; }
    CreateSapAdjustRequest Model { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var result=await MWOService.GetMWOApprovedById(MWOId);
        if(result.Succeeded)
        {
            Model.MWOApproved = result.Data;
        }
        Model.Date = DateTime.UtcNow;
        Model.Validator += ValidateAsync;

    }
    async Task SaveAsync()
    {
        var result=await Service.CreateSapAdjust(Model);
        if(result.Succeeded)
        {
            MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

            CancelAsync();
        }
        else
        {
            MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);


        }
    }
    private void CancelAsync()
    {
        Navigation.NavigateBack();

    }
    FluentValidationValidator _fluentValidationValidator = null!;
    async Task<bool> ValidateAsync()
    {
        try
        {
            NotValidated = !(await _fluentValidationValidator.ValidateAsync());
            return NotValidated;
        }
        catch (Exception ex)
        {
            string exm = ex.Message;
        }
        return false;

    }
    bool NotValidated = true;

    public void Dispose()
    {
        Model.Validator -= ValidateAsync;
    }
}
