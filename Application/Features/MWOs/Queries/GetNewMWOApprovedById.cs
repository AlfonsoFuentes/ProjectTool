using Shared.Enums.BudgetItemTypes;
using Shared.Enums.CostCenter;
using Shared.Enums.MWOStatus;
using Shared.Enums.MWOTypes;
using Shared.Models.MWO;

namespace Application.Features.MWOs.Queries
{
    public record GetNewMWOApprovedById(Guid MWOId) : IRequest<IResult<MWOApprovedWithBudgetItemsResponse>>;

    internal class GetNewMWOApprovedByIdHandler : IRequestHandler<GetNewMWOApprovedById, IResult<MWOApprovedWithBudgetItemsResponse>>
    {
        private IMWORepository Repository { get; set; }

        public GetNewMWOApprovedByIdHandler(IMWORepository repository)
        {
            Repository = repository;
        }

        public async Task<IResult<MWOApprovedWithBudgetItemsResponse>> Handle(GetNewMWOApprovedById request, CancellationToken cancellationToken)
        {
            var mwo = await Repository.GetMWOWithBudgetItemsPurchaseOrdersById(request.MWOId);
            if (mwo == null)
            {
                return Result<MWOApprovedWithBudgetItemsResponse>.Fail("MWO not found");
            }
            
            MWOApprovedWithBudgetItemsResponse result = new()
            {
                Id = mwo.Id,
                Name = mwo.Name,
                Type = MWOTypeEnum.GetType(mwo.Type),

                IsAssetProductive = mwo.IsAssetProductive,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo.PercentageContingency,
                PercentageEngineering = mwo.PercentageCapitalizedSalary,


                CECName = $"CEC0000{mwo.MWONumber}",
                CostCenter = CostCenterEnum.GetName(mwo.CostCenter),
                CreatedBy = mwo.CreatedByUserName,
                CreatedOn = mwo.CreatedDate.ToString("d"),

                MWOStatus = MWOStatusEnum.GetType(mwo.Status),
                PercentageTaxForAlterations = mwo.PercentageTaxForAlterations,
                HasExpenses = mwo.HasExpenses,
                Capital = new()
                {
                    BudgetUSD = mwo.CapitalUSD,
                    ActualUSD = mwo.CapitalActualUSD,
                    ApprovedUSD = mwo.CapitalApprovedUSD,
                    AssignedUSD = mwo.CapitalAssignedUSD,
                    PotentialCommitmentUSD = mwo.CapitalPotentialCommitmentUSD,
                },
                Expenses = new()
                {
                    BudgetUSD = mwo.ExpensesUSD,
                    ActualUSD = mwo.ExpensesActualUSD,
                    ApprovedUSD = mwo.ExpensesApprovedUSD,
                    PotentialCommitmentUSD = mwo.ExpensesPotentialCommitmentUSD,
                    AssignedUSD = mwo.ExpensesAssignedUSD,

                },
            };

            var budgetitems = mwo.BudgetItems.OrderBy(x => x.Nomeclatore);
            foreach (var budgetitem in budgetitems)
            {
                var budgetitemdto = new NewBudgetItemsWithPurchaseorders()
                {
                    BudgetItemId = budgetitem.Id,
                    MWOId = budgetitem.MWOId,
                    Name = budgetitem.Name,
                    Type = BudgetItemTypeEnum.GetType(budgetitem.Type),
                    BudgetUSD=budgetitem.Budget,
                    Nomenclatore = budgetitem.Nomeclatore,
                    IsMainItemTaxesNoProductive = budgetitem.IsMainItemTaxesNoProductive,
                    IsMWOAssetProductive = budgetitem.MWO.IsAssetProductive,
                    Percentage = budgetitem.Percentage,
                    
                };
                var purchaseorderitems = budgetitem.PurchaseOrderItems.Where(x => !x.IsTaxAlteration);
                foreach (var purchaseorderitem in purchaseorderitems)
                {
                    NewPurchaseOrderItemResponse purchaseorderiten = new();
                    purchaseorderiten.BudgetItemId = budgetitem.Id;
                    purchaseorderiten.PurchaseOrderId = purchaseorderitem.PurchaseOrderId;
                    purchaseorderiten.Name = purchaseorderitem.Name;
                    purchaseorderiten.ActualCurrency = purchaseorderitem.POItemActualCurrency;
                    purchaseorderiten.UnitaryValueCurrency = purchaseorderitem.UnitaryValueCurrency;
                    purchaseorderiten.Quantity = purchaseorderitem.Quantity;
                    purchaseorderiten.IsTaxAlteration = purchaseorderitem.IsTaxAlteration;
                    purchaseorderiten.IsTaxNoProductive = purchaseorderitem.IsTaxNoProductive;
                    purchaseorderiten.USDCOP = purchaseorderitem.USDCOP;
                    purchaseorderiten.USDEUR = purchaseorderitem.USDEUR;
                    purchaseorderiten.PurchaseOrderStatus = purchaseorderitem.PurchaseorderStatus;
                    purchaseorderiten.PurchaseOrderCurrency = purchaseorderitem.PurchaseOrderCurrency;
                    purchaseorderiten.QuoteCurrency = purchaseorderitem.QuoteCurrency;
                    purchaseorderiten.PurchaseOrderNumber = purchaseorderitem.PurchaseOrderNumber;
                    purchaseorderiten.Supplier = purchaseorderitem.Supplier;
                    purchaseorderiten.PurchaseRequisition = purchaseorderitem.PurchaseRequisition;
                    purchaseorderiten.ExpectedOn = purchaseorderitem.POExpectedDate;
                    purchaseorderiten.IsTaxEditable = purchaseorderitem.IsTaxEditable;
                    purchaseorderiten.IsCapitalizedSalary = purchaseorderitem.IsCapitalizedSalary;
                    budgetitemdto.PurchaseOrderItems.Add(purchaseorderiten);
                }


                result.BudgetItems.Add(budgetitemdto);
            }


            return Result<MWOApprovedWithBudgetItemsResponse>.Success(result);
        }
    }

}
