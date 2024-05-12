using Shared.NewModels.PurchaseOrders.Responses;

namespace ClientRadzen.NewPages.BudgetItems.MWOApproved;
#nullable disable
public partial class NewBudgetItemsMWOApprovedPurchaseOrderIten
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public NewBudgetItemsMWOApprovedMain MainPage { get; set; }
    [Parameter]
    [EditorRequired]
    public List<NewPriorPurchaseOrderItemResponse> PurchaseOrderItems { get; set; } = new();
    [Parameter]
    [EditorRequired]
    public bool IsMainItemTaxesNoProductive { get; set; } = false;

}
