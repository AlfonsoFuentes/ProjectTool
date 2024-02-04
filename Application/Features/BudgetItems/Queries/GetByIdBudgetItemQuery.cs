using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;

namespace Application.Features.BudgetItems.Queries
{
    public record GetByIdBudgetItemQuery(Guid Id) : IRequest<IResult<BudgetItemResponse>>;

    public class GetByIdBudgetItemQueryHandler : IRequestHandler<GetByIdBudgetItemQuery, IResult<BudgetItemResponse>>
    {
        private IBudgetItemRepository Repository { get; set; }

        public GetByIdBudgetItemQueryHandler(IBudgetItemRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<BudgetItemResponse>> Handle(GetByIdBudgetItemQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetBudgetItemById(request.Id);

            if (row == null)
            {

                return Result<BudgetItemResponse>.Fail("Not found!");
            }
            BudgetItemResponse response = new()
            {
                Id = row.Id,
                Name = row.Name,
            };
            return Result<BudgetItemResponse>.Success(response);
        }
    }

}
