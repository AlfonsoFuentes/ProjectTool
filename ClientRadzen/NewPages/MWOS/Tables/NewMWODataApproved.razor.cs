using ClientRadzen.NewPages.MWOS;

namespace ClientRadzen.NewPages.MWOS.Tables;

#nullable disable
public partial class NewMWODataApproved
{
    [CascadingParameter]
    public NewMWOMain DataMain { get; set; }

    [CascadingParameter]
    public App MainApp { get; set; }
    
    [Parameter]
    public NewMWOApprovedReponse Data { get; set; }



}
