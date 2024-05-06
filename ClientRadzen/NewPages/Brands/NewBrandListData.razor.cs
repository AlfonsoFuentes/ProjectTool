#nullable disable
namespace ClientRadzen.NewPages.Brands
{
    public partial class NewBrandListData
    {
        [CascadingParameter]
        private App MainApp { get; set; }
        [CascadingParameter]
        public NewBrandMain BrandMain { get; set; }

    }
}
