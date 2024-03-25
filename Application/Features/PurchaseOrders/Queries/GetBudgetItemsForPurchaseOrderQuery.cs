using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.CostCenter;
using Shared.Models.Currencies;
using Shared.Models.MWO;
using Shared.Models.MWOTypes;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
using Shared.Models.Suppliers;
using System.Linq.Expressions;

namespace Application.Features.PurchaseOrders.Queries
{
    public record GetBudgetItemsForPurchaseOrderQuery(Guid BudgetItemId) : IRequest<IResult<BudgetItemsListForPurchaseordersResponse>>;

    internal class GetDataFotCreatePurchaseOrderQueryHandler : IRequestHandler<GetBudgetItemsForPurchaseOrderQuery, IResult<BudgetItemsListForPurchaseordersResponse>>
    {

        private IPurchaseOrderRepository _purchaseOrderRepository;
        public GetDataFotCreatePurchaseOrderQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<BudgetItemsListForPurchaseordersResponse>> Handle(GetBudgetItemsForPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            var budgetItem = await _purchaseOrderRepository.GetBudgetItemById(request.BudgetItemId);

            if (budgetItem == null)
            {
                return Result<BudgetItemsListForPurchaseordersResponse>.Fail("Budget Item Not found!");
            }

            var mwo = await _purchaseOrderRepository.GetMWOWithBudgetItemsAndPurchaseOrderById(budgetItem.MWOId);
            if (mwo == null)
            {
                return Result<BudgetItemsListForPurchaseordersResponse>.Fail("MWO Not found!");
            }


            Func<BudgetItem, bool> Criteria = budgetItem.Type == BudgetItemTypeEnum.Alterations.Id ?
                x => x.Type == BudgetItemTypeEnum.Alterations.Id && x.Id != budgetItem.Id :
                x => (x.Type != BudgetItemTypeEnum.Alterations.Id &&
                x.Type != BudgetItemTypeEnum.Taxes.Id &&
                x.Type != BudgetItemTypeEnum.Engineering.Id && x.Id != budgetItem.Id);
            var BudgetItems = mwo.BudgetItems.Where(Criteria).Select(x => new BudgetItemApprovedResponse()
            {
                BudgetItemId = x.Id,
                Name = x.Name,
                Budget = x.Budget,
                Order = x.Order,
     
                Type = BudgetItemTypeEnum.GetType(x.Type),
               
                
            }).OrderBy(x => x.Nomenclatore).ToList();

            BudgetItemsListForPurchaseordersResponse result = new();
           
            result.BudgetItems = BudgetItems;
            return Result<BudgetItemsListForPurchaseordersResponse>.Success(result);
        }
    }
}
