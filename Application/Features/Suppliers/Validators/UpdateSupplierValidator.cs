using Application.Interfaces;
using FluentValidation;
using Shared.Models.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Suppliers.Validators
{
    public class UpdateSupplierValidator:AbstractValidator<UpdateSupplierRequest>
    {
        private ISupplierRepository _supplierRepository;
        public UpdateSupplierValidator(ISupplierRepository supplierRepository)
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
            RuleFor(x=>x.ContactEmail).EmailAddress().When(x=>!string.IsNullOrWhiteSpace(x.ContactEmail)).WithMessage("Must Supply Valid email");
            RuleFor(x => x).MustAsync(ReviewIfNameExist).WithMessage(x=>$"Vendor Name:{x.Name} already exist");
            RuleFor(x => x).MustAsync(ReviewIfVendorCodeExist).WithMessage(x => $"Vendor Code:{x.VendorCode} already exist");
            RuleFor(x => x).MustAsync(ReviewIfEmailExist).WithMessage(x => $"Vendor email:{x.ContactEmail} already exist");
        }
        async Task<bool> ReviewIfNameExist(UpdateSupplierRequest supplier, CancellationToken cancellationToken)
        {
            var result = await _supplierRepository.ReviewNameExist(supplier.Id, supplier.Name);
            return !result;
        }
        async Task<bool> ReviewIfVendorCodeExist(UpdateSupplierRequest supplier, CancellationToken cancellationToken)
        {
            var result = await _supplierRepository.ReviewVendorCodeExist(supplier.Id, supplier.VendorCode);
            return !result;
        }
        async Task<bool> ReviewIfEmailExist(UpdateSupplierRequest supplier, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(supplier.ContactEmail)) return true;
            var result = await _supplierRepository.ReviewEmailExist(supplier.Id, supplier.ContactEmail);
            return !result;
        }
    }
}
