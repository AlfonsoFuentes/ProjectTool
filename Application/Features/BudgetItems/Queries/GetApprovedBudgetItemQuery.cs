using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.CostCenter;
using Shared.Models.MWOTypes;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.BudgetItems.Queries
{
    public record GetApprovedBudgetItemQuery(Guid BudgetItemId) : IRequest<IResult<BudgetItemApprovedResponse>>;
    internal class GetApprovedBudgetItemQueryHandler : IRequestHandler<GetApprovedBudgetItemQuery, IResult<BudgetItemApprovedResponse>>
    {

        private IPurchaseOrderRepository _purchaseOrderRepository;
        public GetApprovedBudgetItemQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<BudgetItemApprovedResponse>> Handle(GetApprovedBudgetItemQuery request, CancellationToken cancellationToken)
        {
            var budgetItem = await _purchaseOrderRepository.GetBudgetItemWithMWOById(request.BudgetItemId);

            if (budgetItem == null)
            {
                return Result<BudgetItemApprovedResponse>.Fail("Budget Item Not found!");
            }


            BudgetItemApprovedResponse budgetItemResponse = new()
            {
                BudgetItemId = budgetItem.Id,
                Budget = budgetItem.Budget,
                MWOCECName = $"CEC0000{budgetItem.MWO.MWONumber}",
                MWOId = budgetItem.MWOId,
                MWOName = budgetItem.MWO.Name,
                CostCenter=CostCenterEnum.GetName(budgetItem.MWO.CostCenter),

                IsMainItemTaxesNoProductive = budgetItem.IsMainItemTaxesNoProductive,
                Name = budgetItem.Name,
                Type = BudgetItemTypeEnum.GetType(budgetItem.Type),
                Order = budgetItem.Order,
                //PurchaseOrders = budgetItem.PurchaseOrderItems.Select(x =>
                //new PurchaseOrderItemForBudgetItemResponse()
                //{
                //    POValueUSD = x.POValueUSD,
                //    IsAlteration = x.PurchaseOrder.IsAlteration,
                //    IsCapitalizedSalary = x.PurchaseOrder.IsCapitalizedSalary,
                //    IsTaxNoProductive = x.IsTaxNoProductive,
                //    IsTaxEditable = x.PurchaseOrder.IsTaxEditable,
                //    BudgetItemId = x.BudgetItemId,
                //    Actual = x.Actual,
                //    PurchaseorderItemId = x.Id,
                //    PurchaseorderName = x.PurchaseOrder.PurchaseorderName,
                //    PurchaseorderNumber = x.PurchaseOrder.PONumber,
                //    PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(x.PurchaseOrder.PurchaseOrderStatus),
                //    PurchaseRequisition = x.PurchaseOrder.PurchaseRequisition,


                //}).ToList(),

            };



            return Result<BudgetItemApprovedResponse>.Success(budgetItemResponse);
        }
    }
}
