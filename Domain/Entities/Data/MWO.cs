using Shared.Enums.BudgetItemTypes;
using Shared.Enums.CostCenter;
using Shared.Enums.MWOStatus;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Data
{
    public class MWO : BaseEntity, ITenantEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public int CostCenter { get; set; }
        public string MWONumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        public int Focus { get; set; }
        public int Status { get; set; }
        public bool IsAssetProductive { get; set; }
        public double PercentageTaxForAlterations { get; set; }

        public ICollection<BudgetItem> BudgetItems { get; set; } = new List<BudgetItem>();
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
        public ICollection<SapAdjust> SapAdjusts { get; set; } = new List<SapAdjust>();
        public static MWO Create(string name, int type)
        {
            return new MWO()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Type = type,
                Status = 0,

            };
        }
        public static MWO Create()
        {
            return new MWO()
            {
                Id = Guid.NewGuid(),
                Status = 0,

            };
        }
        public BudgetItem AddBudgetItem(int type)
        {
            var LastOrder = BudgetItems.Where(x => x.Type == type).Count() == 0 ? 1 :
                BudgetItems.Where(x => x.Type == type).OrderBy(x => x.Order).Last().Order + 1;
            return new BudgetItem()
            {
                MWOId = Id,
                Id = Guid.NewGuid(),
                Order = LastOrder,
                Type = type,

            };
        }
        public PurchaseOrder AddPurchaseOrder()
        {
            PurchaseOrder item = PurchaseOrder.Create(Id);


            return item;
        }

        public SapAdjust AddAdjust()
        {
            return SapAdjust.Create(Id);
        }
        public DateTime ApprovedDate { get; set; }
        [NotMapped]
        public List<BudgetItem> BudgetItemsExpenses => BudgetItems == null ? new() : BudgetItems.Where(x => x.IsAlteration).ToList();
        [NotMapped]
        public List<BudgetItem> BudgetItemsCapital => BudgetItems == null ? new() : BudgetItems.Where(x => x.IsAlteration == false).ToList();
        [NotMapped]
        public double ExpensesUSD => BudgetItemsExpenses.Count == 0 ? 0 : BudgetItemsExpenses.Sum(x => x.Budget);
        [NotMapped]
        public double CapitalUSD => BudgetItemsCapital.Count == 0 ? 0 : BudgetItemsCapital.Sum(x => x.Budget);
        [NotMapped]
        public double AppropiationUSD => ExpensesUSD + CapitalUSD;
        [NotMapped]
        public bool HasExpenses => BudgetItemsExpenses.Count > 0;
        #region MWONotApproved
        [NotMapped]
        public List<BudgetItem> BudgetItemsWithoutEngineering => BudgetItemsCapital == null ? new() :
            BudgetItemsCapital.Where(x => x.IsEngineeringItem == false).ToList();
        [NotMapped]
        public List<BudgetItem> BudgetItemsWithoutEngineeringOrTaxes => BudgetItemsCapital == null ? new() :
            BudgetItemsCapital.Where(x => !(x.IsEngineeringItem == true || x.IsMainItemTaxesNoProductive == true)).ToList();

        [NotMapped]
        public List<BudgetItem> BudgetItemsEngineering => BudgetItemsCapital == null ? new() :
            BudgetItemsCapital.Where(x => x.IsEngineeringItem == true).ToList();

        [NotMapped]
        public BudgetItem? ItemCapitalizedSalary => BudgetItemsEngineering == null ? new() :
           BudgetItemsEngineering.FirstOrDefault(x => x.Type == BudgetItemTypeEnum.Engineering.Id && x.IsEngineeringItem == true);
        [NotMapped]
        public BudgetItem? ItemContingency => BudgetItemsEngineering == null ? new() :
            BudgetItemsEngineering.FirstOrDefault(x => x.Type == BudgetItemTypeEnum.Contingency.Id && x.IsEngineeringItem == true);
        [NotMapped]
        public BudgetItem? ItemTaxNoProductive => BudgetItemsCapital == null ? new() :
            BudgetItemsCapital.FirstOrDefault(x => x.Type == BudgetItemTypeEnum.Taxes.Id && x.IsMainItemTaxesNoProductive == true);

        #region EngineeringDataForCalculation
        [NotMapped]
        public double TotalPercentEnginContingency => BudgetItemsEngineering.Count == 0 ? 0 :
            BudgetItemsEngineering.Sum(x => x.Percentage);
        [NotMapped]
        public double PercentageCapitalizedSalary => ItemCapitalizedSalary == null ? 0 : ItemCapitalizedSalary.Percentage;
        [NotMapped]
        public double PercentageContingency => ItemContingency == null ? 0 : ItemContingency.Percentage;
        [NotMapped]
        public double CapitalForEngineeringCalculationUSD => BudgetItemsWithoutEngineering.Count == 0 ? 0 : BudgetItemsWithoutEngineering.Sum(x => x.Budget);

        #endregion
        #region TaxesCalculations
        [NotMapped]
        public double PercentageAssetNoProductive => ItemTaxNoProductive == null ? 0 : ItemTaxNoProductive.Percentage;
        [NotMapped]
        public double CapitalForTaxesCalculationsUSD => BudgetItemsWithoutEngineeringOrTaxes.Count == 0 ? 0 : BudgetItemsWithoutEngineeringOrTaxes.Sum(x => x.Budget);

        #endregion
        #endregion



        [NotMapped]
        public string CECName => Status == MWOStatusEnum.Created.Id ? "MWO not approved" : $"CEC0000{MWONumber}";

        #region DataMWOApproved

        #region CapitalMWOApproved
        [NotMapped]
        public double CapitalActualUSD => BudgetItemsCapital == null || BudgetItemsCapital.Count == 0 ? 0 :
             BudgetItemsCapital.Sum(x => x.ActualUSD);
        [NotMapped]
        public double CapitalAssignedUSD => BudgetItemsCapital == null || BudgetItemsCapital.Count == 0 ? 0 :
            BudgetItemsCapital.Sum(x => x.AssignedUSD);
        [NotMapped]
        public double CapitalApprovedUSD => BudgetItemsCapital == null || BudgetItemsCapital.Count == 0 ? 0 :
            BudgetItemsCapital.Sum(x => x.ApprovedUSD);
        [NotMapped]
        public double CapitalPotentialCommitmentUSD => BudgetItemsCapital == null || BudgetItemsCapital.Count == 0 ? 0 :
            BudgetItemsCapital.Sum(x => x.PotentialCommitmentUSD);

        [NotMapped]
        public double CapitalPendingToReceiveUSD => BudgetItemsCapital == null || BudgetItemsCapital.Count == 0 ? 0 :
            BudgetItemsCapital.Sum(x => x.CommitmentUSD);
        #endregion
        #region ExpensesMWOApproved
        [NotMapped]
        public double ExpensesActualUSD => BudgetItemsExpenses == null || BudgetItemsExpenses.Count == 0 ? 0 :
             BudgetItemsExpenses.Sum(x => x.ActualUSD);
        [NotMapped]
        public double ExpensesAssignedUSD => BudgetItemsExpenses == null || BudgetItemsExpenses.Count == 0 ? 0 :
            BudgetItemsExpenses.Sum(x => x.AssignedUSD);
        [NotMapped]
        public double ExpensesApprovedUSD => BudgetItemsExpenses == null || BudgetItemsExpenses.Count == 0 ? 0 :
            BudgetItemsExpenses.Sum(x => x.ApprovedUSD);
        [NotMapped]
        public double ExpensesPotentialCommitmentUSD => BudgetItemsExpenses == null || BudgetItemsExpenses.Count == 0 ? 0 :
            BudgetItemsExpenses.Sum(x => x.PotentialCommitmentUSD);

        [NotMapped]
        public double ExpensesPendingToReceiveUSD => BudgetItemsExpenses == null || BudgetItemsExpenses.Count == 0 ? 0 :
            BudgetItemsExpenses.Sum(x => x.CommitmentUSD);
        #endregion
        #endregion
        [NotMapped]
        public string CostCenterName => CostCenterEnum.GetName(CostCenter);
    }
}
