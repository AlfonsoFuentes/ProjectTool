using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.Suppliers;
using System.Linq.Expressions;

namespace Application.Features.BudgetItems.Queries
{
    public record GetBudgetItemsNotApprovedQuery(Guid MWOId) : IRequest<IQueryable<BudgetItemNotApprovedExportFileResponse>>;
    internal class GetBudgetItemsNotApprovedQueryHandler
        : IRequestHandler<GetBudgetItemsNotApprovedQuery, IQueryable<BudgetItemNotApprovedExportFileResponse>>

    {
        private readonly IBudgetItemRepository Repository;

        public GetBudgetItemsNotApprovedQueryHandler(IBudgetItemRepository repository)
        {
            Repository = repository;
        }

        public async Task<IQueryable<BudgetItemNotApprovedExportFileResponse>> Handle(GetBudgetItemsNotApprovedQuery request, CancellationToken cancellationToken)
        {
            var rows = await Repository.GetBudgetItemWithMWOList(request.MWOId);
            Expression<Func<BudgetItem, BudgetItemNotApprovedExportFileResponse>> expression = e => new BudgetItemNotApprovedExportFileResponse
            {
                Brand = e.Brand == null ? string.Empty : e.Brand.Name,
                Budget = $"{e.Budget}",
                Model = e.Model!,
                Name = e.Name,
                Nomenclatore = $"{BudgetItemTypeEnum.GetLetter(e.Type)}{e.Order}",
                Quantity = $"{e.Quantity}",
                UnitaryValue = $"{e.UnitaryCost}",
                Type = BudgetItemTypeEnum.GetName(e.Type),
                MWOName=e.MWO.Name,

            };
            var result = rows!.Select(expression);
          
            return result;
        }
    }
}


