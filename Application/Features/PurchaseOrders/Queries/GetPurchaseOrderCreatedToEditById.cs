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
    public record GetPurchaseOrderCreatedToEditById(Guid PurchaseOrderId) : IRequest<IResult<EditPurchaseOrderRegularCreatedRequest>>;
    internal class GetPurchaseOrderCreatedToEditByIdHandler : IRequestHandler<GetPurchaseOrderCreatedToEditById, IResult<EditPurchaseOrderRegularCreatedRequest>>
    {
        private IPurchaseOrderRepository _purchaseOrderRepository;

        public GetPurchaseOrderCreatedToEditByIdHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<EditPurchaseOrderRegularCreatedRequest>> Handle(GetPurchaseOrderCreatedToEditById request, CancellationToken cancellationToken)
        {
            PurchaseOrder purchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderWithItemsAndSupplierById(request.PurchaseOrderId);
            var budgtitem = await _purchaseOrderRepository.GetBudgetItemWithMWOById(purchaseOrder.MainBudgetItemId);
            if (purchaseOrder == null)
            {
                return Result<EditPurchaseOrderRegularCreatedRequest>.Fail("Not found");
            }
            EditPurchaseOrderRegularCreatedRequest result = new()
            {
                MWOId = purchaseOrder.MWOId,
                CostCenter = CostCenterEnum.GetName(purchaseOrder.MWO.CostCenter),
                IsAssetProductive = purchaseOrder.MWO.IsAssetProductive,
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
                PurchaseorderName = purchaseOrder.PurchaseorderName,
                PurchaseOrderId = purchaseOrder.Id,
                PurchaseRequisition = purchaseOrder.PurchaseRequisition,
                QuoteNo = purchaseOrder.QuoteNo,
                PurchaseOrderCurrency = CurrencyEnum.GetType(purchaseOrder.Currency),
                QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                USDCOP = purchaseOrder.USDCOP,
                USDEUR = purchaseOrder.USDEUR,
                CurrencyDate = purchaseOrder.CurrencyDate,
                PurchaseOrderItems = purchaseOrder.PurchaseOrderItems.Select(x => new PurchaseOrderItemRequest()
                {
                   
                    BudgetItemId = x.BudgetItemId,
                    PurchaseOrderItemId = x.Id,
                    Name = x.Name,
                    Quantity = x.Quantity,
                    BudgetItemName = x.BudgetItem.Name,
                    TRMUSDCOP = purchaseOrder.USDCOP,
                    TRMUSDEUR = purchaseOrder.USDEUR,
                    QuoteCurrency = CurrencyEnum.GetType(purchaseOrder.QuoteCurrency),
                  
                    PurchaseOrderCurrency= CurrencyEnum.GetType(purchaseOrder.Currency),
                    QuoteCurrencyValue = GetQuoteCurrencyValue( x.UnitaryValueCurrency, purchaseOrder.QuoteCurrency, purchaseOrder.Currency,
                    purchaseOrder.USDCOP, purchaseOrder.USDEUR),
                    Budget = x.BudgetItem.Budget,
                    ActualCurrency = x.ActualCurrency,

                   

                    AssignedCurrency = x.BudgetItem.PurchaseOrderItems.Where(x =>
                    x.PurchaseOrder.PurchaseOrderStatus != PurchaseOrderStatusEnum.Created.Id).Sum(x => x.UnitaryValueCurrency * x.Quantity),

                    PotencialCurrency = x.BudgetItem.PurchaseOrderItems.Where(x =>
                    x.PurchaseOrder.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.UnitaryValueCurrency * x.Quantity),


                    OriginalAssignedCurrency = x.BudgetItem.PurchaseOrderItems.Where(x =>
                    x.PurchaseOrder.PurchaseOrderStatus != PurchaseOrderStatusEnum.Created.Id).Sum(x => x.UnitaryValueCurrency * x.Quantity),
                    OriginalPotencialCurrency = x.BudgetItem.PurchaseOrderItems.Where(x =>
                    x.PurchaseOrder.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.UnitaryValueCurrency * x.Quantity),
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
            return Result<EditPurchaseOrderRegularCreatedRequest>.Success(result);
        }

        double GetQuoteCurrencyValue(double UnitaryValue,int quotecurrency, int purchaseOrdercurrency, double TRMUSDCOP,double TRMUSDEUR)
        {
          var result=  
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
