using Shared.Enums.Currencies;

namespace Shared.NewModels.Suppliers.Requests
{
    public class NewSupplierUpdateRequest
    {
        public Guid SupplierId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public string TaxCodeLP { get; set; } = "721031";
        public string TaxCodeLD { get; set; } = "721545";
        public string NickName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; } = string.Empty;
        public string? ContactName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? ContactEmail { get; set; } = string.Empty;
        public CurrencyEnum SupplierCurrency { get; set; } = CurrencyEnum.None;
    }
}
