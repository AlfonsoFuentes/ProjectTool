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
using Shared.Models.MWOStatus;
using Shared.Models.MWOTypes;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
using Shared.Models.Suppliers;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Features.PurchaseOrders.Queries
{
    public record GetDataForCreatePurchaseOrderQuery(Guid BudgetItemId) : IRequest<IResult<DataForCreatePurchaseOrder>>;

    internal class GetDataFotCreatePurchaseOrderQueryHandler : IRequestHandler<GetDataForCreatePurchaseOrderQuery, IResult<DataForCreatePurchaseOrder>>
    {

        private IPurchaseOrderRepository _purchaseOrderRepository;
        public GetDataFotCreatePurchaseOrderQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<DataForCreatePurchaseOrder>> Handle(GetDataForCreatePurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            var budgetItem = await _purchaseOrderRepository.GetBudgetItemWithPurchaseOrdersById(request.BudgetItemId);

            if (budgetItem == null)
            {
                return Result<DataForCreatePurchaseOrder>.Fail("Budget Item Not found!");
            }

            var mwo = await _purchaseOrderRepository.GetMWOWithBudgetItemsAndPurchaseOrderById(budgetItem.MWOId);
            if (mwo == null)
            {
                return Result<DataForCreatePurchaseOrder>.Fail("MWO Not found!");
            }
            BudgetItemApprovedResponse budgetItemResponse = new()
            {
                Id = budgetItem.Id,
                Budget = budgetItem.Budget,
                MWOId = budgetItem.MWOId,
                Name = budgetItem.Name,
                Type = BudgetItemTypeEnum.GetType(budgetItem.Type),
                Nomenclatore = $"{BudgetItemTypeEnum.GetLetter(budgetItem.Type)}{budgetItem.Order}",
                PurchaseOrders = budgetItem.PurchaseOrderItems.Count == 0 ? new() :
                budgetItem.PurchaseOrderItems.Select(x => new PurchaseOrderItemForBudgetItemResponse()
                {
                    Actual = x.Actual,
                    BudgetItemId = x.BudgetItemId,
                    POValueUSD = x.POValueUSD,
                    PurchaseorderItemId = x.PurchaseOrderId,
                    PurchaseorderName = x.PurchaseOrder == null ? string.Empty : x.PurchaseOrder.PurchaseorderName,
                    PurchaseorderNumber = x.PurchaseOrder == null ? string.Empty : x.PurchaseOrder.PONumber,
                    Supplier = x.PurchaseOrder == null ? string.Empty : x.PurchaseOrder.Supplier == null ? string.Empty : x.PurchaseOrder.Supplier.Name,
                    PurchaseOrderStatus = x.PurchaseOrder == null ? PurchaseOrderStatusEnum.None : PurchaseOrderStatusEnum.GetType(x.PurchaseOrder.PurchaseOrderStatus),

                }).ToList(),
            };


            MWOResponse mWOResponse = new()
            {
                Id = mwo.Id,
                Name = mwo.Name,
                CECName = $"CEC0000{mwo.MWONumber}",
                Capital = mwo.BudgetItems.Where(x => x.Type != BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget),
                Expenses = mwo.BudgetItems.Where(x => x.Type == BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget),
                CostCenter = CostCenterEnum.GetName(mwo.CostCenter),
                MWOType = MWOTypeEnum.GetType(mwo.Type),
                IsRealProductive = mwo.IsAssetProductive,



            };
            Func<BudgetItem, bool> Criteria = budgetItem.Type == BudgetItemTypeEnum.Alterations.Id ?
                x => x.Type == BudgetItemTypeEnum.Alterations.Id && x.Id != budgetItem.Id :
                x => (x.Type != BudgetItemTypeEnum.Alterations.Id &&
                x.Type != BudgetItemTypeEnum.Contingency.Id &&
                x.Type != BudgetItemTypeEnum.Taxes.Id &&
                x.Type != BudgetItemTypeEnum.Engineering.Id && x.Id != budgetItem.Id);
            var BudgetItems = mwo.BudgetItems.Where(Criteria).Select(x => new BudgetItemApprovedResponse()
            {
                Id = x.Id,
                Name = x.Name,
                Budget = x.Budget,
                Nomenclatore = $"{BudgetItemTypeEnum.GetLetter(x.Type)}{x.Order}",
                Type = BudgetItemTypeEnum.GetType(x.Type),
                PurchaseOrders = budgetItem.PurchaseOrderItems.Count == 0 ? new() :
                budgetItem.PurchaseOrderItems.Select(x => new PurchaseOrderItemForBudgetItemResponse()
                {
                    Actual = x.Actual,
                    BudgetItemId = x.BudgetItemId,
                    POValueUSD = x.POValueUSD,
                    PurchaseorderItemId = x.PurchaseOrderId,
                    PurchaseorderName = x.PurchaseOrder == null ? string.Empty : x.PurchaseOrder.PurchaseorderName,
                    PurchaseorderNumber = x.PurchaseOrder == null ? string.Empty : x.PurchaseOrder.PONumber,
                    Supplier = x.PurchaseOrder == null ? string.Empty : x.PurchaseOrder.Supplier == null ? string.Empty : x.PurchaseOrder.Supplier.Name,
                    PurchaseOrderStatus = x.PurchaseOrder == null ? PurchaseOrderStatusEnum.None : PurchaseOrderStatusEnum.GetType(x.PurchaseOrder.PurchaseOrderStatus),

                }).ToList(),
            }).OrderBy(x => x.Nomenclatore).ToList();
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
            DataForCreatePurchaseOrder result = new();
            result.MWO = mWOResponse;
            result.BudgetItem = budgetItemResponse;
            result.Suppliers = await suppliers.Select(expressionSupplier).ToListAsync();
            result.OriginalBudgetItems = BudgetItems;
            result.BudgetItems = BudgetItems;
            return Result<DataForCreatePurchaseOrder>.Success(result);
        }
    }
}
