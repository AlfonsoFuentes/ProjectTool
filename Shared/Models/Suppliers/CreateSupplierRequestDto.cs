namespace Shared.Models.Suppliers
{
    public class CreateSupplierRequestDto
    {

        public string Name { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public string TaxCodeLD { get; set; } = "751245";
        public string NickName { get; set; } = string.Empty;
        public string TaxCodeLP { get; set; } = "721031";


        public string? PhoneNumber { get; set; } = string.Empty;
        public string? ContactName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? ContactEmail { get; set; } = string.Empty;

        public int SupplierCurrency { get; set; }
        public void ConvertToDto(CreateSupplierRequest request)
        {
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
        public void ConvertToDto(CreateSupplierForPurchaseOrderRequest request)
        {
          
            Name = request.Name;
            NickName = request.NickName;
            TaxCodeLP = request.TaxCodeLP;
          
            SupplierCurrency = request.SupplierCurrency.Id;
            TaxCodeLD = request.TaxCodeLD;
            VendorCode = request.VendorCode;
        }
    }
}
