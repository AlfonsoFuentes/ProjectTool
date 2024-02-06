using Shared.Models.Suppliers;

namespace Client.Infrastructure.Validators.Suppliers
{
    public class UpdateSupplierValidator : AbstractValidator<UpdateSupplierRequest>
    {
        public UpdateSupplierValidator()
        {
            RuleFor(x => x.ContactEmail).EmailAddress().WithMessage("Contact email must be valid");

        }
    }
}
