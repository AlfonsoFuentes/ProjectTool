using Shared.Models.Currencies;

namespace Shared.Models.Suppliers
{
    public class CreateSupplierRequest
    {
        public string Name { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public string TaxCodeLD { get; set; } = "751245";

        public string TaxCodeLP { get; set; } = "721031";


        public string? PhoneNumber { get; set; } = string.Empty;
        public string? ContactName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? ContactEmail { get; set; } = string.Empty;

        public CurrencyEnum SupplierCurrency { get; set; } = CurrencyEnum.None;
    }
}
