using Application.Interfaces;
using FluentValidation;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Validators
{
    internal class UpdateBudgetItemValidator : AbstractValidator<UpdateBudgetItemRequest>
    {
        private IBudgetItemRepository Repository { get; set; }
        public UpdateBudgetItemValidator(IBudgetItemRepository repository)
        {
            Repository = repository;
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name must be defined")
               .NotNull().WithMessage("Name must be defined");
            RuleFor(x => x.Budget).GreaterThan(0).WithMessage("Budget must be defined");
            RuleFor(x => x).MustAsync(ReviewIfNameExist)
                .WithMessage(data => $"Name already exist in item type: {data.Type.Name} in MWO: {data.MWOName}");
        }
        private async Task<bool> ReviewIfNameExist(UpdateBudgetItemRequest request, CancellationToken cancellation)
        {
            var result = await Repository.ReviewNameExist(request.Id,request.MWOId, request.Type.Id, request.Name);
            return !result;
        }
    }
}
