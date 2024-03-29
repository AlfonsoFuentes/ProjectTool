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
                    Budget = budgtitem.Budget,
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
                PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.Currency),
                QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                TRMUSDCOP = purchaseOrder.USDCOP,
                TRMUSDEUR = purchaseOrder.USDEUR,
                CurrencyDate= purchaseOrder.CurrencyDate,
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
                    CurrencyUnitaryValue = (purchaseOrder.Currency == CurrencyEnum.USD.Id ? x.POValueUSD :
                    purchaseOrder.Currency == CurrencyEnum.COP.Id ? x.POValueUSD * purchaseOrder.USDCOP :
                    x.POValueUSD * purchaseOrder.USDEUR) / x.Quantity,
                    Budget = x.BudgetItem.Budget,
                    BudgetAssigned = x.PurchaseOrder.PurchaseOrderStatus != PurchaseOrderStatusEnum.Created.Id ? x.BudgetItem.PurchaseOrderItems.Sum(x => x.POValueUSD) : 0,
                    BudgetPotencial = x.PurchaseOrder.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id ? x.BudgetItem.PurchaseOrderItems.Sum(x => x.POValueUSD) : 0,
                    POItemActualUSD = x.Actual,

                }).ToList(),

                Supplier = purchaseOrder.Supplier == null ? new() : new()
                {
                    Id = purchaseOrder.Supplier.Id,
                    Name= purchaseOrder.Supplier.Name,
                    VendorCode = purchaseOrder.Supplier.VendorCode,
                    TaxCodeLD = purchaseOrder.Supplier.TaxCodeLD,
                    TaxCodeLP = purchaseOrder.Supplier.TaxCodeLP,
                    NickName = purchaseOrder.Supplier.NickName,

                },



            };
            return Result<EditPurchaseOrderRegularApprovedRequest>.Success(result);
        }
    }
}
