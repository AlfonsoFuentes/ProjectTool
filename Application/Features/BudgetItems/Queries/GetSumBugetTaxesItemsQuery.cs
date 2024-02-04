using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;

namespace Application.Features.BudgetItems.Queries
{
    public record GetSumBugetTaxesItemsQuery(Guid BudgetItemId) : IRequest<IResult<double>>;
    public class GetSumBugetTaxesItemsQueryHandler : IRequestHandler<GetSumBugetTaxesItemsQuery, IResult<double>>
    {
        private IBudgetItemRepository Repository { get; set; }

        public GetSumBugetTaxesItemsQueryHandler(IBudgetItemRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<double>> Handle(GetSumBugetTaxesItemsQuery request, CancellationToken cancellationToken)
        {
            var retorno = await Repository.GetSumTaxes(request.BudgetItemId);

            return Result<double>.Success(retorno);
        }
    }
}
