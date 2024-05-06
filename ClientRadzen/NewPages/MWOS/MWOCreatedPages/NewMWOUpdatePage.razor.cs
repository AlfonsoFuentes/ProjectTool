
namespace ClientRadzen.NewPages.MWOS.MWOCreatedPages;
#nullable disable
public partial class NewMWOUpdatePage
{
    [CascadingParameter]
    private App MainApp { get; set; }
    NewMWOUpdateRequest Model { get; set; } = new();

    [Parameter]
    public Guid MWOId { get; set; }

    [Inject]
    private INewMWOService Service { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var result=await Service.GetMWOToUpdateById(MWOId);
        if(result.Succeeded)
        {
            Model = result.Data;
        }
    }
    private async Task SaveAsync()
    {

        var result = await Service.UpdateMWOCreated(Model);
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


    public async Task ChangeName(string name)
    {

        Model.Name = name;
        await ValidateAsync();

    }
    async Task ChangeAssetProductive(bool isproductive)
    {
        Model.IsAssetProductive = isproductive;
        await ValidateAsync();
    }
    public async Task ChangePercentageTaxes(string stringpercentage)
    {

        double percentage = 0;
        if (!double.TryParse(stringpercentage, out percentage))
        {

        }

        Model.PercentageAssetNoProductive = percentage;
        await ValidateAsync();
    }
    public async Task ChangePercentageEngineering(string stringpercentage)
    {

        double percentage = 0;
        if (!double.TryParse(stringpercentage, out percentage))
        {

        }

        Model.PercentageEngineering = percentage;
        await ValidateAsync();

    }
    public async Task ChangePercentageContingency(string stringpercentage)
    {

        double percentage = 0;
        if (!double.TryParse(stringpercentage, out percentage))
        {

        }


        Model.PercentageContingency = percentage;
        await ValidateAsync();

    }
    public async Task ChangeTaxForAlterations(string stringpercentage)
    {

        double percentage = 0;
        if (!double.TryParse(stringpercentage, out percentage))
        {

        }

        Model.PercentageTaxForAlterations = percentage;
        await ValidateAsync();

    }
    public async Task ChangeType()
    {

        await ValidateAsync();
    }
}
