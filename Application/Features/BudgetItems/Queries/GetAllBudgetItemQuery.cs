using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Commons.UserManagement;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;
using System.Globalization;
using System.Linq.Expressions;

namespace Application.Features.BudgetItems.Queries
{
    public record GetAllBudgetItemQuery(Guid MWOId) : IRequest<IResult<ListBudgetItemResponse>>;

    public class GetAllBudgetItemQueryHandler : IRequestHandler<GetAllBudgetItemQuery, IResult<ListBudgetItemResponse>>
    {
        private IBudgetItemRepository Repository { get; set; }
        private CurrentUser CurrentUser { get; set; }
        public GetAllBudgetItemQueryHandler(IBudgetItemRepository repository, CurrentUser currentUser)
        {
            Repository = repository;
            CurrentUser = currentUser;
        }

        public async Task<IResult<ListBudgetItemResponse>> Handle(GetAllBudgetItemQuery request, CancellationToken cancellationToken)
        {
            CultureInfo ci = new CultureInfo("en-US");
            ListBudgetItemResponse response = new ListBudgetItemResponse();
            response.MWOName=await Repository.GetMWOName(request.MWOId);    
            var rows = await Repository.GetBudgetItemList(request.MWOId);
            Expression<Func<BudgetItem, BudgetItemResponse>> expression = e => new BudgetItemResponse
            {
                Id = e.Id,
                Name = e.Name,
                Type = BudgetItemTypeEnum.GetType(e.Type).Name,
                Nomenclatore = $"{BudgetItemTypeEnum.GetLetter(e.Type)}{e.Order}",
                Budget = e.Budget.ToString("C1", ci),
                CreatedBy = e.CreatedByUserName,
                CreatedOn = e.CreatedDate.ToString(),

            };
            var result = rows.Select(expression).ToList();
            result = result.OrderBy(x => x.Nomenclatore).ToList();
            response.BudgetItems = result;

            return Result<ListBudgetItemResponse>.Success(response);
        }
    }

}
