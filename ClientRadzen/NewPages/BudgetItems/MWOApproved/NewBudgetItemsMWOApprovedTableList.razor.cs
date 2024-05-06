namespace ClientRadzen.NewPages.BudgetItems.MWOApproved;
#nullable disable
public partial class NewBudgetItemsMWOApprovedTableList
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public NewBudgetItemsMWOApprovedMain MainPage { get; set; }
}
