

using Shared.Enums.BudgetItemTypes;
using Shared.Enums.CostCenter;
using Shared.Enums.Currencies;
using Shared.Enums.PurchaseorderStatus;

namespace Application.Features.BudgetItems.Queries
{
    public record GetApprovedBudgetItemQuery(Guid BudgetItemId) : IRequest<IResult<BudgetItemApprovedResponse>>;
    internal class GetApprovedBudgetItemQueryHandler : IRequestHandler<GetApprovedBudgetItemQuery, IResult<BudgetItemApprovedResponse>>
    {

        private IPurchaseOrderRepository _purchaseOrderRepository;
        public GetApprovedBudgetItemQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<IResult<BudgetItemApprovedResponse>> Handle(GetApprovedBudgetItemQuery request, CancellationToken cancellationToken)
        {
            var budgetItem = await _purchaseOrderRepository.GetBudgetItemWithMWOById(request.BudgetItemId);

            if (budgetItem == null)
            {
                return Result<BudgetItemApprovedResponse>.Fail("Budget Item Not found!");
            }
            var purchaseordersbyitem = await _purchaseOrderRepository.GetPurchaseorderByBudgetItem(request.BudgetItemId);

            BudgetItemApprovedResponse budgetItemResponse = new()
            {
                BudgetItemId = budgetItem.Id,
                BudgetUSD = budgetItem.Budget,
                MWOCECName = $"CEC0000{budgetItem.MWO.MWONumber}",
                MWOId = budgetItem.MWOId,
                MWOName = budgetItem.MWO.Name,
                CostCenter = CostCenterEnum.GetName(budgetItem.MWO.CostCenter),
                IsMWOAssetProductive = budgetItem.MWO.IsAssetProductive,
                IsMainItemTaxesNoProductive = budgetItem.IsMainItemTaxesNoProductive,
                Name = budgetItem.Name,
                Type = BudgetItemTypeEnum.GetType(budgetItem.Type),
                Order = budgetItem.Order,


            };
            budgetItemResponse.PurchaseOrders = purchaseordersbyitem.Select(e => new NewPurchaseOrderResponse()
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
                PotentialCommitmentUSD = e.PotentialCommitmentUSD,
                MWOName = e.MWO.Name,
                CECName = e.MWO.CECName,
                AssignedUSD = e.AssignedUSD,

            }).ToList();


            return Result<BudgetItemApprovedResponse>.Success(budgetItemResponse);
        }
    }
}
