using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
using System.Linq.Expressions;

namespace Application.Features.PurchaseOrders.Queries
{
    public record GetAllPurchaseOrderQuery : IRequest<IResult<PurchaseOrdersListResponse>>;

    public class GetPurchaseordersToApproveQueryHandler : IRequestHandler<GetAllPurchaseOrderQuery, IResult<PurchaseOrdersListResponse>>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;

        public GetPurchaseordersToApproveQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<PurchaseOrdersListResponse>> Handle(GetAllPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            var purchaseorder = await _purchaseOrderRepository.GetAllPurchaseorders();
            Expression<Func<PurchaseOrder, PurchaseOrderResponse>> expression = e => new PurchaseOrderResponse
            {
                PurchaseorderId = e.Id,
                MWOId = e.MWOId,
                PurchaseRequisition = e.PurchaseRequisition,
                PONumber = e.PONumber,
                TaxCode = e.TaxCode,
                ExpetedOn = e.POExpectedDateDate,
                VendorCode = e.Supplier == null ? string.Empty : e.Supplier.VendorCode,
                AccountAssigment = e.AccountAssigment,
                MWOCode = e.MWO == null ? string.Empty : $"CEC0000{e.MWO.MWONumber}",
                MWOName = e.MWO == null ? string.Empty : e.MWO.Name,
                CreatedBy = e.CreatedByUserName,
                CreatedOn = e.CreatedDate,
                PurchaseorderName = e.PurchaseorderName,
                QuoteNo = e.QuoteNo,
                Supplier = e.Supplier == null ? string.Empty : e.Supplier.Name,
                SupplierId = e.SupplierId,
                PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(e.PurchaseOrderStatus),
                PurchaseOrderItems = e.PurchaseOrderItems.Count == 0 ? new() :
                e.PurchaseOrderItems.Select(x => new PurchaseorderItemsResponse()
                {
                    PurchaseorderItemId=x.Id,
                    BudgetItemId = x.Id,
                    POValueUSD = x.POValueUSD,

                }).ToList(),

            };

            PurchaseOrdersListResponse orderToApproveResponse = new();

            orderToApproveResponse.Purchaseorders = purchaseorder.Select(expression).ToList();

            orderToApproveResponse.Purchaseorders= orderToApproveResponse.Purchaseorders
                .OrderBy(x => x.PurchaseOrderStatus.Id).ThenBy(x=>x.PurchaseRequisition).ToList();
            return Result<PurchaseOrdersListResponse>.Success(orderToApproveResponse);
        }
    }



}
