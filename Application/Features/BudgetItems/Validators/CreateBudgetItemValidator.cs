using Application.Interfaces;
using FluentValidation;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Validators
{
    internal class CreateBudgetItemValidator : AbstractValidator<CreateBudgetItemRequest>
    {
        private IBudgetItemRepository Repository { get; set; }
        public CreateBudgetItemValidator(IBudgetItemRepository repository)
        {
            Repository = repository;
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name must be defined")
               .NotNull().WithMessage("Name must be defined");
            RuleFor(x => x.Budget).GreaterThan(0).WithMessage("Budget must be defined");
            RuleFor(x => x).MustAsync(ReviewIfNameExist)
                .WithMessage(data => $"Name already exist in item types:{data.Type.Name} of MWO: {data.MWOName}");
        }
        private async Task<bool> ReviewIfNameExist(CreateBudgetItemRequest request, CancellationToken cancellation)
        {
            var result = await Repository.ReviewNameExist(request.MWOId, request.Type.Id, request.Name);
            return !result;
        }
    }
}
