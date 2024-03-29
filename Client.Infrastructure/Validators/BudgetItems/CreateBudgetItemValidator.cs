using Client.Infrastructure.Managers.BudgetItems;
using Shared.Models.BudgetItems;

namespace Client.Infrastructure.Validators.BudgetItems
{
    public class CreateBudgetItemValidator : AbstractValidator<CreateBudgetItemRequest>
    {
        private IBudgetItemValidatorService _service { get; set; }
        public CreateBudgetItemValidator(IBudgetItemValidatorService service)
        {
            _service = service;
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name must be defined");
            RuleFor(x => x.Name)

                .NotNull().WithMessage("Name must be defined");

            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist).When(x => !string.IsNullOrWhiteSpace(x.Name))
                .WithMessage(data => $"{data.Name} already exist in item types in MWO: {data.MWO.Name}");

            RuleFor(x => x.Budget).GreaterThan(0).WithMessage("Budget must defined");

            RuleFor(x => x.BudgetItemDtos.Count).NotEqual(0).When(x => x.IsTaxesData).WithMessage("Must selected Items to Apply Taxes");


        }
        private async Task<bool> ReviewIfNameExist(CreateBudgetItemRequest request, string name, CancellationToken cancellation)
        {
            var result = await _service.ValidateNameExist(request.MWO.Id, request.Name);
            return !result;
        }
    }
}
