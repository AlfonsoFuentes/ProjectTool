using Shared.Enums.BudgetItemTypes;
using Shared.Enums.Currencies;
using Shared.ExtensionsMetods;
using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.PurchaseOrders.Request;

namespace ClientRadzen.NewPages.PurchaseOrder.Approveds;
#nullable disable
public partial class NewPurchaseOrderEditApprovedPage
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public Guid PurchaseOrderId { get; set; }
    [Inject]
    private INewPurchaseOrderService Service { get; set; }
    [Inject]
    private INewSupplierService SupplierService { get; set; }
    [Inject]
    private INewBudgetItemService BudgetItemService { get; set; }


    public NewPurchaseOrderEditApprovedRequest Model = null;


    List<NewSupplierResponse> Suppliers { get; set; } = new();

    FluentValidationValidator _fluentValidationValidator = null!;


    NewBudgetItemToCreatePurchaseOrderResponse ItemToAdd;


    protected override async Task OnInitializedAsync()
    {
        var resultSupplier = await SupplierService.GetAllSupplier();
        if (resultSupplier.Succeeded)
        {
            Suppliers = resultSupplier.Data.Suppliers;

        }
        var USDCOP = MainApp.RateList == null ? 4000 : Math.Round(MainApp.RateList.COP, 2);
        var USDEUR = MainApp.RateList == null ? 1 : Math.Round(MainApp.RateList.EUR, 2);
        var result = await Service.GetPurchaseOrderToEditApproved(PurchaseOrderId);
        if (result.Succeeded)
        {
            Model = result.Data;
          
            



        }
        var resultBudgetItems = await BudgetItemService.GetAllMWOApprovedForCreatePurchaseOrder(Model.MWOId);
        if (resultBudgetItems.Succeeded)
        {

            OriginalBudgetItems = resultBudgetItems.Data.BudgetItems; 
            InitializeBudgetItems();
            InitializePurchaseOrder();
        }
        StateHasChanged();
    }


    async Task<bool> ValidateAsync()
    {
        Validated = await _fluentValidationValidator.ValidateAsync();
        return Validated;
    }
    bool Validated = false;
    public async Task SaveAsync()
    {
        var result = await Service.EditApprovedPurchaseOrderAsync(Model);
        if (result.Succeeded)
        {
            MainApp.NotifyMessage(NotificationSeverity.Success, "Success", result.Messages);

            Cancel();
        }
        else
        {
            MainApp.NotifyMessage(NotificationSeverity.Error, "Error", result.Messages);


        }

    }

    void Cancel()
    {
        Navigation.NavigateBack();
    }
    
    bool debug = true;

    public void AddBudgetItemToPurchaseOrder(NewBudgetItemToCreatePurchaseOrderResponse budgetitem)
    {
        PurchaseOrderItems = new();
        Model.AddBudgetItem(budgetitem);
        foreach (var item in Model.PurchaseOrderItems)
        {
            PurchaseOrderItems.Add(item);
        }
        if (BudgetItems.Any(x => x.BudgetItemId == budgetitem.BudgetItemId))
        {
            BudgetItems.Remove(budgetitem);
        }
        if (BudgetItems.Count > 0)
        {
            PurchaseOrderItems.Add(new());
        }
    }
    public async Task ChangePONumber(string ponumber)
    {

        Model.PurchaseOrderNumber = ponumber.Trim();
        await ValidateAsync();

    }
    public async Task ChangedExpectedDate(DateTime? expected)
    {
        Model.ExpectedDate = expected;
        await ValidateAsync();
    }
    public List<NewBudgetItemToCreatePurchaseOrderResponse> OriginalBudgetItems { get; set; } = new();
    public List<NewBudgetItemToCreatePurchaseOrderResponse> BudgetItems { get; set; } = new();
    public List<NewPurchaseOrderCreateItemRequest> PurchaseOrderItems { get; set; } = new();

    public async Task ChangeName(NewPurchaseOrderCreateItemRequest model, string name)
    {

        Model.SetPurchaseOrderItemName(model, name);

        await ValidateAsync();
        StateHasChanged();
    }
    public async Task ChangeQuantity(NewPurchaseOrderCreateItemRequest item, string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double quantity = arg.ToDouble();
        item.Quantity = quantity;
        await ValidateAsync();
        StateHasChanged();
    }
    public async Task ChangeCurrencyValue(NewPurchaseOrderCreateItemRequest item, double arg)
    {


        double currencyvalue = arg;

        item.UnitaryValueCurrency = currencyvalue;
        StateHasChanged();
        await ValidateAsync();
        StateHasChanged();
    }
    void InitializeBudgetItems()
    {
        BudgetItems = new();

        if (Model.IsAlteration)
        {
            BudgetItems = OriginalBudgetItems.Where(x => x.IsAlteration).ToList();
        }
        else if (!Model.IsCapitalizedSalary)
        {
            BudgetItems = OriginalBudgetItems.Where(x => x.IsAlteration == false).ToList();

            if (BudgetItems.Any(x => x.IsMainItemTaxesNoProductive))
            {
                var budget = BudgetItems.Single(x => x.IsMainItemTaxesNoProductive);
                BudgetItems.Remove(budget);
            }
            if (BudgetItems.Any(x => x.Type.Id == BudgetItemTypeEnum.Engineering.Id && x.IsEngineeringItem))
            {
                var budget = BudgetItems.Single(x => x.Type.Id == BudgetItemTypeEnum.Engineering.Id && x.IsEngineeringItem);
                BudgetItems.Remove(budget);
            }
        }
    }
    void InitializePurchaseOrder()
    {
        PurchaseOrderItems = new();
        foreach (var item in Model.PurchaseOrderItems)
        {
            PurchaseOrderItems.Add(item);
            if (BudgetItems.Any(x => x.BudgetItemId == item.BudgetItemId))
            {
                var budgetitem = BudgetItems.Single(x => x.BudgetItemId == item.BudgetItemId);
                BudgetItems.Remove(budgetitem);

            }
        }

        if (BudgetItems.Count > 0)
        {
            PurchaseOrderItems.Add(new());
        }
    }
    public void RemovedBudgetItemToPurchaseOrder(NewPurchaseOrderCreateItemRequest Item)
    {
        PurchaseOrderItems.Clear();
        if (Model.PurchaseOrderItems.Any(x => x.BudgetItemId == Item.BudgetItemId))
        {
            var item = Model.PurchaseOrderItems.Single(x => x.BudgetItemId == Item.BudgetItemId);
            Model.PurchaseOrderItems.Remove(item);
            InitializeBudgetItems();
        }

        foreach (var item in Model.PurchaseOrderItems)
        {
            PurchaseOrderItems.Add(item);
        }
        if (BudgetItems.Any(x => x.BudgetItemId == Item.BudgetItemId))
        {
            var budgetitem = BudgetItems.Single(x => x.BudgetItemId == Item.BudgetItemId);
            BudgetItems.Remove(budgetitem);
        }
        if (BudgetItems.Count > 0)
        {
            PurchaseOrderItems.Add(new());
        }
    }
}
