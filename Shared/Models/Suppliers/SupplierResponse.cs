﻿using Shared.Models.Currencies;

namespace Shared.Models.Suppliers
{
    public class SupplierResponse
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public string TaxCodeLD { get; set; } = "751545";
        public string NickName { get; set; } = string.Empty;
        public string TaxCodeLP { get; set; } = "721031";


        public string PhoneNumber { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;

        public CurrencyEnum SupplierCurrency { get; set; } = CurrencyEnum.None;
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
    }
}
