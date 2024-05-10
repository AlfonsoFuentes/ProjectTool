namespace Infrastructure.Persistence.Repositories
{
    internal class Repository : IRepository
    {
        public IAppDbContext Context { get; set; }

        public Repository(IAppDbContext context)
        {
            Context = context;
        }

        public Task UpdateAsync<T>(T entity) where T : class
        {
            Context.Set<T>().Update(entity);

            return Task.CompletedTask;

        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            await Context.Set<T>().AddAsync(entity);
        }

        public Task RemoveAsync<T>(T entity) where T : class
        {
            Context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }
        public Task RemoveRangeAsync<T>(List<T> entities) where T : class
        {
            Context.Set<T>().RemoveRange(entities);
            return Task.CompletedTask;
        }
        public async Task<T?> GetByIdAsync<T>(Guid Id) where T : class
        {
            var result = await Context.Set<T>().FindAsync(Id);
            return result;
        }
        public async Task<MWO?> GetMWOById(Guid MWOId)
        {
            var mwo = await Context.Set<MWO>()
                .Include(x => x.BudgetItems)

                .FirstOrDefaultAsync(x => x.Id == MWOId);

            return mwo;

        }
      

        public async Task<BudgetItem?> GetBudgetItemToUpdate(Guid BudgetItemId)
        {
            var result = await Context.BudgetItems
                .Include(x => x.TaxesItems)
                 .ThenInclude(x => x.Selected)

                 .FirstOrDefaultAsync(x => x.Id == BudgetItemId);

            return result;
        }
        public async Task<BudgetItem?> GetBudgetItemToUpdateTaxes(Guid BudgetItemId)
        {
            var result = await Context.BudgetItems
                .Include(x => x.TaxesItems)
                .ThenInclude(x => x.Selected)
                .FirstOrDefaultAsync(x => x.Id == BudgetItemId);

            return result;
        }
        public async Task<List<TaxesItem>> GetTaxesItemsToDeleteBudgetItem(Guid BudgetItemId)
        {
            var result = await Context.TaxesItems
                .Include(x => x.BudgetItem)
                .Include(x => x.Selected)
                .Where(x => x.SelectedId == BudgetItemId)
                .ToListAsync();

            return result;
        }
        public async Task UpdateTaxesAndEgineeringItems(MWO mwo, bool updateTaxes, bool updateEgineeringItems, CancellationToken toke)
        {
            if (updateTaxes)
            {
                var taxesitem = mwo.ItemTaxNoProductive;
                taxesitem!.UnitaryCost = mwo.CapitalForTaxesCalculationsUSD * taxesitem.Percentage / 100;
                taxesitem!.Quantity = 1;
                await UpdateAsync(taxesitem);
                await Context.SaveChangesAsync(toke);
            }
            if (updateEgineeringItems)
            {
                var totalpercentageEngineering = mwo.TotalPercentEnginContingency;
                var budgetforEngineeringcalculation = mwo.CapitalForEngineeringCalculationUSD;
                var engineeringitems = mwo.BudgetItemsEngineering;
                foreach (var item in engineeringitems)
                {
                    var unitarycost = Math.Round(budgetforEngineeringcalculation * item.Percentage / (100 - totalpercentageEngineering), 2);
                    item.UnitaryCost = unitarycost;
                    item.Quantity = 1;
                    await UpdateAsync(item);
                }

                await Context.SaveChangesAsync(toke);
            }
        }

        public async Task<List<BudgetItem>> GetItemsToUpdateVersion1()
        {
            var budgetitems = await Context.BudgetItems
                 .Where(x => x.IsNotAbleToEditDelete == true &&
                 (x.Type == BudgetItemTypeEnum.Engineering.Id || x.Type == BudgetItemTypeEnum.Contingency.Id)).ToListAsync();

            return budgetitems;

        }
        public async Task<List<PurchaseOrder>> GetPurchaseOrderToUpdateVersion1()
        {
            var purchaseorders = await Context.PurchaseOrders
                .Include(x => x.PurchaseOrderItems)
                .Where(x => x.PurchaseOrderStatus > PurchaseOrderStatusEnum.Approved.Id)
                .ToListAsync();

            return purchaseorders;
        }
        public async Task<List<PurchaseOrderItem>> GetPurchaseOrderItemsToUpdateVersion2()
        {
            var purchaseorders = await Context.PurchaseOrderItems
                .Include(x => x.PurchaseOrder)
                .Where(x => x.PurchaseOrder.PurchaseOrderStatus > PurchaseOrderStatusEnum.Approved.Id)
                .ToListAsync();

            return purchaseorders;
        }
        public async Task<PurchaseOrder?> GetPurchaseOrderByIdCreatedAsync(Guid PurchaseOrderId)
        {
            var result = await Context.PurchaseOrders
              .Include(x => x.MWO)
              .Include(x => x.Supplier)
              .Include(x => x.PurchaseOrderItems).ThenInclude(x => x.BudgetItem)
             
              .SingleOrDefaultAsync(x => x.Id == PurchaseOrderId);

            return result;
        }
        public async Task<BudgetItem> GetTaxBudgetItemNoProductive(Guid MWOId)
        {
            var result = await Context.BudgetItems.SingleOrDefaultAsync(x => x.MWOId == MWOId && x.IsMainItemTaxesNoProductive == true);
            return result!;
        }
    }
}
