using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Enums.Currencies;
using Shared.Enums.PurchaseorderStatus;
using Shared.Models.PurchaseOrders.Responses;
using System.Diagnostics;
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
            var purchaseorder = await _purchaseOrderRepository.GetAllPurchaseordersCreated();

            return purchaseorder.Select(expression);
        }
        async Task<IEnumerable<PurchaseOrderResponse>> GetPurchaseOrderApproved()
        {
            var purchaseorder = await _purchaseOrderRepository.GetAllPurchaseordersToReceive();

            return purchaseorder.Select(expression);
        }
        async Task<IEnumerable<PurchaseOrderResponse>> GetPurchaseOrderClosed()
        {
            var purchaseorder = await _purchaseOrderRepository.GetAllPurchaseordersClosed();

            return purchaseorder.Select(expression);
        }
       static double GetQuoteCurrencyValue(double UnitaryValue, int quotecurrency, int purchaseOrdercurrency, double TRMUSDCOP, double TRMUSDEUR)
        {
            var result =
                  purchaseOrdercurrency == CurrencyEnum.USD.Id && quotecurrency == CurrencyEnum.USD.Id ? UnitaryValue :
                  purchaseOrdercurrency == CurrencyEnum.USD.Id && quotecurrency == CurrencyEnum.COP.Id ? UnitaryValue * TRMUSDCOP :
                  purchaseOrdercurrency == CurrencyEnum.USD.Id && quotecurrency == CurrencyEnum.EUR.Id ? UnitaryValue * TRMUSDEUR :

                  purchaseOrdercurrency == CurrencyEnum.COP.Id && quotecurrency == CurrencyEnum.USD.Id ? UnitaryValue / TRMUSDCOP :
                  purchaseOrdercurrency == CurrencyEnum.COP.Id && quotecurrency == CurrencyEnum.COP.Id ? UnitaryValue :
                  purchaseOrdercurrency == CurrencyEnum.COP.Id && quotecurrency == CurrencyEnum.EUR.Id ? UnitaryValue / TRMUSDCOP / TRMUSDEUR :

                  purchaseOrdercurrency == CurrencyEnum.EUR.Id && quotecurrency == CurrencyEnum.USD.Id ? UnitaryValue / TRMUSDEUR :
                  purchaseOrdercurrency == CurrencyEnum.EUR.Id && quotecurrency == CurrencyEnum.COP.Id ? UnitaryValue * TRMUSDCOP / TRMUSDEUR :
                  purchaseOrdercurrency == CurrencyEnum.EUR.Id && quotecurrency == CurrencyEnum.EUR.Id ? UnitaryValue : 0;

            return result;
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
                    QuoteUnitaryValueCurrency = GetQuoteCurrencyValue(x.UnitaryValueCurrency, e.QuoteCurrency, e.PurchaseOrderCurrency,
                    e.USDCOP, e.USDEUR),
                    Quantity = x.Quantity,
                    PurchaseOrderCurrency= CurrencyEnum.GetType(e.PurchaseOrderCurrency),
                    QuoteCurrency = CurrencyEnum.GetType(e.QuoteCurrency),
                    ActualCurrency = x.POItemActualCurrency,
                   
                    USDCOP = e.USDCOP,
                    USDEUR = e.USDEUR,


                }).ToList(),

        };
       
    }




}
