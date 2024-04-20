using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using System.Linq.Expressions;

namespace Application.Features.BudgetItems.Queries
{
    public record GetAllBudgetItemQuery(Guid MWOId) : IRequest<IResult<ListBudgetItemResponse>>;

    public class GetAllBudgetItemQueryHandler : IRequestHandler<GetAllBudgetItemQuery, IResult<ListBudgetItemResponse>>
    {
        private IBudgetItemRepository Repository { get; set; }
    
        public GetAllBudgetItemQueryHandler(IBudgetItemRepository repository)
        {
            Repository = repository;
         
        }

        public async Task<IResult<ListBudgetItemResponse>> Handle(GetAllBudgetItemQuery request, CancellationToken cancellationToken)
        {

            ListBudgetItemResponse response = new ListBudgetItemResponse();
            var mwo = await Repository.GetMWOById(request.MWOId);
            if (mwo == null) Result<ListBudgetItemResponse>.Fail("MWO Not found");

            response.MWO = new()
            {
                Id = mwo!.Id,
                Name = mwo!.Name,
                IsAssetProductive = mwo!.IsAssetProductive,
                PercentageAssetNoProductive = mwo!.PercentageAssetNoProductive,
                PercentageContingency = mwo!.PercentageContingency,
                PercentageEngineering = mwo!.PercentageEngineering,
                PercentageTaxForAlterations = mwo!.PercentageTaxForAlterations,


            };

            var rows = await Repository.GetBudgetItemList(request.MWOId);


            Expression<Func<BudgetItem, BudgetItemResponse>> expression = e => new BudgetItemResponse
            {

                Id = e.Id,
                Name = e.Name,

                Type = BudgetItemTypeEnum.GetType(e.Type),
                Nomenclatore = $"{BudgetItemTypeEnum.GetLetter(e.Type)}{e.Order}",
                Budget = e.Budget,
                CreatedBy = e.CreatedByUserName,
                CreatedOn = e.CreatedDate.ToString(),
                IsNotAbleToEditDelete = e.IsNotAbleToEditDelete,
                IsMainItemTaxesNoProductive = e.IsMainItemTaxesNoProductive,
                Quantity = e.Quantity,
                UnitaryCost = e.UnitaryCost,
                MWO = response.MWO,
                Percentage = e.Percentage,
                Brand = e.Brand == null ? string.Empty : e.Brand.Name,


            };
            var result = rows.Select(expression).ToList();
            var resultTaxes = result.Where(x => !(x.Type.Id == BudgetItemTypeEnum.Engineering.Id
                || x.Type.Id == BudgetItemTypeEnum.Contingency.Id
                || x.Type.Id == BudgetItemTypeEnum.Taxes.Id
                || x.Type.Id == BudgetItemTypeEnum.Alterations.Id)).Select(z => new BudgetItemDto()
                {
                    
                    Budget = z.Budget,
                    BudgetItemId = z.Id,
                    Name = z.Name,
                    Nomenclatore = z.Nomenclatore,

                }).ToList();
            await GetDataForTaxes(result);

            result = result.OrderBy(x => x.Nomenclatore).ToList();
            response.BudgetItems = result;
            response.BudgetItemsToApplyTaxes = resultTaxes;

            return Result<ListBudgetItemResponse>.Success(response);
        }
        async Task GetDataForTaxes(List<BudgetItemResponse> result)
        {
            Expression<Func<TaxesItem, BudgetItemDto>> expression = e => new BudgetItemDto
            {
                SelectedItemId = e.SelectedId,
                BudgetItemId = e.BudgetItemId,
                Budget = e.Selected.Budget,
                Name = e.Selected.Name,

            };
            var Taxes = result.Where(x => x.Type.Id == BudgetItemTypeEnum.Taxes.Id && !x.IsMainItemTaxesNoProductive).ToList();
            foreach (var row in Taxes)
            {
                var taxesList = await Repository.GetBudgetItemSelectedTaxesList(row.Id);
                row.SelectedBudgetItemForTaxes = taxesList.AsQueryable().Select(expression).ToList();
            }

        }

    }

}



