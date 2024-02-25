namespace Domain.Entities.Data
{
    public class MWO : BaseEntity
    {
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
    }
}
