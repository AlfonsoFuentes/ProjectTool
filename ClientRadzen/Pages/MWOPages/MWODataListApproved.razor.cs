using Microsoft.AspNetCore.Components;
using Shared.Models.MWOStatus;

#nullable disable
namespace ClientRadzen.Pages.MWOPages;
public partial class MWODataListApproved
{
    [CascadingParameter]
    public MWODataMain DataMain { get; set; }
    IEnumerable<MWOResponse> FilteredItems => DataMain.Response.MWOsApproved == null ? new List<MWOResponse>() : DataMain.Response.MWOsApproved;
}
