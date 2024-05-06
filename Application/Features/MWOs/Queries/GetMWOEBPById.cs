using Shared.Enums.Currencies;
using Shared.Enums.PurchaseorderStatus;
using Shared.Models.MWO;
using System.Linq.Expressions;

namespace Application.Features.MWOs.Queries
{
    public record GetMWOEBPById(Guid MWOId) : IRequest<IResult<MWOEBPResponse>>;
    public class GetMWOEBPByIdHandler : IRequestHandler<GetMWOEBPById, IResult<MWOEBPResponse>>
    {
        private IMWORepository Repository { get; set; }

        public GetMWOEBPByIdHandler(IMWORepository repository)
        {
            Repository = repository;

        }


        public async Task<IResult<MWOEBPResponse>> Handle(GetMWOEBPById request, CancellationToken cancellationToken)
        {

            var mwo = await Repository.GetMWOById(request.MWOId);
            if (mwo == null) Result<MWOEBPResponse>.Fail("MWO Not found");
            MWOEBPResponse mworesponse = new MWOEBPResponse()
            {
                MWOId = mwo!.Id,
                MWOName = mwo.Name,
                CECName = $"CEC0000{mwo.MWONumber}",



            };

            var purchaseorders = await Repository.GetPurchaseOrdersByMWOId(request.MWOId);

            Expression<Func<PurchaseOrder, NewPurchaseOrderResponse>> expressionpurchaseorder = e => new NewPurchaseOrderResponse
            {
                PurchaseOrderId = e.Id,
                PurchaseOrderNumber = e.PONumber,
                DateExpectedOn = e.POExpectedDateDate,
                DateClosedOn = e.POClosedDate,
                DateApprovedOn = e.POApprovedDate,
                MWOId = e.MWOId,
                MainBudgetItemId = e.MainBudgetItemId,
                IsTaxEditable = e.IsTaxEditable,
                AccountAssigment = e.AccountAssigment,
                PurchaseOrderCurrency = CurrencyEnum.GetType(e.PurchaseOrderCurrency),
                CurrencyDate = e.CurrencyDate.ToString("d"),
                IsAlteration = e.IsAlteration,
                IsCapitalizedSalary = e.IsCapitalizedSalary,
                PurchaseorderName = e.PurchaseorderName,
                PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(e.PurchaseOrderStatus),
                PurchaseRequisition = e.PurchaseRequisition,
                QuoteCurrency = CurrencyEnum.GetType(e.QuoteCurrency),
                QuoteNo = e.QuoteNo,
                SPL = e.SPL,
                Supplier = e.Supplier == null ? new() : new NewSupplierResponse()
                {
                    SupplierId = e.Supplier.Id,
                    Name = e.Supplier.Name,
                    NickName = e.Supplier.NickName,
                    VendorCode = e.Supplier.VendorCode,
                    TaxCodeLD = e.Supplier.TaxCodeLD,
                    TaxCodeLP = e.Supplier.TaxCodeLP,
                },
                TaxCode = e.TaxCode,
                USDCOP = e.USDCOP,
                USDEUR = e.USDEUR,
                ActualUSD = e.ActualUSD,
                ApprovedUSD = e.ApprovedUSD,
                MWOName = e.MWO.Name,
                CECName=e.MWO.CECName,
                AssignedUSD=e.AssignedUSD,
                 

            };
            var purchaseorderdto = purchaseorders.Select(expressionpurchaseorder).ToList();
            mworesponse.Potential = purchaseorderdto.Where(x => x.PurchaseOrderStatus.Id == PurchaseOrderStatusEnum.Created.Id).ToList();
            mworesponse.Commitment = purchaseorderdto.Where(x => x.CommitmentUSD > 0).ToList();
            mworesponse.Actual = purchaseorderdto.Where(x => x.ActualUSD > 0).ToList();
            return Result<MWOEBPResponse>.Success(mworesponse);
        }

    }

}
