using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Responses;
using Shared.Models.PurchaseorderStatus;
using Shared.Models.Suppliers;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Application.Features.PurchaseOrders.Queries
{
    public record GetAllNewPurchaseOrderQuery : IRequest<IResult<NewPurchaseOrdersListResponse>>;

    public class GetAllNewPurchaseOrderQueryHandler : IRequestHandler<GetAllNewPurchaseOrderQuery, IResult<NewPurchaseOrdersListResponse>>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;

        public GetAllNewPurchaseOrderQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;

        }

        public async Task<IResult<NewPurchaseOrdersListResponse>> Handle(GetAllNewPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            NewPurchaseOrdersListResponse response = new();
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

            return Result<NewPurchaseOrdersListResponse>.Success(response);
        }

        async Task<IEnumerable<NewPurchaseOrderCreatedResponse>> GetPurchaseOrderCreated()
        {
            var purchaseorder = await _purchaseOrderRepository.GetAllPurchaseordersCreated();

            return purchaseorder.Select(expressionCreated);
        }
        async Task<IEnumerable<NewPurchaseOrderApprovedResponse>> GetPurchaseOrderApproved()
        {
            var purchaseorder = await _purchaseOrderRepository.GetAllPurchaseordersToReceive();

            return purchaseorder.Select(expressionApproved);
        }
        async Task<IEnumerable<NewPurchaseOrderClosedResponse>> GetPurchaseOrderClosed()
        {
            var purchaseorder = await _purchaseOrderRepository.GetAllPurchaseordersClosed();

            return purchaseorder.Select(expressionClosed);
        }

        Expression<Func<PurchaseOrder, NewPurchaseOrderCreatedResponse>> expressionCreated = e => new NewPurchaseOrderCreatedResponse
        {
            PurchaseOrderId=e.Id,
            MWOId = e.MWOId,
            MainBudgetItemId = e.MainBudgetItemId,
            Supplier = e.Supplier == null ? null! : new SupplierResponse()
            {
                Id = e.Supplier.Id,
                Name = e.Supplier.Name,
                NickName = e.Supplier.NickName,

            },
            MWOName=e.MWO.Name,
            CECName=e.MWO.CECName,
            QuoteNo = e.QuoteNo,
            QuoteCurrency = CurrencyEnum.GetType(e.QuoteCurrency),
            PurchaseOrderCurrency = CurrencyEnum.GetType(e.Currency),
            PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(e.PurchaseOrderStatus),
            PurchaseRequisition = e.PurchaseRequisition,
            SPL = e.SPL,
            TaxCode = e.TaxCode,
            USDCOP = e.USDCOP,
            USDEUR = e.USDEUR,
            CreatedOn = e.CreatedDate.ToString("d"),
            CurrencyDate = e.CurrencyDate.ToString("d"),
            AccountAssigment = e.AccountAssigment,
            PurchaseorderName = e.PurchaseorderName,
            IsAlteration = e.IsAlteration,
            IsCapitalizedSalary = e.IsCapitalizedSalary,
            IsTaxEditable = e.IsTaxEditable,
            ActualUSD = e.ActualUSD,
            ApprovedUSD = e.ApprovedUSD,
            AssignedUSD = e.AssignedUSD,
            PendingToReceiveUSD = e.PendingToReceiveUSD,
            PotentialCommitmentUSD = e.PotentialCommitmentUSD,





        };
        Expression<Func<PurchaseOrder, NewPurchaseOrderApprovedResponse>> expressionApproved = e => new NewPurchaseOrderApprovedResponse
        {
            PurchaseOrderId = e.Id,
            MWOId = e.MWOId,
            MainBudgetItemId = e.MainBudgetItemId,
            Supplier = e.Supplier == null ? null! : new SupplierResponse()
            {
                Id = e.Supplier.Id,
                Name = e.Supplier.Name,
                NickName = e.Supplier.NickName,

            },
            CECName = e.MWO.CECName,
            QuoteNo = e.QuoteNo,
            QuoteCurrency = CurrencyEnum.GetType(e.QuoteCurrency),
            PurchaseOrderCurrency = CurrencyEnum.GetType(e.Currency),
            PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(e.PurchaseOrderStatus),
            PurchaseRequisition = e.PurchaseRequisition,
            PurchaseOrderNumber=e.PONumber,
            ExpectedOn=e.POExpectedDateDate==null?string.Empty:e.POExpectedDateDate.Value.ToString("d"),
            SPL = e.SPL,
            TaxCode = e.TaxCode,
            USDCOP = e.USDCOP,
            USDEUR = e.USDEUR,
            CreatedOn = e.CreatedDate.ToString("d"),
            CurrencyDate = e.CurrencyDate.ToString("d"),
            AccountAssigment = e.AccountAssigment,
            PurchaseorderName = e.PurchaseorderName,
            IsAlteration = e.IsAlteration,
            IsCapitalizedSalary = e.IsCapitalizedSalary,
            IsTaxEditable = e.IsTaxEditable,
            ActualUSD = e.ActualUSD,
            ApprovedUSD = e.ApprovedUSD,
           





        };
        Expression<Func<PurchaseOrder, NewPurchaseOrderClosedResponse>> expressionClosed = e => new NewPurchaseOrderClosedResponse
        {
            PurchaseOrderId = e.Id,
            MWOId = e.MWOId,
            MainBudgetItemId = e.MainBudgetItemId,
            Supplier = e.Supplier == null ? null! : new SupplierResponse()
            {
                Id = e.Supplier.Id,
                Name = e.Supplier.Name,
                NickName = e.Supplier.NickName,

            },
            CECName = e.MWO.CECName,
            QuoteNo = e.QuoteNo,
            QuoteCurrency = CurrencyEnum.GetType(e.QuoteCurrency),
            PurchaseOrderCurrency = CurrencyEnum.GetType(e.Currency),
            PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(e.PurchaseOrderStatus),
            PurchaseRequisition = e.PurchaseRequisition,
            PurchaseOrderNumber = e.PONumber,
            ClosedOn = e.POClosedDate== null ? string.Empty : e.POClosedDate.Value.ToString("d"),
            SPL = e.SPL,
            TaxCode = e.TaxCode,
            USDCOP = e.USDCOP,
            USDEUR = e.USDEUR,
            CreatedOn=e.CreatedDate.ToString("d"),
            
            CurrencyDate = e.CurrencyDate.ToString("d"),
            AccountAssigment = e.AccountAssigment,
            PurchaseorderName = e.PurchaseorderName,
            IsAlteration = e.IsAlteration,
            IsCapitalizedSalary = e.IsCapitalizedSalary,
            IsTaxEditable = e.IsTaxEditable,
          
            ApprovedUSD = e.ApprovedUSD,
        





        };
    }


}
