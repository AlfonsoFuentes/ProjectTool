using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using System.Globalization;
using System.Linq.Expressions;

namespace Application.Features.BudgetItems.Queries
{
    public record GetDataForCreateBudgetItemQuery(Guid MWOId) : IRequest<IResult<DataforCreateBudgetItemResponse>>;
    public class GetAllBudgetItemForTaxesListQueryHandler : IRequestHandler<GetDataForCreateBudgetItemQuery, IResult<DataforCreateBudgetItemResponse>>
    {
        private IBudgetItemRepository Repository { get; set; }
      
        public GetAllBudgetItemForTaxesListQueryHandler(IBudgetItemRepository repository)
        {
            Repository = repository;
        
        }

        public async Task<IResult<DataforCreateBudgetItemResponse>> Handle(GetDataForCreateBudgetItemQuery request, CancellationToken cancellationToken)
        {
            CultureInfo ci = new CultureInfo("en-US");
            DataforCreateBudgetItemResponse response = new DataforCreateBudgetItemResponse();
            response.MWOName = await Repository.GetMWOName(request.MWOId);
            var rows = await Repository.GetBudgetItemForTaxesList(request.MWOId);
            Expression<Func<BudgetItem, BudgetItemDto>> expression = e => new BudgetItemDto
            {
                
                Name = $"{e.Name} {String.Format(new System.Globalization.CultureInfo("en-US"), "{0:C}", e.Budget)}",
                BudgetItemId = e.Id,
                Nomenclatore = $"{BudgetItemTypeEnum.GetLetter(e.Type)}{e.Order}",
                Budget = e.Budget,


            };
            var result = rows.AsQueryable().Select(expression).ToList();
            result = result.OrderBy(x => x.Nomenclatore).ToList();
            response.BudgetItems = result;
            var sumPercentage = await Repository.GetSumEngConPercentage(request.MWOId);
            var SumBudget = await Repository.GetSumBudget(request.MWOId);

            response.SumEngContPercentage = sumPercentage;
            response.SumBudget = SumBudget;

            return Result<DataforCreateBudgetItemResponse>.Success(response);
        }
    }
}
