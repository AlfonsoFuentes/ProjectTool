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
                    CurrencyUnitaryValue = (purchaseOrder.Currency == CurrencyEnum.USD.Id ? x.POValueUSD :
                    purchaseOrder.Currency == CurrencyEnum.COP.Id ? x.POValueUSD * purchaseOrder.USDCOP :
                    x.POValueUSD * purchaseOrder.USDEUR) / x.Quantity,

                }).FirstOrDefault()!,

               




            };
            return Result<EditTaxPurchaseOrderRequest>.Success(result);
        }
    }
}
