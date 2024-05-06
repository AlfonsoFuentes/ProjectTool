

namespace Application.Mappers.Suppliers
{
    public static class SupplierMapper
    {
        public static void FromSupplierCreate(this NewSupplierCreateRequest request, Supplier supplier)
        {
            supplier.Name = request.Name;
            supplier.SupplierCurrency = request.SupplierCurrency.Id;
            supplier.NickName = request.NickName;
            supplier.TaxCodeLD = request.TaxCodeLD;
            supplier.TaxCodeLP = request.TaxCodeLP;
            supplier.VendorCode = request.VendorCode;
            supplier.PhoneNumber = request.PhoneNumber;
            supplier.Address = request.Address;
            supplier.ContactEmail = request.ContactEmail;
            supplier.ContactName = request.ContactName;

        }
        public static void FromSupplierUpdate(this NewSupplierUpdateRequest request, Supplier supplier)
        {
            supplier.Name = request.Name;
            supplier.SupplierCurrency = request.SupplierCurrency.Id;
            supplier.NickName = request.NickName;
            supplier.TaxCodeLD = request.TaxCodeLD;
            supplier.TaxCodeLP = request.TaxCodeLP;
            supplier.VendorCode = request.VendorCode;
            supplier.PhoneNumber = request.PhoneNumber;
            supplier.Address = request.Address;
            supplier.ContactEmail = request.ContactEmail;
            supplier.ContactName = request.ContactName;

        }
        public static void FromSupplierCreateBasic(this NewSupplierCreateBasicRequest request, Supplier supplier)
        {
            supplier.Name = request.Name;
            supplier.SupplierCurrency = request.SupplierCurrency.Id;
            supplier.NickName = request.NickName;
            supplier.TaxCodeLD = request.TaxCodeLD;
            supplier.TaxCodeLP = request.TaxCodeLP;
            supplier.VendorCode = request.VendorCode;

        }
        public static NewSupplierResponse ToResponse(this Supplier supplier)
        {
            return new()
            {
                SupplierId = supplier.Id,
                Address = supplier.Address,
                ContactEmail = supplier.ContactEmail,
                ContactName = supplier.ContactName,
                Name = supplier.Name,
                NickName = supplier.NickName,
                PhoneNumber = supplier.PhoneNumber,
                SupplierCurrency = CurrencyEnum.GetType(supplier.SupplierCurrency),
                TaxCodeLD = supplier.TaxCodeLD,
                TaxCodeLP = supplier.TaxCodeLP,
                VendorCode = supplier.VendorCode,
                CreatedBy=supplier.CreatedByUserName,

            };
        }
        public static NewSupplierUpdateRequest ToUpdateRequest(this Supplier supplier)
        {
            return new()
            {
                SupplierId = supplier.Id,
                Address = supplier.Address,
                ContactEmail = supplier.ContactEmail,
                ContactName = supplier.ContactName,
                Name = supplier.Name,
                NickName = supplier.NickName,
                PhoneNumber = supplier.PhoneNumber,
                SupplierCurrency = CurrencyEnum.GetType(supplier.SupplierCurrency),
                TaxCodeLD = supplier.TaxCodeLD,
                TaxCodeLP = supplier.TaxCodeLP,
                VendorCode = supplier.VendorCode,

            };
        }
        public static NewSupplierExportFileResponse ToFileExportResponse(this Supplier supplier)
        {
            return new()
            {
               
                Address = supplier.Address,
                ContactEmail = supplier.ContactEmail,
                ContactName = supplier.ContactName,
                Name = supplier.Name,
                NickName = supplier.NickName,
                PhoneNumber = supplier.PhoneNumber,
                SupplierCurrency = CurrencyEnum.GetName(supplier.SupplierCurrency),
                TaxCodeLD = supplier.TaxCodeLD,
                TaxCodeLP = supplier.TaxCodeLP,
                VendorCode = supplier.VendorCode,

            };
        }
    }
}
