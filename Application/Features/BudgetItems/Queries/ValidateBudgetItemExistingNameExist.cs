using Application.Interfaces;
using MediatR;

namespace Application.Features.BudgetItems.Queries
{
    public record ValidateBudgetItemExistingNameExist(Guid BudgetItemId, Guid MWOId, string Name) : IRequest<bool>;
    public class ValidateBudgetItemExistingNameExistHandler : IRequestHandler<ValidateBudgetItemExistingNameExist, bool>
    {
        private readonly IBudgetItemRepository repository;

        public ValidateBudgetItemExistingNameExistHandler(IBudgetItemRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateBudgetItemExistingNameExist request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfNameExist(request.BudgetItemId, request.MWOId, request.Name);
        }
    }
}
