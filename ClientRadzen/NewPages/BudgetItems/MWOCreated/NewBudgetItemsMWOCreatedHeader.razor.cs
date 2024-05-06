namespace ClientRadzen.NewPages.BudgetItems.MWOCreated;
public partial class NewBudgetItemsMWOCreatedHeader
{
    [CascadingParameter]
    public NewMWOCreatedWithItemsResponse Response { get; set; } = new();

}
