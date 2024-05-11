namespace Infrastructure.Persistence.Repositories
{
    internal class VersionRepository:IVersionRepository
    {
        public IAppDbContext Context { get; set; }

        public VersionRepository(IAppDbContext context)
        {
            Context = context;
        }
        public async Task<List<BudgetItem>> GetItemsToUpdateVersion1()
        {
            var budgetitems = await Context.BudgetItems
                 .Where(x => x.IsNotAbleToEditDelete == true &&
                 (x.Type == BudgetItemTypeEnum.Engineering.Id || x.Type == BudgetItemTypeEnum.Contingency.Id)).ToListAsync();

            return budgetitems;

        }
        public async Task<List<PurchaseOrderItem>> GetPurchaseOrderItemsToUpdateVersion2()
        {
            var purchaseorders = await Context.PurchaseOrderItems
                .Include(x => x.PurchaseOrder)
                .Where(x => x.PurchaseOrder.PurchaseOrderStatus > PurchaseOrderStatusEnum.Approved.Id)
                .ToListAsync();

            return purchaseorders;
        }

        public async Task<List<PurchaseOrderItemReceived>> GetPurchaseOrderItemsReceivedToUpdateVersion3()
        {
            var receiveds = await Context.PurchaseOrderItemReceiveds
                .ToListAsync();

            return receiveds;
        }
    }
}
