using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using System.Linq.Expressions;

namespace Application.Features.BudgetItems.Queries
{
    public record GetByIdBudgetItemQuery(Guid Id) : IRequest<IResult<UpdateBudgetItemRequest>>;

    public class GetByIdBudgetItemQueryHandler : IRequestHandler<GetByIdBudgetItemQuery, IResult<UpdateBudgetItemRequest>>
    {
        private IBudgetItemRepository Repository { get; set; }

        public GetByIdBudgetItemQueryHandler(IBudgetItemRepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<UpdateBudgetItemRequest>> Handle(GetByIdBudgetItemQuery request, CancellationToken cancellationToken)
        {
            var row = await Repository.GetBudgetItemWithBrandById(request.Id);

            if (row == null)
            {

                return Result<UpdateBudgetItemRequest>.Fail("Not found!");
            }

            UpdateBudgetItemRequest response = new()
            {
                Id = row.Id,
                Name = row.Name,
                Type = BudgetItemTypeEnum.GetType(row.Type),
                Brand = row.Brand == null ? null : new Shared.Models.Brands.BrandResponse() { Id = row.Brand!.Id, Name = row.Brand!.Name },
                Existing = row.Existing,
                MWOId = row.MWOId,
                Budget = Math.Round(row.Budget, 2),
                Model = row.Model,
                Percentage = row.Percentage,
                Quantity = row.Quantity,
                UnitaryCost = row.UnitaryCost,
                Reference = row.Reference,
                MWOName = row.MWO.Name,
                Nomenclatore = $"{BudgetItemTypeEnum.GetLetter(row.Type)}{row.Order}",


            };
            if (row.Type == BudgetItemTypeEnum.Taxes.Id)
            {

                Expression<Func<TaxesItem, BudgetItemDto>> expression = e => new BudgetItemDto
                {
                    Id = e.SelectedId,
                    BudgetItemId = e.BudgetItemId,
                    Budget = e.Selected.Budget,
                    Name = e.Selected.Name,

                };

                var taxesList = await Repository.GetBudgetItemSelectedTaxesList(row.Id);

                response.SelectedBudgetItemDtos = taxesList.AsQueryable().Select(expression).ToList();
                response.SelectedIdBudgetItemDtos = response.SelectedBudgetItemDtos.Select(x => x.Id).ToList();


            }
            return Result<UpdateBudgetItemRequest>.Success(response);
        }
    }

}
