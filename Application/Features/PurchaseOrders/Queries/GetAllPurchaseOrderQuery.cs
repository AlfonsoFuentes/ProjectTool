using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Commons.UserManagement;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
using System.Diagnostics;
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
            PurchaseOrdersListResponse response = new();
            Stopwatch sw = Stopwatch.StartNew();
            response.PurchaseordersCreated = await GetPurchaseOrderCreated();
            sw.Stop();
            var elapsed1 = sw.ElapsedMilliseconds;
            sw = Stopwatch.StartNew();

            response.PurchaseordersApproved = await GetPurchaseOrderApproved();
            sw.Stop();
            var elapsed2 = sw.ElapsedMilliseconds;
            sw = Stopwatch.StartNew();

            response.PurchaseordersClosed = await GetPurchaseOrderClosed();
            sw.Stop();
            var elapsed3 = sw.ElapsedMilliseconds;

            return Result<PurchaseOrdersListResponse>.Success(response);
        }

        async Task<IEnumerable<PurchaseOrderResponse>> GetPurchaseOrderCreated()
        {
            var purchaseorder = await _purchaseOrderRepository.GetAllPurchaseordersCreated(CurrentUser);

            return purchaseorder.Select(expression);
        }
        async Task<IEnumerable<PurchaseOrderResponse>> GetPurchaseOrderApproved()
        {
            var purchaseorder = await _purchaseOrderRepository.GetAllPurchaseordersToReceive(CurrentUser);

            return purchaseorder.Select(expression);
        }
        async Task<IEnumerable<PurchaseOrderResponse>> GetPurchaseOrderClosed()
        {
            var purchaseorder = await _purchaseOrderRepository.GetAllPurchaseordersClosed(CurrentUser);

            return purchaseorder.Select(expression);
        }
        Expression<Func<PurchaseOrder, PurchaseOrderResponse>> expression = e => new PurchaseOrderResponse
        {
            PurchaseOrderId = e.Id,
            MWOId = e.MWOId,
            PurchaseRequisition = e.PurchaseRequisition,
            PONumber = e.PONumber,
            TaxCode = e.TaxCode,
            ExpectedOn = !e.POExpectedDateDate.HasValue ? string.Empty : e.POExpectedDateDate.Value.ToShortDateString(),
            VendorCode = e.Supplier == null ? string.Empty : e.Supplier.VendorCode,
            AccountAssigment = e.AccountAssigment,
            CECName = e.MWO == null ? string.Empty : $"CEC0000{e.MWO.MWONumber}",
            MWOName = e.MWO == null ? string.Empty : e.MWO.Name,
            CreatedBy = e.CreatedByUserName,
            CreatedOn = e.CreatedDate.ToShortDateString(),
            IsAlteration = e.IsAlteration,
            IsCapitalizedSalary = e.IsCapitalizedSalary,
            IsTaxEditable = e.IsTaxEditable,
            IsTaxNoProductive=!e.IsTaxEditable,
            ReceivedOn = e.POClosedDate.HasValue ? e.POClosedDate.Value.ToShortDateString() : string.Empty,
            PurchaseorderName = e.PurchaseorderName,
            QuoteNo = e.QuoteNo,
            SupplierNickName = e.Supplier == null ? string.Empty : e.Supplier.NickName,
            SupplierName = e.Supplier == null ? string.Empty : e.Supplier.Name,
            SupplierId = e.SupplierId,
            PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(e.PurchaseOrderStatus),

            PurchaseOrderItems = e.PurchaseOrderItems.Count == 0 ? new() :
                e.PurchaseOrderItems.Select(x => new PurchaseorderItemsResponse()
                {
                    PurchaseorderItemId = x.Id,
                    BudgetItemId = x.Id,
                    UnitaryValueCurrency = x.UnitaryValueCurrency,
                    Quantity = x.Quantity,

                    ActualCurrency = x.ActualCurrency,
                    Currency = CurrencyEnum.GetType(e.Currency),
                    USDCOP = e.USDCOP,
                    USDEUR = e.USDEUR,


                }).ToList(),

        };
    }



}
