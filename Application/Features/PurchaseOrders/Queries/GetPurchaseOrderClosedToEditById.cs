using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.BudgetItemTypes;
using Shared.Models.CostCenter;
using Shared.Models.Currencies;
using Shared.Models.PurchaseOrders.Requests.PurchaseOrderItems;
using Shared.Models.PurchaseOrders.Requests.RegularPurchaseOrders.Edits;
using Shared.Models.PurchaseorderStatus;

namespace Application.Features.PurchaseOrders.Queries
{
    public record GetPurchaseOrderClosedToEditById(Guid PurchaseOrderId) : IRequest<IResult<EditPurchaseOrderRegularClosedRequest>>;
    internal class GetPurchaseOrderClosedToEditByIdHandler : IRequestHandler<GetPurchaseOrderClosedToEditById, IResult<EditPurchaseOrderRegularClosedRequest>>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;

        public GetPurchaseOrderClosedToEditByIdHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<EditPurchaseOrderRegularClosedRequest>> Handle(GetPurchaseOrderClosedToEditById request, CancellationToken cancellationToken)
        {
            PurchaseOrder purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderWithItemsAndSupplierById(request.PurchaseOrderId);
            var budgtitem = await _purchaseOrderRepository.GetBudgetItemWithMWOById(purchaseOrder.MainBudgetItemId);
            if (purchaseOrder == null)
            {
                return Result<EditPurchaseOrderRegularClosedRequest>.Fail("Not found");
            }
            EditPurchaseOrderRegularClosedRequest result = new()
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
                    Budget = budgtitem.Budget,
                    Brand = budgtitem.Brand == null ? string.Empty : budgtitem.Brand.Name,
                    BudgetItemId = budgtitem.Id,
                    Name = budgtitem.Name,
                    Type = BudgetItemTypeEnum.GetType(budgtitem.Type),
                    IsMainItemTaxesNoProductive = budgtitem.IsMainItemTaxesNoProductive,


                },
                PurchaseOrderStatus = PurchaseOrderStatusEnum.GetType(purchaseOrder.PurchaseOrderStatus),
                POValueCurrencyOriginal = Math.Round(purchaseOrder.PurchaseOrderItems.Sum(x => x.UnitaryValueCurrency * x.Quantity), 2),
                PurchaseorderName = purchaseOrder.PurchaseorderName,
                PurchaseOrderId = purchaseOrder.Id,
                PurchaseRequisition = purchaseOrder.PurchaseRequisition,
                QuoteNo = purchaseOrder.QuoteNo,
                PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.Currency),
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
                    Budget = x.BudgetItem.Budget,
                    AssignedCurrency = x.PurchaseOrder.PurchaseOrderStatus != PurchaseOrderStatusEnum.Created.Id ? 
                    x.BudgetItem.PurchaseOrderItems.Sum(x => x.UnitaryValueCurrency*x.Quantity) : 0,
                    PotencialCurrency = x.PurchaseOrder.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id ? 
                    x.BudgetItem.PurchaseOrderItems.Sum(x => x.UnitaryValueCurrency * x.Quantity) : 0,
                    ActualCurrency = x.ActualCurrency,

                    Currency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),

                    CurrencyUnitaryValue = x.UnitaryValueCurrency ,



                }).ToList(),

                Supplier = purchaseOrder.Supplier == null ? new() : new()
                {
                    Id = purchaseOrder.Supplier.Id,
                    Name = purchaseOrder.Supplier.Name,
                    VendorCode = purchaseOrder.Supplier.VendorCode,
                    TaxCodeLD = purchaseOrder.Supplier.TaxCodeLD,
                    TaxCodeLP = purchaseOrder.Supplier.TaxCodeLP,
                    NickName = purchaseOrder.Supplier.NickName,

                },




            };
            return Result<EditPurchaseOrderRegularClosedRequest>.Success(result);
        }
    }
}
