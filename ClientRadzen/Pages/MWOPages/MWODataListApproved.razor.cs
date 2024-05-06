#nullable disable
namespace ClientRadzen.Pages.MWOPages;
public partial class MWODataListApproved
{
    [CascadingParameter]
    public MWODataMain DataMain { get; set; }
    IEnumerable<MWOApprovedResponse> FilteredItems => DataMain.Response.MWOsApproved == null ? new List<MWOApprovedResponse>() : DataMain.Response.MWOsApproved;
}
