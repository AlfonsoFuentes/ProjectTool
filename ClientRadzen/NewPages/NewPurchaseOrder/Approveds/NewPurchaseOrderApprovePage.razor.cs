using Shared.Commons.Results;
using Shared.Enums.BudgetItemTypes;
using Shared.Enums.Currencies;
using Shared.ExtensionsMetods;
using Shared.NewModels.BudgetItems.Responses;
using Shared.NewModels.PurchaseOrders.Base;
using Shared.NewModels.PurchaseOrders.Request;

namespace ClientRadzen.NewPages.NewPurchaseOrder.Approveds;
#nullable disable
public partial class NewPurchaseOrderApprovePage
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
    public NewPurchaseOrderApproveRequest Model = null;
    public List<NewBudgetItemMWOApprovedResponse> OriginalBudgetItems { get; set; } = new();
    List<NewSupplierResponse> Suppliers { get; set; } = new();

    FluentValidationValidator _fluentValidationValidator = null!;

    protected override async Task OnInitializedAsync()
    {
        var resultSupplier = await SupplierService.GetAllSupplier();
        if (resultSupplier.Succeeded)
        {
            Suppliers = resultSupplier.Data.Suppliers;

        }
        var USDCOP = MainApp.RateList == null ? 4000 : Math.Round(MainApp.RateList.COP, 2);
        var USDEUR = MainApp.RateList == null ? 1 : Math.Round(MainApp.RateList.EUR, 2);
        var resultPurchaseOrder = await Service.GetPurchaseOrderToApproved(PurchaseOrderId);
        if (resultPurchaseOrder.Succeeded)
        {
            var resultBudgetItems = await BudgetItemService.GetAllMWOApprovedWithItems(resultPurchaseOrder.Data.PurchaseOrder.MWOId);
            if (resultBudgetItems.Succeeded)
            {

                OriginalBudgetItems = resultBudgetItems.Data.BudgetItems;
            }
            Model = resultPurchaseOrder.Data;
            Model.PurchaseOrder.SetTRM(USDCOP, USDEUR);
        }


        StateHasChanged();
    }


    public async Task<bool> ValidateAsync()
    {
        Validated = await _fluentValidationValidator.ValidateAsync();
        return Validated;
    }
    bool Validated = false;
    async Task<IResult> SaveAsync()
    {

        return await Service.ApprovePurchaseOrderAsync(Model);
    }

    bool debug = true;




}
