using Shared.Models.BudgetItemTypes;
using Shared.Models.PurchaseorderStatus;
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
        public int Status { get; set; }
        public bool IsAssetProductive { get; set; }
        public double PercentageTaxForAlterations { get; set; }
        public double PercentageAssetNoProductive { get; set; }
        public double PercentageEngineering { get; set; }
        public double PercentageContingency { get; set; }
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
            BudgetItemsCapital.Sum(x => x.PendingToReceiveUSD);
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
            BudgetItemsExpenses.Sum(x => x.PendingToReceiveUSD);
        [NotMapped]
        public List<BudgetItem> BudgetItemsExpenses => BudgetItems == null ? new() : BudgetItems.Where(x => x.IsAlteration).ToList();
        [NotMapped]
        public List<BudgetItem> BudgetItemsCapital => BudgetItems == null ? new() : BudgetItems.Where(x => x.IsAlteration == false).ToList();

        public double ExpensesUSD => BudgetItemsExpenses.Count == 0 ? 0 : BudgetItemsExpenses.Sum(x => x.Budget);
        [NotMapped]
        public double CapitalUSD => BudgetItemsCapital.Count == 0 ? 0 : BudgetItemsCapital.Sum(x => x.Budget);
        [NotMapped]
        public double AppropiationUSD => ExpensesUSD + CapitalUSD;
        [NotMapped]
        public bool HasExpenses => BudgetItemsExpenses.Count > 0;
        [NotMapped]
        public string CECName => string.IsNullOrEmpty(MWONumber) ? "MWO not approved" : $"CEC0000{MWONumber}";
    }
}
