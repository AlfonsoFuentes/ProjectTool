using Application.Interfaces;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders;
using Shared.Models.PurchaseOrders.Requests.Approves;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Queries
{
    public record GetPurchaseOrderToApproveById(Guid PurchaseOrderId) : IRequest<IResult<ApprovePurchaseOrderRequest>>;

    internal class GetPurchaseOrderToApproveByIdHandler : IRequestHandler<GetPurchaseOrderToApproveById, IResult<ApprovePurchaseOrderRequest>>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;

        public GetPurchaseOrderToApproveByIdHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<ApprovePurchaseOrderRequest>> Handle(GetPurchaseOrderToApproveById request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderWithItemsAndSupplierById(request.PurchaseOrderId);
            if (purchaseOrder == null)
            {
                return Result<ApprovePurchaseOrderRequest>.Fail("Not found");
            }
            ApprovePurchaseOrderRequest result = new()
            {
                AccountAssigment = purchaseOrder.AccountAssigment,
                MWOId = purchaseOrder.MWOId,
                MWOName = purchaseOrder.MWO == null ? string.Empty : purchaseOrder.MWO.Name,
                PurchaseorderName = purchaseOrder.PurchaseorderName,
                PurchaseorderId = purchaseOrder.Id,
                PurchaseRequisition = purchaseOrder.PurchaseRequisition,
                QuoteNo = purchaseOrder.QuoteNo,
                MWOCode = purchaseOrder.MWO == null ? string.Empty : $"CEC0000{purchaseOrder.MWO.MWONumber}",
                ItemsInPurchaseorder = purchaseOrder.PurchaseOrderItems.Select(x => new PurchaseorderItemsToApproveRequest()
                {
                    BudgetItemId = x.BudgetItemId,
                    POValueUSD = x.POValueUSD,
                    BudgetItemName=x.BudgetItem.Name,

                }).ToList(),
                IsMWONoProductive = purchaseOrder.MWO == null ? false : !purchaseOrder.MWO.IsAssetProductive,
                Supplier = purchaseOrder.Supplier.Name,
                VendorCode = purchaseOrder.Supplier.VendorCode,
                TaxCode = purchaseOrder.TaxCode,
                SupplierId = purchaseOrder.SupplierId,
                CreatedBy = purchaseOrder.CreatedByUserName,
                CreatedOn = purchaseOrder.CreatedDate,
                ExpetedOn = purchaseOrder.PurchaseOrderStatus == PurchaseOrderStatusEnum.Approved.Id ? 
                            purchaseOrder.POExpectedDateDate : DateTime.UtcNow,
                PONumber = purchaseOrder.PurchaseOrderStatus == PurchaseOrderStatusEnum.Approved.Id ? 
                            purchaseOrder.PONumber : string.Empty,
                PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(purchaseOrder.PurchaseOrderStatus),
                PurchaseOrderValue = purchaseOrder.POValueUSD,
                IsAlteration = purchaseOrder.IsAlteration,
            };
            return Result<ApprovePurchaseOrderRequest>.Success(result);
        }
    }

}
