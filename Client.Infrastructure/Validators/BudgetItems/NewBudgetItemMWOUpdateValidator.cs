using Client.Infrastructure.Managers.BudgetItems;
using Shared.NewModels.BudgetItems.Request;

namespace Client.Infrastructure.Validators.BudgetItems
{
    public class NewBudgetItemMWOUpdateValidator : AbstractValidator<NewBudgetItemMWOUpdateRequest>
    {
        private IBudgetItemValidatorService _service { get; set; }
        public NewBudgetItemMWOUpdateValidator(IBudgetItemValidatorService service)
        {
            _service = service;
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name must be defined")
               .NotNull().WithMessage("Name must be defined");
            RuleFor(x => x.Budget).GreaterThan(0).WithMessage("Budget must defined");

            RuleFor(x => x.TaxesSelectedItems.Count).NotEqual(0).When(x => !x.IsMainItemTaxesNoProductive && x.IsTaxesData).WithMessage("Must selected Items to Apply Taxes");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                      .WithMessage(data => $"{data.Name} already exist in item types in MWO: {data.MWOName}");
        }
        private async Task<bool> ReviewIfNameExist(NewBudgetItemMWOUpdateRequest request, string name, CancellationToken cancellation)
        {
            var result = await _service.ValidateNameExist(request.BudgetItemId, request.MWOId, request.Name);
            return !result;
        }
    }
}
