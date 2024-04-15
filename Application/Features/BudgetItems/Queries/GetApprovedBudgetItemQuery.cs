using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.CostCenter;
using Shared.Models.Currencies;
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
            var purchaseordersbyitem = await _purchaseOrderRepository.GetPurchaseorderByBudgetItem(request.BudgetItemId);

            BudgetItemApprovedResponse budgetItemResponse = new()
            {
                BudgetItemId = budgetItem.Id,
                Budget = budgetItem.Budget,
                MWOCECName = $"CEC0000{budgetItem.MWO.MWONumber}",
                MWOId = budgetItem.MWOId,
                MWOName = budgetItem.MWO.Name,
                CostCenter = CostCenterEnum.GetName(budgetItem.MWO.CostCenter),

                IsMainItemTaxesNoProductive = budgetItem.IsMainItemTaxesNoProductive,
                Name = budgetItem.Name,
                Type = BudgetItemTypeEnum.GetType(budgetItem.Type),
                Order = budgetItem.Order,


            };
            budgetItemResponse.PurchaseOrders = purchaseordersbyitem.Select(x => new PurchaseOrderResponse()
            {
                AccountAssigment = x.AccountAssigment,
                IsAlteration = x.IsAlteration,
                IsCapitalizedSalary = x.IsCapitalizedSalary,
                IsTaxEditable = x.IsTaxEditable,
                MWOId = budgetItem.MWOId,
                MWOName = budgetItemResponse.MWOName,
                CECName = budgetItemResponse.MWOCECName,
                IsTaxNoProductive = budgetItem.IsMainItemTaxesNoProductive,
                PONumber = x.PONumber,
                PurchaseOrderId = x.Id,
                PurchaseorderName = x.PurchaseorderName,
                PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(x.PurchaseOrderStatus),
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedDate.ToShortDateString(),
                ExpectedOn = x.POExpectedDateDate == null ? string.Empty : x.POExpectedDateDate.Value.ToShortDateString(),
                PurchaseRequisition = x.PurchaseRequisition,
                QuoteNo = x.QuoteNo,
                SupplierNickName = x.Supplier == null ? string.Empty : x.Supplier.NickName,
                SupplierName = x.Supplier == null ? string.Empty : x.Supplier.Name,
                SupplierId = x.SupplierId,
                TaxCode = x.TaxCode,
                VendorCode = x.Supplier == null ? string.Empty : x.Supplier.VendorCode,
                ReceivedOn = x.POClosedDate == null ? string.Empty : x.POClosedDate.Value.ToShortDateString(),
                PurchaseOrderItems = x.PurchaseOrderItems.Select(y => new PurchaseorderItemsResponse()
                {
                    PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(x.PurchaseOrderStatus),
                    UnitaryValueCurrency=y.UnitaryValueCurrency,
                    Quantity = y.Quantity,
                    
                    Currency = CurrencyEnum.GetType(x.Currency),
                    USDCOP = x.USDCOP,
                    USDEUR = x.USDEUR,
                    BudgetItemId = y.BudgetItemId,
                    PurchaseorderItemId = y.PurchaseOrderId,
                   
                }
                ).ToList(),

            }).ToList();


            return Result<BudgetItemApprovedResponse>.Success(budgetItemResponse);
        }
    }
}
