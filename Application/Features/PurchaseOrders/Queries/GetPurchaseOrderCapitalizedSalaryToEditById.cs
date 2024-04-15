﻿using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
using Shared.Models.CostCenter;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;

namespace Application.Features.PurchaseOrders.Queries
{
    public record GetPurchaseOrderCapitalizedSalaryToEditById(Guid PurchaseOrderId) : IRequest<IResult<EditCapitalizedSalaryPurchaseOrderRequest>>;

    internal class GetPurchaseOrderCapitalizedSalaryToEditByIdHandler : IRequestHandler<GetPurchaseOrderCapitalizedSalaryToEditById, IResult<EditCapitalizedSalaryPurchaseOrderRequest>>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;

        public GetPurchaseOrderCapitalizedSalaryToEditByIdHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<EditCapitalizedSalaryPurchaseOrderRequest>> Handle(GetPurchaseOrderCapitalizedSalaryToEditById request, CancellationToken cancellationToken)
        {
            PurchaseOrder purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderWithItemsAndSupplierById(request.PurchaseOrderId);
            var budgtitem = await _purchaseOrderRepository.GetBudgetItemWithMWOById(purchaseOrder.MainBudgetItemId);
            if (purchaseOrder == null)
            {
                return Result<EditCapitalizedSalaryPurchaseOrderRequest>.Fail("Not found");
            }
            EditCapitalizedSalaryPurchaseOrderRequest result = new()
            {

                MWOId = purchaseOrder.MWOId,
                CostCenter = CostCenterEnum.GetName(purchaseOrder.MWO.CostCenter),

                MWOCECName = $"CEC0000{purchaseOrder.MWO.MWONumber}",
                MWOName = purchaseOrder.MWO.Name,
                MainBudgetItem = new()
                {
                    Budget = budgtitem.Budget,
                    Brand = budgtitem.Brand == null ? string.Empty : budgtitem.Brand.Name,
                    BudgetItemId = budgtitem.Id,
                    Name = budgtitem.Name,
                    Type = BudgetItemTypeEnum.GetType(budgtitem.Type),
                    IsMainItemTaxesNoProductive = budgtitem.IsMainItemTaxesNoProductive,


                },
                PurchaseOrderName = purchaseOrder.PurchaseorderName,
                PurchaseOrderId = purchaseOrder.Id,
                PurchaseorderNumber = purchaseOrder.PONumber,
                PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.Currency),
                IsCapitalizedSalary = purchaseOrder.PONumber == string.Empty,
                USDCOP = purchaseOrder.USDCOP,
                USDEUR = purchaseOrder.USDEUR,
                CurrencyDate = purchaseOrder.CurrencyDate,
                PurchaseOrderItem = purchaseOrder.PurchaseOrderItems.Select(x => new PurchaseOrderItemRequest()
                {
                    Budget = x.BudgetItem.Budget,
                    BudgetItemId = x.BudgetItemId,
                    PurchaseOrderItemId = x.Id,
                    ActualCurrency = x.ActualCurrency,
                    AssignedCurrency = x.ActualCurrency,
                    Currency = CurrencyEnum.GetType(purchaseOrder.Currency),
                    Name = x.Name,
                    Quantity = x.Quantity,
                    BudgetItemName = x.BudgetItem.Name,
                    TRMUSDCOP = purchaseOrder.USDCOP,
                    TRMUSDEUR = purchaseOrder.USDEUR,

                    CurrencyUnitaryValue = x.UnitaryValueCurrency

                }).FirstOrDefault()!,

            };
            return Result<EditCapitalizedSalaryPurchaseOrderRequest>.Success(result);
        }
    }
}
