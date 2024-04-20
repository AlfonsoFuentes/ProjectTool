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
        
        //public double Capital { get; private set; }
        //public double Expenses { get; private set; }
        //public double ActualCapital { get; private set; }
        //public double CommitmentCapital { get; private set; }
        //public double PotentialCommitmentCapital { get; private set; }
        //public double ActualExpenses { get; private set; }
        //public double CommitmentExpenses { get; private set; }
        //public double PotentialCommitmentExpenses { get; private set; }

        //public void SetDataNotApproved()
        //{
        //    if (BudgetItems != null && BudgetItems.Count() > 0)
        //    {
        //        Expenses = BudgetItems.Where(x => x.Type == BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget);
        //        Capital = BudgetItems.Where(x => x.Type != BudgetItemTypeEnum.Alterations.Id).Sum(x => x.Budget);
        //    }
        //}
        //public void SetDataApproved()
        //{
        //    if (PurchaseOrders != null && PurchaseOrders.Count() > 0)
        //    {
        //        ActualCapital = PurchaseOrders.Where(x => !x.IsAlteration).Sum(x => x.ActualUSD);
        //        ActualExpenses = PurchaseOrders.Where(x => x.IsAlteration).Sum(x => x.ActualUSD);

        //        var povalueCapital = PurchaseOrders.Where(x => !x.IsAlteration && x.PurchaseOrderStatus != PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD);
        //        var povalueExpenses = PurchaseOrders.Where(x => x.IsAlteration && x.PurchaseOrderStatus != PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD);

        //        CommitmentCapital = povalueCapital - ActualCapital;
        //        CommitmentExpenses = povalueExpenses - ActualExpenses;

        //        PotentialCommitmentCapital = PurchaseOrders.Where(x => !x.IsAlteration && x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD);
        //        PotentialCommitmentExpenses = PurchaseOrders.Where(x => x.IsAlteration && x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id).Sum(x => x.POValueUSD);
        //    }
        //}
    }
}
