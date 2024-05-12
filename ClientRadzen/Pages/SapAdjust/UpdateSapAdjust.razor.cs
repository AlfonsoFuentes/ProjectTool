using Blazored.FluentValidation;
using Client.Infrastructure.Managers.Reports;
using Client.Infrastructure.Managers.SapAdjusts;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models.SapAdjust;
using Shared.NewModels.EBPReport;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace ClientRadzen.Pages.SapAdjust;
public partial class UpdateSapAdjust
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Inject]
    private ISapAdjustService Service { get; set; }
    [Inject]
    private INewMWOService MWOService { get; set; }
    [Parameter]
    public Guid SapAdjustId { get; set; }
    UpdateSapAdjustRequest Model { get; set; } = new();
    [Inject]
    private IReportManager ReportManager { get; set; }
    NewEBPReportResponse MWOEBPResponse { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        var resultAdjust = await Service.GetSapAdjustById(SapAdjustId);
        if (resultAdjust.Succeeded)
        {
            Model = resultAdjust.Data;

        }
        
    }
    async Task SaveAsync()
    {
        var result = await Service.UpdateSapAdjust(Model);
        if (result.Succeeded)
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


    public async Task ChangeActualSap(string actualsapstring)
    {
        double actualsap = 0;
        if (double.TryParse(actualsapstring, out actualsap))
        {
            Model.ActualSap = actualsap;
        }
        await ValidateAsync();
    }
    public async Task ChangeCommitmentSap(string commitmentsapstring)
    {
        double commitmsap = 0;
        if (double.TryParse(commitmentsapstring, out commitmsap))
        {
            Model.CommitmentSap = commitmsap;
        }
        await ValidateAsync();
    }
    public async Task ChangePotencialSap(string potencialsapstring)
    {
        double potencialsap = 0;
        if (double.TryParse(potencialsapstring, out potencialsap))
        {
            Model.PotencialSap = potencialsap;
        }
        await ValidateAsync();
    }
    public async Task ChangeImage(string image)
    {
        Model.ImageData = string.Empty;
        if (!string.IsNullOrWhiteSpace(image))
        {
            Model.ImageData = image;
        }

        await ValidateAsync();
    }
    async Task UpdateEBPRepor()
    {
        var resultEbp = await ReportManager.GetEBPReport(Model.MWOId);
        if (resultEbp.Succeeded)
        {
            MWOEBPResponse = resultEbp.Data;
        }
    }
    protected override async Task OnParametersSetAsync()
    {
        await UpdateEBPRepor();
    }
}
