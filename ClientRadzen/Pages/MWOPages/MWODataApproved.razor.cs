using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Shared.Models.MWOStatus;

namespace ClientRadzen.Pages.MWOPages;

#nullable disable
public partial class MWODataApproved
{

    [CascadingParameter]
    public App MainApp { get; set; }
    [CascadingParameter]
    public MWODataMain DataMain { get; set; }
    [Parameter]
    public MWOResponse Data { get; set; }
    [CascadingParameter]
    private MWODataMain MainPage { get; set; }

  
}
