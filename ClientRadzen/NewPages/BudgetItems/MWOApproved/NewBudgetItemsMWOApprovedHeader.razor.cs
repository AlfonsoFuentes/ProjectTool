namespace ClientRadzen.NewPages.BudgetItems.MWOApproved;
#nullable disable
public partial class NewBudgetItemsMWOApprovedHeader
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public NewBudgetItemsMWOApprovedMain MainPage { get; set; }
}
