﻿

namespace Shared.NewModels.Suppliers.Reponses
{
    public class NewSupplierResponse
    {
        public Guid SupplierId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public string TaxCodeLD { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
        public string TaxCodeLP { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? ContactName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? ContactEmail { get; set; } = string.Empty;
        public CurrencyEnum SupplierCurrency { get; set; } = CurrencyEnum.None;
    }
}
