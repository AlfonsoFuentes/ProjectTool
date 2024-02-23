using Shared.Models.BudgetItems;

namespace Client.Infrastructure.Validators.BudgetItems
{
    public class CreateBudgetItemValidator : AbstractValidator<CreateBudgetItemRequest>
    {
        public CreateBudgetItemValidator()
        {

            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("Name must be defined");
            RuleFor(x => x.Budget).GreaterThan(0).WithMessage("Budget must be defined");
            RuleFor(x => x.Percentage).GreaterThan(0).When(x => x.IsContingencyData).WithMessage("Percentage must be defined");
            RuleFor(x => x.BudgetItemDtos.Count).NotEqual(0).When(x => x.IsTaxesData).WithMessage("Must selected Items to Apply Taxes");
        }
    }
}
