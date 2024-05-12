namespace Application.NewFeatures.BudgetItems.Validators
{
    public record ValidateBudgetItemNameExist(Guid MWOId, string Name) : IRequest<bool>;
    public class ValidateBudgetItemNameExistHandler : IRequestHandler<ValidateBudgetItemNameExist, bool>
    {
        private readonly IQueryValidationsRepository repository;

        public ValidateBudgetItemNameExistHandler(IQueryValidationsRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateBudgetItemNameExist request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfBudgetItemNameExist(request.MWOId, request.Name);
        }
    }
}
