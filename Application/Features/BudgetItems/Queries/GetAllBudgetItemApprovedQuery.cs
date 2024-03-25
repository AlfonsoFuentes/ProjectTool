using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Commons.UserManagement;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.CostCenter;
using Shared.Models.MWO;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
using System.Globalization;
using System.Linq.Expressions;

namespace Application.Features.BudgetItems.Queries
{

    public record GetAllBudgetItemApprovedQuery(Guid MWOId) : IRequest<IResult<ListApprovedBudgetItemsResponse>>;
    public class GetAllBudgetItemApprovedQueryHandler : IRequestHandler<GetAllBudgetItemApprovedQuery, IResult<ListApprovedBudgetItemsResponse>>
    {
        private IBudgetItemRepository Repository { get; set; }
        private CurrentUser CurrentUser { get; set; }
        public GetAllBudgetItemApprovedQueryHandler(IBudgetItemRepository repository, CurrentUser currentUser)
        {
            Repository = repository;
            CurrentUser = currentUser;
        }

        public async Task<IResult<ListApprovedBudgetItemsResponse>> Handle(GetAllBudgetItemApprovedQuery request, CancellationToken cancellationToken)
        {
          
            ListApprovedBudgetItemsResponse response = new ListApprovedBudgetItemsResponse();
            var mwo = await Repository.GetMWOById(request.MWOId);
            if (mwo == null) Result<ListApprovedBudgetItemsResponse>.Fail("MWO Not found");

            MWOResponse mworesponse = new()
            {
                Id = mwo!.Id,
                Name = mwo!.Name,
                CECName = $"CEC0000{mwo!.MWONumber}",
                CostCenter = CostCenterEnum.GetName(mwo.CostCenter),

            };

         

           
            var rows = await Repository.GetBudgetItemsWithPurchaseordersList(request.MWOId);
            Expression<Func<BudgetItem, BudgetItemApprovedResponse>> expression = e => new BudgetItemApprovedResponse
            {

                BudgetItemId= e.Id,
                Name = e.Name,
                Order = e.Order,
                Type = BudgetItemTypeEnum.GetType(e.Type),

                Budget = e.Budget,
                CreatedBy = e.CreatedByUserName,
                CreatedOn = e.CreatedDate.ToString(),
                IsMainItemTaxesNoProductive = e.IsMainItemTaxesNoProductive,
                Quantity = e.Quantity,

                //MWO = new()
                //{
                //    Id = mwo.Id,
                //    Name = mwo.Name,
                //    IsAssetProductive = mwo.IsAssetProductive,
                //},
                Percentage = e.Percentage,
                Brand = e.Brand == null ? string.Empty : e.Brand.Name,
                


            };
            var result = rows.Select(expression).ToList();



            result = result.OrderBy(x => x.Nomenclatore).ToList();
            response.BudgetItems = result;


            return Result<ListApprovedBudgetItemsResponse>.Success(response);
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



