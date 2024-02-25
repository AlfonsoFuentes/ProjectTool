using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.Closeds;
using Shared.Models.PurchaseOrders.Requests.Receives;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Queries
{
    public record GetPurchaseOrderClosedById(Guid PurchaseOrderId) : IRequest<IResult<ClosedPurchaseOrderRequest>>;
    internal class GetPurchaseOrderClosedByIdHandler : IRequestHandler<GetPurchaseOrderClosedById, IResult<ClosedPurchaseOrderRequest>>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;

        public GetPurchaseOrderClosedByIdHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<ClosedPurchaseOrderRequest>> Handle(GetPurchaseOrderClosedById request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderWithItemsAndSupplierById(request.PurchaseOrderId);
            if (purchaseOrder == null)
            {
                return Result<ClosedPurchaseOrderRequest>.Fail("Not found");
            }
            ClosedPurchaseOrderRequest result = new()
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
                Supplier = purchaseOrder.Supplier == null ? new() : new()
                {
                    Id = purchaseOrder.Supplier.Id,
                    Name = purchaseOrder.Supplier.Name,
                },
                SupplierId = purchaseOrder.SupplierId,
                TaxCode = purchaseOrder.TaxCode,
                VendorCode = purchaseOrder.Supplier == null ? string.Empty : purchaseOrder.Supplier.VendorCode,
                QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.Currency),
                USDCOP = purchaseOrder.USDCOP,
                USDEUR = purchaseOrder.USDEUR,
                IsAlteration = purchaseOrder.IsAlteration,
                PercentageAlteration = purchaseOrder.MWO == null ? 0 : purchaseOrder.MWO.PercentageTaxForAlterations,
                


            };
            return Result<ClosedPurchaseOrderRequest>.Success(result);
        }
    }

}
