using Microsoft.AspNetCore.Components;
using Shared.Models.MWOStatus;

#nullable disable
namespace ClientRadzen.Pages.MWOPages;
public partial class MWODataListCreated
{
    [CascadingParameter]
    public MWODataMain DataMain { get; set; }
    IEnumerable<MWOResponse> FilteredItems => DataMain.Response.MWOsCreated==null?new List<MWOResponse>(): DataMain.Response.MWOsCreated;
}
