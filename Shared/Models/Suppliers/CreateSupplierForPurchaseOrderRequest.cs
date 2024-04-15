using Shared.Models.Currencies;

namespace Shared.Models.Suppliers
{
    public class CreateSupplierForPurchaseOrderRequest
    {
    
        public string Name { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public string TaxCodeLD { get; set; } = "751245";
        public string NickName { get; set; } = string.Empty;
        public string TaxCodeLP { get; set; } = "721031";


       
        public CurrencyEnum SupplierCurrency { get; set; } = CurrencyEnum.None;

       
       
    }
}
