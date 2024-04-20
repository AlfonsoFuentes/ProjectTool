using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
using Shared.Models.CostCenter;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Creates;
using Shared.Models.PurchaseOrders.Requests.Taxes;
using Shared.Models.PurchaseorderStatus;

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
                    Budget = budgtitem.Budget,
                    Brand = budgtitem.Brand == null ? string.Empty : budgtitem.Brand.Name,
                    BudgetItemId = budgtitem.Id,
                    Name = budgtitem.Name,
                    Type = BudgetItemTypeEnum.GetType(budgtitem.Type),
                    IsMainItemTaxesNoProductive = budgtitem.IsMainItemTaxesNoProductive,


                },
                PurchaseorderName = purchaseOrder.PurchaseorderName,
                PurchaseorderId = purchaseOrder.Id,

                PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.Currency),
                PONumber = purchaseOrder.PONumber,
                USDCOP = purchaseOrder.USDCOP,
                USDEUR = purchaseOrder.USDEUR,
                CurrencyDate = purchaseOrder.CurrencyDate,
                PurchaseOrderItem = purchaseOrder.PurchaseOrderItems.Select(x => new PurchaseOrderItemRequest()
                {
                    Budget = x.BudgetItem.Budget,
                    BudgetItemId = x.BudgetItemId,
                    PurchaseOrderItemId = x.Id,
                    Name = x.Name,
                    Quantity = x.Quantity,
                    BudgetItemName = x.BudgetItem.Name,
                    QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                    PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.Currency),
                    QuoteCurrencyValue = GetQuoteCurrencyValue(x.UnitaryValueCurrency, purchaseOrder.QuoteCurrency, purchaseOrder.Currency,
                    purchaseOrder.USDCOP, purchaseOrder.USDEUR),
                    ActualCurrency =x.ActualCurrency,
               
                    TRMUSDCOP = purchaseOrder.USDCOP,
                    TRMUSDEUR = purchaseOrder.USDEUR,
                    AssignedCurrency = x.BudgetItem.PurchaseOrderItems.Where(x =>
                    x.PurchaseOrder.PurchaseOrderStatus != PurchaseOrderStatusEnum.Created.Id).Sum(x => x.UnitaryValueCurrency * x.Quantity),

                    PotencialCurrency = x.BudgetItem.PurchaseOrderItems.Where(x =>
                    x.PurchaseOrder.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.UnitaryValueCurrency * x.Quantity),


                    OriginalAssignedCurrency = x.BudgetItem.PurchaseOrderItems.Where(x =>
                    x.PurchaseOrder.PurchaseOrderStatus != PurchaseOrderStatusEnum.Created.Id).Sum(x => x.UnitaryValueCurrency * x.Quantity),
                    OriginalPotencialCurrency = x.BudgetItem.PurchaseOrderItems.Where(x =>
                    x.PurchaseOrder.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.UnitaryValueCurrency * x.Quantity),

                }).FirstOrDefault()!,






            };
            return Result<EditTaxPurchaseOrderRequest>.Success(result);
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
