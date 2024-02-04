using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;

namespace Application.Features.BudgetItems.Queries
{
    public record GetSumBudgetItemsQuery(Guid MWOId) : IRequest<IResult<double>>;
    public class GetSumBudgetItemsQueryHandler : IRequestHandler<GetSumBudgetItemsQuery, IResult<double>>
    {
        private IBudgetItemRepository Repository { get; set; }

        public GetSumBudgetItemsQueryHandler(IBudgetItemRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<double>> Handle(GetSumBudgetItemsQuery request, CancellationToken cancellationToken)
        {
            var retorno = await Repository.GetSumBudget(request.MWOId);

            return Result<double>.Success(retorno);
        }
    }
}
