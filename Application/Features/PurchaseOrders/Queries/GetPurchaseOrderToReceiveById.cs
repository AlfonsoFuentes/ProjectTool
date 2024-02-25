using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.Approves;
using Shared.Models.PurchaseOrders.Requests.Receives;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Queries
{
    public record GetPurchaseOrderToReceiveById(Guid PurchaseOrderId) : IRequest<IResult<ReceivePurchaseOrderRequest>>;

    internal class GetPurchaseOrderToReceiveByIdHandler : IRequestHandler<GetPurchaseOrderToReceiveById, IResult<ReceivePurchaseOrderRequest>>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;

        public GetPurchaseOrderToReceiveByIdHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<ReceivePurchaseOrderRequest>> Handle(GetPurchaseOrderToReceiveById request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderWithItemsAndSupplierById(request.PurchaseOrderId);
            if (purchaseOrder == null)
            {
                return Result<ReceivePurchaseOrderRequest>.Fail("Not found");
            }
            ReceivePurchaseOrderRequest result = new()
            {
                PurchaseorderId = purchaseOrder.Id,
                AccountAssigment = purchaseOrder.AccountAssigment,
                CreatedBy = purchaseOrder.CreatedBy,
                CreatedOn = purchaseOrder.CreatedDate,
                ExpetedOn = purchaseOrder.POExpectedDateDate,
                MWOCode = purchaseOrder.MWO == null ? string.Empty : $"CEC0000{purchaseOrder.MWO.MWONumber}",
                MWOId = purchaseOrder.MWOId,
                MWOName = purchaseOrder.MWO == null ? string.Empty : purchaseOrder.MWO.Name,
                IsNoAssetProductive = purchaseOrder.MWO == null ? false : !purchaseOrder.MWO.IsAssetProductive,
                PONumber = purchaseOrder.PONumber,
                PurchaseorderName = purchaseOrder.PurchaseorderName,
                PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(purchaseOrder.PurchaseOrderStatus),
                PurchaseOrderValue = purchaseOrder.POValueUSD,
                PurchaseRequisition = purchaseOrder.PurchaseRequisition,
                QuoteNo = purchaseOrder.QuoteNo,
                Supplier = purchaseOrder.Supplier == null ? string.Empty : purchaseOrder.Supplier.Name,
                SupplierId = purchaseOrder.SupplierId,
                TaxCode = purchaseOrder.TaxCode,
                VendorCode = purchaseOrder.Supplier == null ? string.Empty : purchaseOrder.Supplier.VendorCode,
                Currency = CurrencyEnum.GetType(purchaseOrder.Currency),
                USDCOP = purchaseOrder.USDCOP,
                USDEUR = purchaseOrder.USDEUR,
                IsAlteration = purchaseOrder.IsAlteration,
                PercentageAlteration = purchaseOrder.MWO == null ? 0 : purchaseOrder.MWO.PercentageTaxForAlterations,
                ItemsInPurchaseorder = purchaseOrder.PurchaseOrderItems
                .Where(x => x.BudgetItem.Type != BudgetItemTypeEnum.Taxes.Id && x.IsAlteration == false).Select(x => new ReceivePurchaseorderItemRequest()
                {
                    PurchaseOrderItemName = x.Name,
                    PurchaseOrderItemId = x.Id,
                    BudgetItemId = x.BudgetItemId,
                    POValueUSD = x.POValueUSD,
                    ActualUSD = x.Actual,
                    OriginalActualUSD = x.Actual,
                    OriginalPendingUSD = x.POValueUSD - x.Actual,
                    PendingUSD = x.POValueUSD - x.Actual,
                    Currency = CurrencyEnum.GetType(purchaseOrder.Currency),
                    USDCOP = purchaseOrder.USDCOP,
                    USDEUR = purchaseOrder.USDEUR,

                }).ToList(),


            };
            return Result<ReceivePurchaseOrderRequest>.Success(result);
        }
    }

}
