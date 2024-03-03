using Application.Interfaces;
using FluentValidation;
using Shared.Models.Suppliers;

namespace Application.Features.Suppliers.Validators
{
    internal class CreateSupplierValidator : AbstractValidator<CreateSupplierRequest>
    {
        private ISupplierRepository _supplierRepository;
        public CreateSupplierValidator(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Supplier Name must be defined!");
            RuleFor(x => x.Name).NotNull().WithMessage("Supplier Name must be defined!");
            RuleFor(x => x.TaxCodeLD).NotEmpty().NotNull().WithMessage("Tax Code LD must be defined!");
            RuleFor(x => x.TaxCodeLP).NotEmpty().NotNull().WithMessage("Tax Code LP must be defined!");
            RuleFor(x => x.SupplierCurrency.Id).NotEqual(-1).WithMessage("Supplier Currency must be defined!");

            RuleFor(x => x.VendorCode).NotEmpty().WithMessage("Supplier vendor Code must be defined!");
            RuleFor(x => x.VendorCode).NotNull().WithMessage("Supplier vendor Code must be defined!");
            RuleFor(customer => customer.VendorCode).Matches("^[0-9]*$").WithMessage("Supplier vendor Code must be number!");
            RuleFor(customer => customer.TaxCodeLP).Matches("^[0-9]*$").WithMessage("Supplier vendor Code must be number!");
            RuleFor(customer => customer.TaxCodeLD).Matches("^[0-9]*$").WithMessage("Supplier vendor Code must be number!");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist).WithMessage(x => $"Vendor Name:{x.Name} already exist");
            RuleFor(x => x.VendorCode).MustAsync(ReviewIfVendorCodeExist).WithMessage(x => $"Vendor Code:{x.VendorCode} already exist");
           
        }
        async Task<bool> ReviewIfNameExist(string name, CancellationToken cancellationToken)
        {
            var result = await _supplierRepository.ReviewNameExist(name);
            return !result;
        }
        async Task<bool> ReviewIfVendorCodeExist(string vendorcode, CancellationToken cancellationToken)
        {
            var result = await _supplierRepository.ReviewVendorCodeExist(vendorcode);
            return !result;
        }
        async Task<bool> ReviewIfEmailExist(string? email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            var result = await _supplierRepository.ReviewEmailExist(email);
            return !result;
        }
    }
}
