namespace Shared.Models.Suppliers
{
    public class UpdateSupplierRequestDto
    {
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

        public int SupplierCurrency { get; set; }
        public void ConvertToDto(UpdateSupplierRequest request)
        {
            Id = request.Id;
            Address = request.Address;
            ContactEmail = request.ContactEmail;
            ContactName = request.ContactName;
            Name = request.Name;
            NickName = request.NickName;
            TaxCodeLP = request.TaxCodeLP;
            PhoneNumber = request.PhoneNumber;
            SupplierCurrency = request.SupplierCurrency.Id;
            TaxCodeLD = request.TaxCodeLD;
            VendorCode = request.VendorCode;
        }
    }
}
