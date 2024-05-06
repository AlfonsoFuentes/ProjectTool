using Microsoft.AspNetCore.Components;
#nullable disable
namespace ClientRadzen.Pages.MWOPages.Templates;
public partial class TemplateMWOApprovedDataList
{
    [Parameter]
    public MWOApprovedWithBudgetItemsResponse Response { get; set; } = new();
    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public MWOApproved MWOApproved {  get; set; }
    NewBudgetItemsWithPurchaseorders seletedRow = null;
    void ShowPurchaseOrders(NewBudgetItemsWithPurchaseorders approvedResponse)
    {
        seletedRow = approvedResponse;

    }
    void HidePurchaseOrders()
    {
        seletedRow = null!;

    }
}
