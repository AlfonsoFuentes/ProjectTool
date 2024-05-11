using Shared.Enums.BudgetItemTypes;
using Shared.Enums.Currencies;
using Shared.ExtensionsMetods;
using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.PurchaseOrders.Request;

namespace ClientRadzen.NewPages.PurchaseOrder.CreateSalarys;
#nullable disable
public partial class NewPurchaseOrderCreateSalaryPage
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [Parameter]
    public Guid BudgetItemId { get; set; }
    [Inject]
    private INewPurchaseOrderService Service { get; set; }

    [Inject]
    private INewBudgetItemService BudgetItemService { get; set; }


    public NewPurchaseOrderCreateSalaryRequest Model = null;



    FluentValidationValidator _fluentValidationValidator = null!;
    public List<NewBudgetItemToCreatePurchaseOrderResponse> OriginalBudgetItems { get; set; } = new();
    public List<NewBudgetItemToCreatePurchaseOrderResponse> BudgetItems { get; set; } = new();
    public List<NewPurchaseOrderReceiveItemRequest> PurchaseOrderItems { get; set; } = new();

    NewPurchaseOrderReceiveItemRequest PurchaseOrderItem => PurchaseOrderItems.Count == 0 ? new() : PurchaseOrderItems[0];


    protected override async Task OnInitializedAsync()
    {

        var USDCOP = MainApp.RateList == null ? 4000 : Math.Round(MainApp.RateList.COP, 2);
        var USDEUR = MainApp.RateList == null ? 1 : Math.Round(MainApp.RateList.EUR, 2);
        var result = await BudgetItemService.OldGetBudgetItemMWOApprovedById(BudgetItemId);
        if (result.Succeeded)
        {
            Model = new();
            Model.SetMainBudgetItem(result.Data, USDCOP, USDEUR);
            
            Model.SetQuoteCurrency(CurrencyEnum.USD);
            Model.SetPurchaseOrderCurrency(CurrencyEnum.USD);
            
            PurchaseOrderItem.Quantity = 1;
        }
        var resultBudgetItems = await BudgetItemService.GetAllMWOApprovedForCreatePurchaseOrder(Model.MWOId);
        if (resultBudgetItems.Succeeded)
        {

            OriginalBudgetItems = resultBudgetItems.Data.BudgetItems;
        }
        InitializeBudgetItems();
        InitializePurchaseOrder();
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
        var result = await Service.CreatePurchaseOrderSalaryAsync(Model);
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



    public async Task ChangeTRMUSDEUR(string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double usdeur = arg.ToDouble();
        Model.SetTRM(Model.USDEUR, usdeur, DateTime.UtcNow);
        await ValidateAsync();
    }
    public async Task ChangeTRMUSDCOP(string arg)
    {

        if (string.IsNullOrEmpty(arg))
        {
            return;
        }
        double usdcop = arg.ToDouble();
        Model.SetTRM(usdcop, Model.USDEUR, DateTime.UtcNow);

        await ValidateAsync();
    }

    public async Task ChangeName(string name)
    {
        Model.SetPurchaseOrderName(name);

        await ValidateAsync();
        StateHasChanged();
    }
    public async Task ChangeName(NewPurchaseOrderReceiveItemRequest model, string name)
    {

        Model.SetPurchaseOrderItemName(model, name);

        await ValidateAsync();
        StateHasChanged();
    }





    public async Task ChangeQuantity(NewPurchaseOrderReceiveItemRequest item, string arg)
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
    public async Task ChangePONumber(string ponumber)
    {

        Model.PurchaseOrderNumber = ponumber.Trim();
        await ValidateAsync();

    }
    public async Task ChangeCurrencyValue(NewPurchaseOrderReceiveItemRequest item, string arg)
    {


        double currencyvalue = arg.ToDouble();

        item.UnitaryValueCurrency = currencyvalue;
    
        await ValidateAsync();
        StateHasChanged();
    }



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
    public void RemovedBudgetItemToPurchaseOrder(NewPurchaseOrderReceiveItemRequest Item)
    {
        PurchaseOrderItems = new();
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
}
