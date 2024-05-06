namespace ClientRadzen.NewPages.Brands;
#nullable disable
public partial class NewBrandTableList
{
    [CascadingParameter]
    private App MainApp { get; set; }
    [CascadingParameter]
    public NewBrandMain BrandMain { get; set; }
}
