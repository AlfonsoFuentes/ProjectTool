using Microsoft.AspNetCore.Components;
using Shared.Models.MWOStatus;

#nullable disable
namespace ClientRadzen.Pages.MWOPages;
public partial class MWODataListApproved
{
    [CascadingParameter]
    public MWODataMain DataMain { get; set; }
    IQueryable<MWOResponse> FilteredItems => DataMain.FilteredItems.Where(x => x.Status.Id == MWOStatusEnum.Approved.Id);
}
