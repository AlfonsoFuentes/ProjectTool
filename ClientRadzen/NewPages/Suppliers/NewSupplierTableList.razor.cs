#nullable disable
namespace ClientRadzen.NewPages.Suppliers;
public partial class NewSupplierTableList
{
    [CascadingParameter]
    private App MainApp { get; set; }


    [CascadingParameter]
    public NewSupplierMain SupplierMain { get; set; }
}
