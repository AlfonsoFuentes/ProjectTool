using Shared.ExtensionsMetods;

namespace ClientRadzen.NewPages.MWOS.MWOApprovedPages;
#nullable disable
public partial class NewMWOApprovedPage
{
    [CascadingParameter]
    private App MainApp { get; set; }
    [Parameter]
    public Guid MWOId { get; set; }
    [Inject]
    private INewMWOService Service { get; set; } = null!;
    public NewMWOApproveRequest Model { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        var result = await Service.GetMWOByIdToApprove(MWOId);
        if (result.Succeeded)
        {
            Model = result.Data;

        }

        if (string.IsNullOrEmpty(Model.MWONumber))
        {
            NotValidated = true;
        }
    }

    public async Task SaveAsync()
    {
        var result = await Service.ApproveMWO(Model);
        if (result.Succeeded)
        {
            MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

            _NavigationManager.NavigateTo($"/{PageName.MWO.Main}");
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
    bool NotValidated = false;


    public async Task ChangeMWONumber(string _mwonumber)
    {

        Model.MWONumber = _mwonumber;
        await ValidateAsync();
    }

    public async Task ChangePercentageTaxes(string stringpercentage)
    {

        double percentage = stringpercentage.ToDouble();
        
        Model.PercentageAssetNoProductive = percentage;
        await ValidateAsync();
    }
    public async Task ChangePercentageEngineering(string stringpercentage)
    {

        double percentage = stringpercentage.ToDouble();
        
        Model.PercentageEngineering = percentage;
        await ValidateAsync();
    }
    public async Task ChangeTaxForAlterations(string stringpercentage)
    {

        double percentage = stringpercentage.ToDouble();
        
        Model.PercentageTaxForAlterations = percentage;
        await ValidateAsync();

    }
    public async Task ChangePercentageContingency(string stringpercentage)
    {

        double percentage = stringpercentage.ToDouble();
        

        Model.PercentageContingency = percentage;
        await ValidateAsync();

    }
    public async Task ChangeName(string name)
    {

        Model.Name = name;
        await ValidateAsync();

    }
    public async Task ChangeCostCenter()
    {
        await ValidateAsync();
    }
    public async Task ChangeType()
    {

        await ValidateAsync();
    }
}
