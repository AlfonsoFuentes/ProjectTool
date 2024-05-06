#nullable disable
using ClientRadzen.NewPages.MWOS;

namespace ClientRadzen.NewPages.MWOS.Tables;
public partial class NewMWODataListApproved
{
    [CascadingParameter]
    public NewMWOMain DataMain { get; set; }
    IEnumerable<NewMWOApprovedReponse> FilteredItems => DataMain.MWOsApproved == null ? new List<NewMWOApprovedReponse>() : DataMain.MWOsApproved;
}
