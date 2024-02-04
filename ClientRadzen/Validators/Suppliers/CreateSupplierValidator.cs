

using ClientRadzen.Managers.Suppliers;
using Shared.Models.Suppliers;
using Shared.Models.MWOTypes;

namespace ClientRadzen.Validators.Suppliers
{
    public class CreateSupplierValidator : AbstractValidator<CreateSupplierRequest>
    {
        private readonly ISupplierService Service;

        public CreateSupplierValidator(ISupplierService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Supplier Name must be defined!");
            RuleFor(x => x.TaxCodeLD).NotEmpty().NotNull().WithMessage("Tax Code LD must be defined!");
            RuleFor(x => x.TaxCodeLP).NotEmpty().NotNull().WithMessage("Tax Code LP must be defined!");
            RuleFor(x => x.SupplierCurrency).NotEqual(Shared.Models.Currencies.CurrencyEnum.None).WithMessage("Supplier Currency must be defined!");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist).WithMessage($"Supplier Name Already exist");
            RuleFor(x => x.VendorCode).MustAsync(ReviewIfVendorCodeExist).WithMessage($"Vendor Code Already exist");
          

        }

        async Task<bool> ReviewIfNameExist(string name, CancellationToken cancellationToken)
        {
            var result = await Service.ReviewIfNameExist(name);
            return !result;
        }
        async Task<bool> ReviewIfVendorCodeExist(string vendorcode, CancellationToken cancellationToken)
        {
            var result = await Service.ReviewIfVendorCodeExist(vendorcode);
            return !result;
        }
        async Task<bool> ReviewIfEmailExist(string? email, CancellationToken cancellationToken)
        {
            var result = await Service.ReviewIfEmailExist(email);
            return !result;
        }
    }
}
