using Shared.Enums.BudgetItemTypes;
using Shared.Enums.CostCenter;
using Shared.Enums.Currencies;
using Shared.Enums.MWOStatus;
using Shared.Enums.MWOTypes;
using Shared.Enums.PurchaseorderStatus;
using Shared.Models.MWO;
using System.Linq.Expressions;

namespace Application.Features.MWOs.Queries
{
    public record GetMWOApprovedById(Guid MWOId) : IRequest<IResult<MWOApprovedResponse>>;
    public class GetMWOApprovedByIdHandler : IRequestHandler<GetMWOApprovedById, IResult<MWOApprovedResponse>>
    {
        private IMWORepository Repository { get; set; }

        public GetMWOApprovedByIdHandler(IMWORepository repository)
        {
            Repository = repository;

        }


        public async Task<IResult<MWOApprovedResponse>> Handle(GetMWOApprovedById request, CancellationToken cancellationToken)
        {

            var mwo = await Repository.GetMWOById(request.MWOId);
            if (mwo == null) Result<MWOApprovedResponse>.Fail("MWO Not found");
            MWOApprovedResponse mworesponse = new MWOApprovedResponse()
            {
                Id = mwo!.Id,
                Name = mwo.Name,
                CECName = $"CEC0000{mwo.MWONumber}",
                CostCenter = CostCenterEnum.GetName(mwo.CostCenter),
                CreatedBy = mwo!.TenantId,
                IsAssetProductive = mwo.IsAssetProductive,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo!.PercentageContingency,
                PercentageEngineering = mwo!.PercentageCapitalizedSalary,
                PercentageTaxForAlterations = mwo!.PercentageTaxForAlterations,
                MWOStatus = MWOStatusEnum.GetType(mwo.Status),
                MWOType = MWOTypeEnum.GetType(mwo.Type),


            };

            var purchaseorders = await Repository.GetPurchaseOrdersByMWOId(request.MWOId);
            var pororderlist = purchaseorders.ToList();
            Expression<Func<PurchaseOrder, PurchaseOrderResponse>> expressionpurchaseorder = e => new PurchaseOrderResponse
            {
                MWOId = e.MWOId,
                MWOName = mworesponse.Name,
                PONumber = e.PONumber,
                AccountAssigment = e.AccountAssigment,
                CreatedBy = e.TenantId,
                CreatedOn = e.CreatedDate.ToShortDateString(),
                ExpectedOn = e.POExpectedDateDate == null ? string.Empty : e.POExpectedDateDate!.Value.ToShortDateString(),
                IsAlteration = e.IsAlteration,
                IsCapitalizedSalary = e.IsCapitalizedSalary,
                IsTaxEditable = e.IsTaxEditable,
                PurchaseOrderId = e.Id,
                PurchaseorderName = e.PurchaseorderName,
                PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(e.PurchaseOrderStatus),
                PurchaseRequisition = e.PurchaseRequisition,
                QuoteNo = e.QuoteNo,
                SupplierNickName = e.Supplier == null ? string.Empty : e.Supplier.NickName,
                SupplierName = e.Supplier == null ? string.Empty : e.Supplier.Name,
                TaxCode = e.TaxCode,
                VendorCode = e.Supplier == null ? string.Empty : e.Supplier.VendorCode,
                SupplierId = e.SupplierId == null ? Guid.Empty : e.SupplierId.Value,
                
                PurchaseOrderItems = e.PurchaseOrderItems.Select(x => new PurchaseorderItemsResponse
                {
                    BudgetItemId = x.BudgetItemId,
                    ActualCurrency = x.POItemActualCurrency,
                    QuoteUnitaryValueCurrency = x.UnitaryValueCurrency, 
                    Quantity = x.Quantity,
                    PurchaseOrderCurrency = CurrencyEnum.GetType(e.PurchaseOrderCurrency),
                    QuoteCurrency = CurrencyEnum.GetType(e.QuoteCurrency),
                   
                    PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(e.PurchaseOrderStatus),
                    USDCOP = e.USDCOP,
                    USDEUR = e.USDEUR,
               
                }
               ).ToList(),
            };

            var budgetitems = await Repository.GetBudgetItemsByMWOId(request.MWOId);
            Expression<Func<BudgetItem, BudgetItemApprovedResponse>> expression = e => new BudgetItemApprovedResponse
            {

                BudgetItemId = e.Id,
                Name = e.Name,
                Order = e.Order,
                Type = BudgetItemTypeEnum.GetType(e.Type),
                MWOId = e.MWOId,
                MWOCECName = mworesponse.CECName,
                CostCenter = mworesponse.CostCenter,
                MWOName = mworesponse.Name,
                BudgetUSD = e.Budget,
                CreatedBy = e.CreatedByUserName,
                CreatedOn = e.CreatedDate.ToString(),
                IsMainItemTaxesNoProductive = e.IsMainItemTaxesNoProductive,
                Quantity = e.Quantity,

                Percentage = e.Percentage,
                Brand = e.Brand == null ? string.Empty : e.Brand.Name,


            };
            var budgetitemresponse = budgetitems.Select(expression).ToList();
            //mworesponse.BudgetItems = budgetitemresponse.OrderBy(x => x.Nomenclatore).ToList();
            //mworesponse.BudgetItems.ForEach(budgetitem =>
            //{
            //    budgetitem.PurchaseOrders = mworesponse.PurchaseOrders.Where(x => x.PurchaseOrderItems.Any(x => x.BudgetItemId == budgetitem.BudgetItemId)).ToList();
            //});

            return Result<MWOApprovedResponse>.Success(mworesponse);
        }
        double GetQuoteCurrencyValue(double UnitaryValue, int quotecurrency, int purchaseOrdercurrency, double TRMUSDCOP, double TRMUSDEUR)
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
    }

}
