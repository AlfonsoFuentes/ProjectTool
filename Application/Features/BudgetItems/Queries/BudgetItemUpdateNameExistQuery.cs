using Application.Interfaces;
using MediatR;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Queries
{
    public record BudgetItemUpdateNameExistQuery(UpdateBudgetItemRequest Data) : IRequest<bool>;
    public class BudgetItemUpdateNameExistQueryHandler : IRequestHandler<BudgetItemUpdateNameExistQuery, bool>
    {
        private readonly IBudgetItemRepository repository;

        public BudgetItemUpdateNameExistQueryHandler(IBudgetItemRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(BudgetItemUpdateNameExistQuery request, CancellationToken cancellationToken)
        {
            var result= await repository.ReviewNameExist(request.Data.Id, request.Data.Name);
            return result;
        }
    }

}
