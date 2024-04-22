using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Shared.Models.MWOStatus;
using Radzen;
using Client.Infrastructure.Managers.BudgetItems;

namespace ClientRadzen.Pages.MWOPages;

#nullable disable
public partial class MWODataApproved
{

    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public MWODataMain DataMain { get; set; }
    [Parameter]
    public MWOApprovedResponse Data { get; set; }
    [CascadingParameter]
    private MWODataMain MainPage { get; set; }
   
   
}
