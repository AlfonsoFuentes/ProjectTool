using Shared.Enums.BudgetItemTypes;
using Shared.Enums.CostCenter;
using Shared.Enums.Currencies;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits;

namespace Application.Features.PurchaseOrders.Queries
{
    public record GetPurchaseOrderApprovedToEditById(Guid PurchaseOrderId) : IRequest<IResult<EditPurchaseOrderRegularApprovedRequest>>;
    internal class GetPurchaseOrderApprovedToEditByIdHandler : IRequestHandler<GetPurchaseOrderApprovedToEditById, IResult<EditPurchaseOrderRegularApprovedRequest>>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;

        public GetPurchaseOrderApprovedToEditByIdHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<EditPurchaseOrderRegularApprovedRequest>> Handle(GetPurchaseOrderApprovedToEditById request, CancellationToken cancellationToken)
        {
            PurchaseOrder purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderWithItemsAndSupplierById(request.PurchaseOrderId);
            var budgtitem = await _purchaseOrderRepository.GetBudgetItemWithMWOById(purchaseOrder.MainBudgetItemId);
            if (purchaseOrder == null)
            {
                return Result<EditPurchaseOrderRegularApprovedRequest>.Fail("Not found");
            }
            EditPurchaseOrderRegularApprovedRequest result = new()
            {

                MWOId = purchaseOrder.MWOId,
                CostCenter = CostCenterEnum.GetName(purchaseOrder.MWO.CostCenter),
                IsAssetProductive = purchaseOrder.MWO.IsAssetProductive,
                MWOCECName = $"CEC0000{purchaseOrder.MWO.MWONumber}",
                MWOName = purchaseOrder.MWO.Name,
                ExpectedDate = purchaseOrder.POExpectedDateDate,
                PONumber = purchaseOrder.PONumber,

                MainBudgetItem = new()
                {
                    BudgetUSD = budgtitem.Budget,
                    Brand = budgtitem.Brand == null ? string.Empty : budgtitem.Brand.Name,
                    BudgetItemId = budgtitem.Id,
                    Name = budgtitem.Name,
                    Type = BudgetItemTypeEnum.GetType(budgtitem.Type),
                    IsMainItemTaxesNoProductive = budgtitem.IsMainItemTaxesNoProductive,


                },
                PurchaseorderName = purchaseOrder.PurchaseorderName,
                PurchaseOrderId = purchaseOrder.Id,
                PurchaseRequisition = purchaseOrder.PurchaseRequisition,
                QuoteNo = purchaseOrder.QuoteNo,
                PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.PurchaseOrderCurrency),
                QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                USDCOP = purchaseOrder.USDCOP,
                USDEUR = purchaseOrder.USDEUR,
                CurrencyDate = purchaseOrder.CurrencyDate,
                PurchaseOrderItems = purchaseOrder.PurchaseOrderItems.Where(x => !x.IsTaxAlteration).Select(x => new PurchaseOrderItemRequest()
                {

                    BudgetItemId = x.BudgetItemId,
                    PurchaseOrderItemId = x.Id,
                    Name = x.Name,
                    Quantity = x.Quantity,
                    BudgetItemName = x.BudgetItem.Name,
                    TRMUSDCOP = purchaseOrder.USDCOP,
                    TRMUSDEUR = purchaseOrder.USDEUR,
                    QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                    PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.PurchaseOrderCurrency),
                    QuoteCurrencyValue = x.UnitaryValueCurrency,
                    BudgetUSD = x.BudgetItem.Budget,
                    AssignedUSD = x.POItemValueUSD,
                    PotencialUSD = x.PotentialCommitmentUSD,
                    ActualUSD = x.ActualUSD,
                    OriginalAssignedCurrency = x.POItemValueUSD,
                    OriginalPotencialCurrency = x.PotentialCommitmentUSD,

                }).ToList(),

                Supplier = purchaseOrder.Supplier == null ? new() : new()
                {
                    SupplierId= purchaseOrder.Supplier.Id,
                    Name = purchaseOrder.Supplier.Name,
                    VendorCode = purchaseOrder.Supplier.VendorCode,
                    TaxCodeLD = purchaseOrder.Supplier.TaxCodeLD,
                    TaxCodeLP = purchaseOrder.Supplier.TaxCodeLP,
                    NickName = purchaseOrder.Supplier.NickName,

                },



            };
            return Result<EditPurchaseOrderRegularApprovedRequest>.Success(result);
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
