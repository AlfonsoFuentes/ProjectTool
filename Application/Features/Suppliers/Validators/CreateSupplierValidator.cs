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
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist).WithMessage("Vendor Name already exist");
            RuleFor(x => x.VendorCode).MustAsync(ReviewIfVendorCodeExist).WithMessage("Vendor Code already exist");
   
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
