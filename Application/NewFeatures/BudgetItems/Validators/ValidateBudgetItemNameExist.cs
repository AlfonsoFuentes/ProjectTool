namespace Application.NewFeatures.BudgetItems.Validators
{
    public record ValidateBudgetItemExistingNameExist(Guid BudgetItemId, Guid MWOId, string Name) : IRequest<bool>;
    public class ValidateBudgetItemExistingNameExistHandler : IRequestHandler<ValidateBudgetItemExistingNameExist, bool>
    {
        private readonly IQueryValidationsRepository repository;

        public ValidateBudgetItemExistingNameExistHandler(IQueryValidationsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateBudgetItemExistingNameExist request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfBudgetItemNameExist(request.BudgetItemId, request.MWOId, request.Name);
        }
    }
}
