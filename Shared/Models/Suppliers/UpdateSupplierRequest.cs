﻿using Shared.Models.Currencies;

namespace Shared.Models.Suppliers
{
    public class UpdateSupplierRequest
    {
        public Func<Task<bool>> Validator { get; set; } = null!;
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public string TaxCodeLD { get; set; } = string.Empty;

        public string TaxCodeLP { get; set; } = string.Empty;

        public string NickName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? ContactName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? ContactEmail { get; set; } = string.Empty;

        public CurrencyEnum SupplierCurrency { get; set; } = CurrencyEnum.None;
       
        public async Task ChangeName(string name)
        {
            Name = name;
            if (Validator != null) await Validator();
        }
        public async Task ChangeNickName(string name)
        {
            NickName = name;
            if (Validator != null) await Validator();
        }
        public async Task ChangeVendorCode(string name)
        {
            VendorCode = name;
            if (Validator != null) await Validator();
        }
        public async Task ChangeEmail(string name)
        {
            ContactEmail = name;
            if (Validator != null) await Validator();
        }
    }
}
