namespace Application.Features.BudgetItems.Queries
{
    public record ValidateBudgetItemNameExist(Guid MWOId, string Name):IRequest<bool>;
    public class ValidateBudgetItemNameExistHandler : IRequestHandler<ValidateBudgetItemNameExist, bool>
    {
        private readonly IBudgetItemRepository repository;

        public ValidateBudgetItemNameExistHandler(IBudgetItemRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateBudgetItemNameExist request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfNameExist(request.MWOId, request.Name);
        }
    }
}
