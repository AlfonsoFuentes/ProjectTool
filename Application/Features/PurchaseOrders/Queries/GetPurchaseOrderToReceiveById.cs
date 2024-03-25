using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
using Shared.Models.CostCenter;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Queries
{

    public record GetPurchaseOrderToReceiveById(Guid PurchaseOrderId) : IRequest<IResult<ReceiveRegularPurchaseOrderRequest>>;

    internal class GetPurchaseOrderToReceiveByIdHandler : IRequestHandler<GetPurchaseOrderToReceiveById, IResult<ReceiveRegularPurchaseOrderRequest>>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;

        public GetPurchaseOrderToReceiveByIdHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<ReceiveRegularPurchaseOrderRequest>> Handle(GetPurchaseOrderToReceiveById request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderWithItemsAndSupplierById(request.PurchaseOrderId);
            var budgtitem = await _purchaseOrderRepository.GetBudgetItemWithMWOById(purchaseOrder.MainBudgetItemId);
            if (purchaseOrder == null)
            {
                return Result<ReceiveRegularPurchaseOrderRequest>.Fail("Not found");
            }
            ReceiveRegularPurchaseOrderRequest result = new()
            {
                MWOId = purchaseOrder.MWOId,
                CostCenter = CostCenterEnum.GetName(purchaseOrder.MWO.CostCenter),
                IsAssetProductive = purchaseOrder.MWO.IsAssetProductive,
                MWOCECName = $"CEC0000{purchaseOrder.MWO.MWONumber}",
                MWOName = purchaseOrder.MWO.Name,
                PercentageAlteration = purchaseOrder.MWO.PercentageTaxForAlterations,
                PONumber = purchaseOrder.PONumber,
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

                TRMUSDCOP = purchaseOrder.USDCOP,
                TRMUSDEUR = purchaseOrder.USDEUR,
                CurrencyDate = purchaseOrder.CurrencyDate,
                Supplier = purchaseOrder.Supplier == null ? new() : new()
                {
                    Id = purchaseOrder.Supplier.Id,
                    Name = purchaseOrder.Supplier.Name,
                    VendorCode = purchaseOrder.Supplier.VendorCode,
                    TaxCodeLD = purchaseOrder.Supplier.TaxCodeLD,
                    TaxCodeLP = purchaseOrder.Supplier.TaxCodeLP,
                    NickName = purchaseOrder.Supplier.NickName,

                },
                PurchaseOrderItemsToReceive = purchaseOrder.PurchaseOrderItems
                .Where(x => x.BudgetItem.Type != BudgetItemTypeEnum.Taxes.Id && x.IsTaxAlteration == false).Select(x => new ReceivePurchaseorderItemRequest()
                {

                    BudgetItemId = x.BudgetItemId,
                    PurchaseOrderItemId = x.Id,
                    Name = x.Name,
                    BudgetItemName = x.BudgetItem.Name,
                    PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.Currency),
                    POValueUSD = x.POValueUSD,
                    POActualUSD = x.Actual,

                    TRMUSDCOP = purchaseOrder.USDCOP,
                    TRMUSDEUR = purchaseOrder.USDEUR,


                }).ToList(),


            };
            return Result<ReceiveRegularPurchaseOrderRequest>.Success(result);
        }
    }

}
