using Microsoft.AspNetCore.Components;
#nullable disable
namespace ClientRadzen.Pages.MWOPages.Templates;
public partial class TemplateMWOApprovedDataGrid
{
    [Parameter]
    public MWOApprovedWithBudgetItemsResponse Response { get; set; } = new();
    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public MWOApproved MWOApproved { get; set; }
}
