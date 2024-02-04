using Application.Features.MWOs.Queries;
using Application.Interfaces;
using MediatR;

namespace Application.Features.BudgetItems.Queries
{
    public record BudgetItemCreateNameExistQuery(string Name) : IRequest<bool>;
    public class BudgetItemNameExistQueryHandler : IRequestHandler<BudgetItemCreateNameExistQuery, bool>
    {
        private readonly IBudgetItemRepository repository;

        public BudgetItemNameExistQueryHandler(IBudgetItemRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(BudgetItemCreateNameExistQuery request, CancellationToken cancellationToken)
        {
            return await repository.ReviewNameExist(request.Name);
        }
    }

}
