namespace ClientRadzen.NewPages.BudgetItems.MWOCreated;
#nullable disable
public partial class NewBudgetItemMWOCreatedTableList
{
    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public NewBudgetItemsMWOCreatedMain MainPage { get; set; }
}
