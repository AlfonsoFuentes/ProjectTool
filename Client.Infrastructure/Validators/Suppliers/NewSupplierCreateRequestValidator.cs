using Shared.NewModels.Suppliers.Requests;

namespace Client.Infrastructure.Validators.Suppliers
{
    public class NewSupplierCreateRequestValidator : AbstractValidator<NewSupplierCreateRequest>
    {
        private INewSupplierValidatorService _Service;
        public NewSupplierCreateRequestValidator(INewSupplierValidatorService service)
        {
            _Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Supplier Name must be defined!");
            RuleFor(x => x.Name).NotNull().WithMessage("Supplier Name must be defined!");
            RuleFor(x => x.TaxCodeLD).NotEmpty().NotNull().WithMessage("Tax Code LD must be defined!");
            RuleFor(x => x.NickName).NotEmpty().NotNull().WithMessage("Nick Name must be defined!");
            RuleFor(x => x.TaxCodeLP).NotEmpty().NotNull().WithMessage("Tax Code LP must be defined!");
            RuleFor(x => x.SupplierCurrency.Id).NotEqual(-1).WithMessage("Supplier Currency must be defined!");

            RuleFor(x => x.VendorCode).NotEmpty().WithMessage("Supplier vendor Code must be defined!");
            RuleFor(x => x.VendorCode).NotNull().WithMessage("Supplier vendor Code must be defined!");
            RuleFor(customer => customer.VendorCode).Matches("^[0-9]*$").WithMessage("Supplier vendor Code must be number!");
            RuleFor(customer => customer.TaxCodeLP).Matches("^[0-9]*$").WithMessage("Supplier vendor Code must be number!");
            RuleFor(customer => customer.TaxCodeLD).Matches("^[0-9]*$").WithMessage("Supplier vendor Code must be number!");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist).When(x => !string.IsNullOrEmpty(x.Name)).WithMessage(x => $"Vendor Name:{x.Name} already exist");
            RuleFor(x => x.VendorCode).MustAsync(ReviewIfVendorCodeExist).When(x => !string.IsNullOrEmpty(x.VendorCode)).WithMessage(x => $"Vendor Code :{x.VendorCode} already exist");
            RuleFor(x => x.ContactEmail).MustAsync(ReviewIfEmailExist).When(x => !string.IsNullOrEmpty(x.ContactEmail)).WithMessage(x => $"Email :{x.ContactEmail} already exist");
            RuleFor(x => x.ContactEmail).EmailAddress().When(x => !string.IsNullOrEmpty(x.ContactEmail)).WithMessage(x => $"Must supply valid email");
            RuleFor(x => x.ContactEmail).NotEmpty().WithMessage("Supplier email must be defined!");
            RuleFor(x => x.ContactEmail).NotNull().WithMessage("Supplier email must be defined!");
        }
        async Task<bool> ReviewIfNameExist(string name, CancellationToken cancellationToken)
        {
            var result = await _Service.ReviewIfNameExist(name);
            return !result;
        }
        async Task<bool> ReviewIfVendorCodeExist(string vendorcode, CancellationToken cancellationToken)
        {
            var result = await _Service.ReviewIfVendorCodeExist(vendorcode);
            return !result;
        }
        async Task<bool> ReviewIfEmailExist(string? email, CancellationToken cancellationToken)
        {

            var result = await _Service.ReviewIfEmailExist(email);
            return !result;
        }
    }
}
