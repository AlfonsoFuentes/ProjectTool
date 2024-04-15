﻿using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
using Shared.Models.CostCenter;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Queries
{
    public record GetPurchaseOrderCreatedToEditById(Guid PurchaseOrderId) : IRequest<IResult<EditPurchaseOrderRegularCreatedRequest>>;
    internal class GetPurchaseOrderCreatedToEditByIdHandler : IRequestHandler<GetPurchaseOrderCreatedToEditById, IResult<EditPurchaseOrderRegularCreatedRequest>>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;

        public GetPurchaseOrderCreatedToEditByIdHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<EditPurchaseOrderRegularCreatedRequest>> Handle(GetPurchaseOrderCreatedToEditById request, CancellationToken cancellationToken)
        {
            PurchaseOrder purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderWithItemsAndSupplierById(request.PurchaseOrderId);
            var budgtitem = await _purchaseOrderRepository.GetBudgetItemWithMWOById(purchaseOrder.MainBudgetItemId);
            if (purchaseOrder == null)
            {
                return Result<EditPurchaseOrderRegularCreatedRequest>.Fail("Not found");
            }
            EditPurchaseOrderRegularCreatedRequest result = new()
            {
                MWOId = purchaseOrder.MWOId,
                CostCenter = CostCenterEnum.GetName(purchaseOrder.MWO.CostCenter),
                IsAssetProductive = purchaseOrder.MWO.IsAssetProductive,
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
                PurchaseorderName = purchaseOrder.PurchaseorderName,
                PurchaseOrderId = purchaseOrder.Id,
                PurchaseRequisition = purchaseOrder.PurchaseRequisition,
                QuoteNo = purchaseOrder.QuoteNo,
                PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.Currency),
                QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                USDCOP = purchaseOrder.USDCOP,
                USDEUR = purchaseOrder.USDEUR,
                CurrencyDate = purchaseOrder.CurrencyDate,
                PurchaseOrderItems = purchaseOrder.PurchaseOrderItems.Select(x => new PurchaseOrderItemRequest()
                {
                   
                    BudgetItemId = x.BudgetItemId,
                    PurchaseOrderItemId = x.Id,
                    Name = x.Name,
                    Quantity = x.Quantity,
                    BudgetItemName = x.BudgetItem.Name,
                    TRMUSDCOP = purchaseOrder.USDCOP,
                    TRMUSDEUR = purchaseOrder.USDEUR,
                    Currency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                    CurrencyUnitaryValue = x.UnitaryValueCurrency ,

                    Budget = x.BudgetItem.Budget,
                    AssignedCurrency = x.PurchaseOrder.PurchaseOrderStatus != PurchaseOrderStatusEnum.Created.Id ? 
                    x.BudgetItem.PurchaseOrderItems.Sum(x => x.UnitaryValueCurrency * x.Quantity) : 0,
                    PotencialCurrency = x.PurchaseOrder.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id ?
                    x.BudgetItem.PurchaseOrderItems.Sum(x => x.UnitaryValueCurrency * x.Quantity) : 0,
                    ActualCurrency = x.ActualCurrency,

                }).ToList(),

                Supplier = purchaseOrder.Supplier == null ? new() : new()
                {
                    Id = purchaseOrder.Supplier.Id,
                    Name= purchaseOrder.Supplier.Name,
                    VendorCode = purchaseOrder.Supplier.VendorCode,
                    TaxCodeLD = purchaseOrder.Supplier.TaxCodeLD,
                    TaxCodeLP = purchaseOrder.Supplier.TaxCodeLP,
                    NickName = purchaseOrder.Supplier.NickName,
                },




            };
            return Result<EditPurchaseOrderRegularCreatedRequest>.Success(result);
        }
    }
}
