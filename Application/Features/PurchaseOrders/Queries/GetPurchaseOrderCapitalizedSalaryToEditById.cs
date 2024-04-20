using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
using Shared.Models.CostCenter;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.CapitalizedSalaries;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Queries
{
    public record GetPurchaseOrderCapitalizedSalaryToEditById(Guid PurchaseOrderId) : IRequest<IResult<EditCapitalizedSalaryPurchaseOrderRequest>>;

    internal class GetPurchaseOrderCapitalizedSalaryToEditByIdHandler : IRequestHandler<GetPurchaseOrderCapitalizedSalaryToEditById, IResult<EditCapitalizedSalaryPurchaseOrderRequest>>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;

        public GetPurchaseOrderCapitalizedSalaryToEditByIdHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<EditCapitalizedSalaryPurchaseOrderRequest>> Handle(GetPurchaseOrderCapitalizedSalaryToEditById request, CancellationToken cancellationToken)
        {
            PurchaseOrder purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderWithItemsAndSupplierById(request.PurchaseOrderId);
            var budgtitem = await _purchaseOrderRepository.GetBudgetItemWithMWOById(purchaseOrder.MainBudgetItemId);
            if (purchaseOrder == null)
            {
                return Result<EditCapitalizedSalaryPurchaseOrderRequest>.Fail("Not found");
            }
            EditCapitalizedSalaryPurchaseOrderRequest result = new()
            {

                MWOId = purchaseOrder.MWOId,
                CostCenter = CostCenterEnum.GetName(purchaseOrder.MWO.CostCenter),

                MWOCECName = $"CEC0000{purchaseOrder.MWO.MWONumber}",
                MWOName = purchaseOrder.MWO.Name,
                MainBudgetItem = new()
                {
                    Budget = budgtitem.Budget,
                    Brand = budgtitem.Brand == null ? string.Empty : budgtitem.Brand.Name,
                    BudgetItemId = budgtitem.Id,
                    Name = budgtitem.Name,
                    Type = BudgetItemTypeEnum.GetType(budgtitem.Type),
                    IsMainItemTaxesNoProductive = budgtitem.IsMainItemTaxesNoProductive,


                },
                PurchaseOrderName = purchaseOrder.PurchaseorderName,
                PurchaseOrderId = purchaseOrder.Id,
                PurchaseorderNumber = purchaseOrder.PONumber,
                PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.Currency),
                QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                IsCapitalizedSalary = purchaseOrder.PONumber == string.Empty,
                USDCOP = purchaseOrder.USDCOP,
                USDEUR = purchaseOrder.USDEUR,
                CurrencyDate = purchaseOrder.CurrencyDate,
                PurchaseOrderItem = purchaseOrder.PurchaseOrderItems.Select(x => new PurchaseOrderItemRequest()
                {
                    Budget = x.BudgetItem.Budget,
                    BudgetItemId = x.BudgetItemId,
                    PurchaseOrderItemId = x.Id,
                    ActualCurrency = x.ActualCurrency,

                    QuoteCurrency = CurrencyEnum.GetType(CurrencyEnum.USD.Id),
                    PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.Currency),
                    Name = x.Name,
                    Quantity = x.Quantity,
                    BudgetItemName = x.BudgetItem.Name,
                    TRMUSDCOP = purchaseOrder.USDCOP,
                    TRMUSDEUR = purchaseOrder.USDEUR,
                    QuoteCurrencyValue = x.UnitaryValueCurrency,
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
            return Result<EditCapitalizedSalaryPurchaseOrderRequest>.Success(result);
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
