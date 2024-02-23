using Application.Interfaces;
using Client.Infrastructure.Managers.CostCenter;
using Domain.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Commons.Results;
using Shared.Models.BudgetItems;
using Shared.Models.BudgetItemTypes;
using Shared.Models.Currencies;
using Shared.Models.MWO;
using Shared.Models.MWOTypes;
using Shared.Models.PurchaseOrders.Requests.Create;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
using Shared.Models.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.PurchaseOrders.Queries
{
    public record GetDataForEditPurchaseOrderQuery(Guid PurchaseOrderId) : IRequest<IResult<DataForEditPurchaseOrder>>;

    internal class GetDataForEditPurchaseOrderQueryHandler : IRequestHandler<GetDataForEditPurchaseOrderQuery, IResult<DataForEditPurchaseOrder>>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository { get; set; }

        public GetDataForEditPurchaseOrderQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<DataForEditPurchaseOrder>> Handle(GetDataForEditPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderWithItemsAndSupplierById(request.PurchaseOrderId);

            var budgetItem = await _purchaseOrderRepository.GetBudgetItemWithPurchaseOrdersById(purchaseOrder.MainBudgetItemId);
            var mwo = await _purchaseOrderRepository.GetMWOWithBudgetItemsAndPurchaseOrderById(purchaseOrder.MWOId);
            if (purchaseOrder == null || budgetItem == null || mwo == null)
            {
                return Result<DataForEditPurchaseOrder>.Fail("Not found");
            }

            BudgetItemResponse budgetItemResponse = new()
            {
                Id = budgetItem.Id,
                Budget = budgetItem.Budget,
                MWOId = budgetItem.MWOId,
                Name = budgetItem.Name,
                Type = BudgetItemTypeEnum.GetType(budgetItem.Type),
                Nomenclatore = $"{BudgetItemTypeEnum.GetLetter(budgetItem.Type)}{budgetItem.Order}",
                Assigned = budgetItem.PurchaseOrderItems.Count == 0 ? 0 :
               budgetItem.PurchaseOrderItems.Where(x => x.PurchaseOrder.PurchaseOrderStatus != PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD),
                PotencialAssigned = budgetItem.PurchaseOrderItems.Count == 0 ? 0 :
               budgetItem.PurchaseOrderItems.Where(x => x.PurchaseOrder.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD),
            };

            MWOResponse mWOResponse = new()
            {
                Id = mwo.Id,
                Name = mwo.Name,
                CECName = $"CEC0000{mwo.MWONumber}",
                Capital = mwo.BudgetItems.Where(x => x.Type != BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget),
                Expenses = mwo.BudgetItems.Where(x => x.Type == BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget),
                CostCenter = CostCenterEnum.GetName(mwo.CostCenter),
                Type = MWOTypeEnum.GetName(mwo.Type),
                IsRealProductive = mwo.IsAssetProductive,



            };
            Func<BudgetItem, bool> Criteria = budgetItem.Type == BudgetItemTypeEnum.Alterations.Id ?
                x => x.Type == BudgetItemTypeEnum.Alterations.Id :
                x => (x.Type != BudgetItemTypeEnum.Alterations.Id &&
                x.Type != BudgetItemTypeEnum.Contingency.Id &&
                x.Type != BudgetItemTypeEnum.Taxes.Id &&
                x.Type != BudgetItemTypeEnum.Engineering.Id);
            var BudgetItems = mwo.BudgetItems.Where(Criteria).Select(x => new BudgetItemResponse()
            {
                Id = x.Id,
                Name = x.Name,
                Budget = x.Budget,
                Nomenclatore = $"{BudgetItemTypeEnum.GetLetter(x.Type)}{x.Order}",
                Type = BudgetItemTypeEnum.GetType(x.Type),
                Assigned = x.PurchaseOrderItems.Count == 0 ? 0 : x.PurchaseOrderItems.Where(x =>
                x.PurchaseOrder.PurchaseOrderStatus != PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD),
                PotencialAssigned = x.PurchaseOrderItems.Count == 0 ? 0 : x.PurchaseOrderItems.Where(x =>
                x.PurchaseOrder.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD),
            }).OrderBy(x => x.Nomenclatore).ToList();
            EditPurchaseOrderCreatedRequest PurchaseOrder = new()
            {
                AccountAssigment = purchaseOrder.AccountAssigment,
                IsAlteration = purchaseOrder.IsAlteration,
                MWOName = purchaseOrder.MWO.Name,
                MWOCECName = $"CEC0000{purchaseOrder.MWO.MWONumber}",
                MWOId = purchaseOrder.MWO.Id,
                CurrencyDate = purchaseOrder.CurrencyDate,
                PurchaseorderId = purchaseOrder.Id,
                QuoteNo = purchaseOrder.QuoteNo,
                SupplierId = purchaseOrder.SupplierId,
                TaxCode = purchaseOrder.TaxCode,
                USDCOP = purchaseOrder.USDCOP,
                USDEUR = purchaseOrder.USDEUR,
                VendorCode = purchaseOrder.Supplier == null ? string.Empty : purchaseOrder.Supplier.VendorCode,
                PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.Currency),
                QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                PurchaseRequisition = purchaseOrder.PurchaseRequisition,
                AssetRealProductive = purchaseOrder.MWO.IsAssetProductive,
                PurchaseOrderName = purchaseOrder.PurchaseorderName,
                MainBudgetItemId = purchaseOrder.MainBudgetItemId,
                PurchaseOrderStatusEnum = PurchaseOrderStatusEnum.GetType(purchaseOrder.PurchaseOrderStatus),
                Supplier = purchaseOrder.Supplier == null ? new() : new()
                {
                    Id = purchaseOrder.SupplierId,
                    Name = purchaseOrder.Supplier.Name,
                },
                PurchaseOrderItems = purchaseOrder.PurchaseOrderItems.Select(x =>
                new EditPurchaseorderItemCreatedRequest()
                {
                    Nomenclatore = x.BudgetItem == null ? string.Empty : $"{BudgetItemTypeEnum.GetLetter(x.BudgetItem.Type)}{x.BudgetItem.Order}",
                    PurchaseOrderItemId = x.Id,
                    BudgetItemId = x.BudgetItemId,
                    BudgetItemName = x.BudgetItem == null ? string.Empty : x.BudgetItem.Name,
                    Quantity = x.Quantity,
                    PurchaseOrderItemName = x.Name,
                    USDCOP = x.PurchaseOrder.USDCOP,
                    USDEUR = x.PurchaseOrder.USDEUR,
                    QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),

                    BudgetAssigned = x.BudgetItem == null ? 0 : x.BudgetItem.PurchaseOrderItems.
                            Where(x => x.PurchaseOrder.PurchaseOrderStatus != PurchaseOrderStatusEnum.Created.Id)
                           .Sum(x => x.POValueUSD),
                    Budget = x.BudgetItem == null ? 0 : x.BudgetItem.Budget,
                    CurrencyValue = purchaseOrder.QuoteCurrency == CurrencyEnum.USD.Id ? x.POValueUSD :
                    purchaseOrder.QuoteCurrency == CurrencyEnum.COP.Id ? x.POValueUSD * x.PurchaseOrder.USDCOP :
                    x.POValueUSD * x.PurchaseOrder.USDEUR,
                    BudgetPotencialAssigned = x.BudgetItem == null ? 0 : x.BudgetItem.PurchaseOrderItems.
                            Where(x => x.PurchaseOrderId != purchaseOrder.Id && x.PurchaseOrder.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id)
                           .Sum(x => x.POValueUSD),
                    BudgetItemsListForChange = BudgetItems.Where(y => y.Id != x.BudgetItemId).ToList(),



                }).ToList(),

            };
            var suppliers = await _purchaseOrderRepository.GetSuppliers();
            Expression<Func<Supplier, SupplierResponse>> expressionSupplier = e => new SupplierResponse
            {
                Id = e.Id,
                Name = e.Name,
                VendorCode = e.VendorCode,
                TaxCodeLD = e.TaxCodeLD,
                TaxCodeLP = e.TaxCodeLP,
                SupplierCurrency = CurrencyEnum.GetType(e.SupplierCurrency),
            };
            var resultsuppliers = await suppliers.Select(expressionSupplier).ToListAsync();
            DataForEditPurchaseOrder result = new();
            result.MWO = mWOResponse;
            result.BudgetItem = budgetItemResponse;
            result.Suppliers = resultsuppliers;
            result.OriginalBudgetItems = BudgetItems;
            result.PurchaseOrder = PurchaseOrder;

            return Result<DataForEditPurchaseOrder>.Success(result);

        }
    }
}
