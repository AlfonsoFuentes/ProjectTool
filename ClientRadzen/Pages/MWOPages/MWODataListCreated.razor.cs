using Microsoft.AspNetCore.Components;
using Shared.Models.MWOStatus;

#nullable disable
namespace ClientRadzen.Pages.MWOPages;
public partial class MWODataListCreated
{
    [CascadingParameter]
    public MWODataMain DataMain { get; set; }
    IEnumerable<MWOCreatedResponse> FilteredItems => DataMain.Response.MWOsCreated==null?new List<MWOCreatedResponse>(): DataMain.Response.MWOsCreated;
}
