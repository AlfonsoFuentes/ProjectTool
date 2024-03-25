using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Commons.UserManagement;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
using System.Linq.Expressions;

namespace Application.Features.PurchaseOrders.Queries
{
    public record GetAllPurchaseOrderQuery : IRequest<IResult<PurchaseOrdersListResponse>>;

    public class GetPurchaseordersToApproveQueryHandler : IRequestHandler<GetAllPurchaseOrderQuery, IResult<PurchaseOrdersListResponse>>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;
        private CurrentUser CurrentUser { get; set; }
        public GetPurchaseordersToApproveQueryHandler(IPurchaseOrderRepository purchaseOrderRepository, CurrentUser currentUser)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            CurrentUser = currentUser;
        }

        public async Task<IResult<PurchaseOrdersListResponse>> Handle(GetAllPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<PurchaseOrder, bool>> filteruser = CurrentUser.IsSuperAdmin ? p => p.CreatedBy != null : p => p.CreatedBy == CurrentUser.UserId;
            var purchaseorder = await _purchaseOrderRepository.GetAllPurchaseorders();
            Expression<Func<PurchaseOrder, PurchaseOrderResponse>> expression = e => new PurchaseOrderResponse
            {
                PurchaseOrderId = e.Id,
                MWOId = e.MWOId,
                PurchaseRequisition = e.PurchaseRequisition,
                PONumber = e.PONumber,
                TaxCode = e.TaxCode,
                ExpetedOn = !e.POExpectedDateDate.HasValue ? string.Empty : e.POExpectedDateDate.Value.ToShortDateString(),
                VendorCode = e.Supplier == null ? string.Empty : e.Supplier.VendorCode,
                AccountAssigment = e.AccountAssigment,
                CECName = e.MWO == null ? string.Empty : $"CEC0000{e.MWO.MWONumber}",
                MWOName = e.MWO == null ? string.Empty : e.MWO.Name,
                CreatedBy = e.CreatedByUserName,
                CreatedOn = e.CreatedDate.ToShortDateString(),
                ReceivedOn = e.POClosedDate.HasValue ? e.POClosedDate.Value.ToShortDateString() : string.Empty,
                PurchaseorderName = e.PurchaseorderName,
                QuoteNo = e.QuoteNo,
                Supplier = e.Supplier == null ? string.Empty : e.Supplier.NickName,
                SupplierId = e.SupplierId,
                PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(e.PurchaseOrderStatus),
                PurchaseOrderItems = e.PurchaseOrderItems.Count == 0 ? new() :
                e.PurchaseOrderItems.Select(x => new PurchaseorderItemsResponse()
                {
                    PurchaseorderItemId = x.Id,
                    BudgetItemId = x.Id,
                    POValueUSD = x.POValueUSD,
                    Actual = Math.Round(x.Actual, 2)

                }).ToList(),

            };

            PurchaseOrdersListResponse orderToApproveResponse = new();

            orderToApproveResponse.Purchaseorders = purchaseorder.Where(filteruser).Select(expression).ToList();

            orderToApproveResponse.Purchaseorders = orderToApproveResponse.Purchaseorders
                .OrderBy(x => x.PurchaseOrderStatus.Id).ThenBy(x => x.PurchaseRequisition).ToList();
            return Result<PurchaseOrdersListResponse>.Success(orderToApproveResponse);
        }
    }



}
