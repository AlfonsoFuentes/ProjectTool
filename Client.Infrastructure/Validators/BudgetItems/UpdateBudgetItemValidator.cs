using Client.Infrastructure.Managers.BudgetItems;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Validators
{
    public class UpdateBudgetItemValidator : AbstractValidator<UpdateBudgetItemRequest>
    {
        private IBudgetItemValidatorService _service { get; set; }
        public UpdateBudgetItemValidator(IBudgetItemValidatorService service)
        {
            _service = service;
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name must be defined")
               .NotNull().WithMessage("Name must be defined");
           
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                      .WithMessage(data => $"{data.Name} already exist in item types in MWO: {data.MWOName}");
        }
        private async Task<bool> ReviewIfNameExist(UpdateBudgetItemRequest request,string name, CancellationToken cancellation)
        {
            var result = await _service.ValidateNameExist(request.Id, request.MWOId, request.Name);
            return !result;
        }
    }
}
