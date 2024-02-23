

namespace Client.Infrastructure.Validators.Suppliers
{
    public class CreateSupplierValidator : AbstractValidator<CreateSupplierRequest>
    {
       

        public CreateSupplierValidator()
        {
           
            RuleFor(x => x.Name).NotEmpty().WithMessage("Supplier Name must be defined!");
            RuleFor(x => x.Name).NotNull().WithMessage("Supplier Name must be defined!");
            RuleFor(x => x.TaxCodeLD).NotEmpty().NotNull().WithMessage("Tax Code LD must be defined!");
            RuleFor(x => x.TaxCodeLP).NotEmpty().NotNull().WithMessage("Tax Code LP must be defined!");
            RuleFor(x => x.SupplierCurrency).NotEqual(Shared.Models.Currencies.CurrencyEnum.None).WithMessage("Supplier Currency must be defined!");
           
            RuleFor(x => x.VendorCode).NotEmpty().WithMessage("Supplier vendor Code must be defined!");
            RuleFor(x => x.VendorCode).NotNull().WithMessage("Supplier vendor Code must be defined!");
            RuleFor(customer => customer.VendorCode).Matches("^[0-9]*$").WithMessage("Supplier vendor Code must be number!");
            RuleFor(customer => customer.TaxCodeLP).Matches("^[0-9]*$").WithMessage("Supplier vendor Code must be number!");
            RuleFor(customer => customer.TaxCodeLD).Matches("^[0-9]*$").WithMessage("Supplier vendor Code must be number!");
       
        }

        
    }
}
