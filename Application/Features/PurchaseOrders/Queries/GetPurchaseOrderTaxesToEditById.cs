using Shared.Enums.BudgetItemTypes;
using Shared.Enums.CostCenter;
using Shared.Enums.Currencies;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseOrders.Requests.Taxes;

namespace Application.Features.PurchaseOrders.Queries
{
    public record GetPurchaseOrderTaxesToEditById(Guid PurchaseOrderId) : IRequest<IResult<EditTaxPurchaseOrderRequest>>;
    internal class GetPurchaseOrderTaxesToEditByIdHandler : IRequestHandler<GetPurchaseOrderTaxesToEditById, IResult<EditTaxPurchaseOrderRequest>>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;

        public GetPurchaseOrderTaxesToEditByIdHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<EditTaxPurchaseOrderRequest>> Handle(GetPurchaseOrderTaxesToEditById request, CancellationToken cancellationToken)
        {
            PurchaseOrder purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderWithItemsAndSupplierById(request.PurchaseOrderId);
            var budgtitem = await _purchaseOrderRepository.GetBudgetItemWithMWOById(purchaseOrder.MainBudgetItemId);
            if (purchaseOrder == null)
            {
                return Result<EditTaxPurchaseOrderRequest>.Fail("Not found");
            }
            EditTaxPurchaseOrderRequest result = new()
            {

                MWO = new()
                {
                    Id = purchaseOrder.MWOId,
                    Name = purchaseOrder.MWO.Name,
                    CECName = $"CEC0000{purchaseOrder.MWO.MWONumber}",
                    CostCenter = CostCenterEnum.GetName(purchaseOrder.MWO.CostCenter),

                },
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
                PurchaseorderId = purchaseOrder.Id,

                PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.PurchaseOrderCurrency),
                PONumber = purchaseOrder.PONumber,
                USDCOP = purchaseOrder.USDCOP,
                USDEUR = purchaseOrder.USDEUR,
                CurrencyDate = purchaseOrder.CurrencyDate,
                PurchaseOrderItem = purchaseOrder.PurchaseOrderItems.Select(x => new PurchaseOrderItemRequest()
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
                    AssignedUSD = x.AssignedUSD,
                    PotencialUSD = x.PotentialCommitmentUSD,
                    ActualUSD = x.ActualUSD,
                    OriginalAssignedCurrency = x.AssignedUSD,
                    OriginalPotencialCurrency = x.PotentialCommitmentUSD,

                }).FirstOrDefault()!,






            };
            return Result<EditTaxPurchaseOrderRequest>.Success(result);
        }
       
    }
}
