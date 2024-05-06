using Azure;
using ClientRadzen.NewPages.Brands;
using Shared.ExtensionsMetods;
using Shared.NewModels.BudgetItems.Request;

namespace ClientRadzen.NewPages.BudgetItems.MWOCreated;
#nullable disable
public partial class NewBudgetItemCreatePage
{
    [CascadingParameter]
    private App MainApp { get; set; }
    [Inject]
    private INewBudgetItemService Service { get; set; }
    [Parameter]
    public Guid MWOId { get; set; }
    [Inject]
    private INewBrandService BrandService { get; set; }
    FluentValidationValidator _fluentValidationValidator = null!;
    public List<NewBrandResponse> Brands { get; set; } = new();
    public NewBudgetItemMWOCreatedRequest Model { get; set; } = new();

    public NewMWOCreatedWithItemsResponse MWOResponse { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        var result = await Service.GetAllMWOCreatedWithItems(MWOId);
        if (result.Succeeded)
        {
            MWOResponse = result.Data;
        }
        Model.MWOId = MWOId;
        Model.MWOName = MWOResponse.Name;
        Model.IsAssetProductive = MWOResponse.IsAssetProductive;
        await GetAllBrands();

    }
    async Task<List<NewBrandResponse>> GetAllBrands()
    {
        var resultbrands = await BrandService.GetAllBrand();
        if (resultbrands.Succeeded)
        {
            Brands = resultbrands.Data.Brands;
        }
        return Brands;
    }

    public async Task SaveAsync()
    {


        var result = await Service.CreateBudgetItem(Model);
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
    public async Task CreateBrand()
    {
        var result = await DialogService.OpenAsync<NewCreateBrandDialog>($"Create New Brand",
            new Dictionary<string, object>() { },
            new DialogOptions() { Width = "500px", Height = "312px", Resizable = true, Draggable = true });
        if (result != null && result is NewBrandResponse)
        {
            Model.Brand = result as NewBrandResponse;
            var resultData = await BrandService.GetAllBrand();
            if (resultData.Succeeded)
            {
                Brands = resultData.Data.Brands;

            }


        }
    }
    public void CancelAsync()
    {
        Navigation.NavigateBack();

    }

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
    public bool NotValidated = true;
    public async Task ChangeName(string name)
    {
        Model.Name = name;
        await ValidateAsync();

    }
    public async Task ChangeQuantityAlterationEquipment(string stringquantity)
    {

        double quantity = stringquantity.ToDouble();
        Model.Quantity = quantity;


        NotValidated = await ValidateAsync();
    }
    public async Task ChangeUnitaryCostAlterationEquipment(string stringunitaryCost)
    {

        double unitarycost = stringunitaryCost.ToDouble();

        Model.UnitaryCost = unitarycost;


        NotValidated = await ValidateAsync();
    }
    public async Task ChangeUnitaryCostEngineering(string unitarycoststring)
    {

        double unitarycost = unitarycoststring.ToDouble();

        Model.UnitaryCost = unitarycost;
        Model.Quantity = 1;

        NotValidated = await ValidateAsync();


    }
    public async Task ChangePercentageAbleToEdit(string stringpercentage)
    {

        double percentage = stringpercentage.ToDouble();

        Model.Percentage = percentage;

        Model.UnitaryCost = Math.Round(Model.TotalTaxesApplied * Model.Percentage / 100, 2);
        Model.Quantity = 1;

        NotValidated = await ValidateAsync();
    }
    public async Task ChangeTaxesItemList(object objeto)
    {


        Model.UnitaryCost = Math.Round(Model.TotalTaxesApplied * Model.Percentage / 100.0, 2);
        Model.Quantity = 1;


        NotValidated = await ValidateAsync();
    }

}
