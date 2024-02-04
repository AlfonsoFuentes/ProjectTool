using Shared.Models.BudgetItems;

namespace ClientRadzen.Validators.BudgetItems
{
    public class CreateBudgetItemValidator:AbstractValidator<CreateBudgetItemRequest>
    {
        public CreateBudgetItemValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().NotNull().WithMessage("Name must be defined");
            RuleFor(x => x.Budget).GreaterThan(0).WithMessage("Budget must be defined");
        }
    }
}
