#nullable disable
namespace ClientRadzen.NewPages.Suppliers
{
    public partial class NewSuppliersDataList
    {
        [CascadingParameter]
        private App MainApp { get; set; }
     

        [CascadingParameter]
        public NewSupplierMain SupplierMain { get; set; }
        
    }
}
