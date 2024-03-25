using Microsoft.AspNetCore.Components;
using Shared.Models.MWOStatus;

#nullable disable
namespace ClientRadzen.Pages.MWOPages;
public partial class MWODataListCreated
{
    [CascadingParameter]
    public MWODataMain DataMain { get; set; }
    IQueryable<MWOResponse> FilteredItems=>DataMain.FilteredItems.Where(x=>x.Status.Id==MWOStatusEnum.Created.Id);
}
