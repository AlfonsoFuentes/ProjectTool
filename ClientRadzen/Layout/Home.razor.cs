#nullable disable
namespace ClientRadzen.Layout;
public partial class Home
{
    [CascadingParameter]
    public App MainApp { get; set; }
    //[Inject]
    //private IChangeUserManager changeUserManager { get; set; }
    //[Inject]
    //private IPurchaseOrderService purchaseOrderService { get; set; }
    protected override async Task OnInitializedAsync()
    {
        //if (CurrentUser.UserName == "alfonso_fuentes@colpal.com")
        //{
        //    //Este codigo se creo para cambiar de usuario de alfonsofuen@gmail.com(Superadmin) a regular user(alfonso_fuentes)
        //    //await changeUserManager.ChangeUser();
        //}
        //await changeUserManager.UpdateTenant();
        //var result = await purchaseOrderService.RecalculatePurchaseOrder();
        //if (result.Succeeded)
        //{

        //}
    }

    }
