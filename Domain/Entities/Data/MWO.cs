namespace Domain.Entities.Data
{
    public class MWO : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        public int Status { get; set; }
        public ICollection<BudgetItem> BudgetItems { get; set; } = new List<BudgetItem>();

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
            var LastOrder = BudgetItems.Where(x => x.Type == type).Count() == 0 ? 1 : BudgetItems.Where(x => x.Type == type).OrderBy(x=>x.Order).Last().Order;
            return new BudgetItem()
            {
                MWOId = Id,
                Id = Guid.NewGuid(),
                Order = LastOrder + 1,
                Type = type,

            };
        }

    }
}
