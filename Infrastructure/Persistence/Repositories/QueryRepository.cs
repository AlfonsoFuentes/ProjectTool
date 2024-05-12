using DocumentFormat.OpenXml.Office2010.Excel;

namespace Infrastructure.Persistence.Repositories
{
    internal class QueryRepository : IQueryRepository
    {
        public IAppDbContext Context { get; set; }

        public QueryRepository(IAppDbContext context)
        {
            Context = context;
        }
        public async Task<List<T>> GetAllAsync<T>() where T : class
        {
            var rows = Context.Set<T>().
              AsNoTracking().
              AsSplitQuery().
              AsQueryable();
            return await rows.ToListAsync();
        }
        public async Task<List<Brand>> GetAllBrandsAsync()
        {
            var rows = Context.Brands.
               OrderBy(x => x.Name).
              AsNoTracking().
              AsSplitQuery().
              AsQueryable();
            return await rows.ToListAsync();
        }

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            var rows = Context.Suppliers.
              OrderBy(x => x.Name).
             AsNoTracking().
             AsSplitQuery().
             AsQueryable();
            return await rows.ToListAsync();
        }

        public async Task<Brand?> GetBrandByIdAsyn(Guid id)
        {
            var row = await Context.Brands.
               AsNoTracking().
              AsSplitQuery().
              AsQueryable()
              .FirstOrDefaultAsync(x => x.Id == id);
            return row;
        }
        public async Task<BudgetItem?> GetBudgetItemToUpdateByIdAsync(Guid Id)
        {
            var row = await Context.BudgetItems
                .Include(x => x.MWO)
                .Include(x => x.Brand)
                .Include(x => x.TaxesItems)
                .ThenInclude(x => x.Selected)

                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == Id);
            return row;
        }
        public async Task<BudgetItem?> GetBudgetItemMWOApprovedAsync(Guid Id)
        {
            var row = await Context.BudgetItems
                .Include(x => x.MWO)
                .Include(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrder).ThenInclude(x => x.Supplier)
                .Include(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == Id);
            return row;
        }

        public async Task<MWO?> GetMWOByIdCreatedAsync(Guid id)
        {
            var row = await Context.MWOs
                .Include(x => x.BudgetItems)

                .ThenInclude(x => x.Brand)
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()

              .FirstOrDefaultAsync(x => x.Id == id);
            return row;
        }
        public async Task<Supplier?> GetSupplierByIdAsync(Guid id)
        {
            var row = await Context.Suppliers.
              AsNoTracking().
             AsSplitQuery().
             AsQueryable()
             .FirstOrDefaultAsync(x => x.Id == id);
            return row;
        }

        public async Task<List<MWO>> GetAllMWOsCreatedAsync()
        {
            var result = await Context.MWOs
                .Include(x => x.BudgetItems)
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Status == MWOStatusEnum.Created.Id)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return result;


        }
        public async Task<List<MWO>> GetAllMWOsApprovedAsync()
        {
            var result = await Context.MWOs
                .Include(x => x.BudgetItems).ThenInclude(x => x.Brand)
                .Include(x => x.BudgetItems).ThenInclude(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrder)
                .Include(x => x.BudgetItems).ThenInclude(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Status == MWOStatusEnum.Approved.Id)
                .OrderBy(x => x.MWONumber)
                .ToListAsync();

            return result;


        }
        public async Task<MWO?> GetMWOByIdWithPurchaseOrderAsync(Guid MWOId)
        {
            var result = await Context.MWOs
                .Include(x => x.PurchaseOrders).ThenInclude(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                .Include(x => x.PurchaseOrders).ThenInclude(x => x.PurchaseOrderItems).ThenInclude(x => x.BudgetItem)
                .Include(x => x.PurchaseOrders).ThenInclude(x => x.Supplier)

                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == MWOId);

            return result;


        }
        public async Task<MWO?> GetMWOByIdApprovedAsync(Guid MWOId)
        {
            var result = await Context.MWOs
                .Include(x => x.BudgetItems).ThenInclude(x => x.Brand)
                .Include(x => x.BudgetItems).ThenInclude(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrder)
                .Include(x => x.BudgetItems).ThenInclude(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrder).ThenInclude(x => x.Supplier)
                .Include(x => x.BudgetItems).ThenInclude(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()

                .OrderBy(x => x.MWONumber)
                .SingleOrDefaultAsync(x => x.Id == MWOId);

            return result;


        }
        public async Task<List<UpdatedSoftwareVersion>> GetVersionsByUserIdAsync(string userId)
        {
            var result = await Context.UpdatedSoftwareVersions
                .Where(x => x.AplicationUserId == userId)
                .ToListAsync();

            return result;
        }

        public async Task<List<PurchaseOrder>> GetAllPurchaseOrderApprovedAsync()
        {
            var result = await Context.PurchaseOrders
                .Include(x => x.MWO)
                .Include(x => x.Supplier)
                .Include(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => !(x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Closed.Id || x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id))
                .OrderBy(x => x.POExpectedDateDate)
                .ToListAsync();

            return result;
        }

        public async Task<List<PurchaseOrder>> GetAllPurchaseOrderCreatedAsync()
        {
            var result = await Context.PurchaseOrders
              .Include(x => x.MWO)
              .Include(x => x.Supplier)
              .Include(x => x.PurchaseOrderItems)
              .AsNoTracking()
              .AsSplitQuery()
              .AsQueryable()
              .Where(x => x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Created.Id)
              .OrderBy(x => x.PurchaseRequisition)
              .ToListAsync();

            return result;
        }

        public async Task<List<PurchaseOrder>> GetAllPurchaseOrderClosedAsync()
        {
            var result = await Context.PurchaseOrders
                .Include(x => x.MWO)
                .Include(x => x.Supplier)
                .Include(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.PurchaseOrderStatus == PurchaseOrderStatusEnum.Closed.Id)
                .OrderBy(x => x.POClosedDate)
                .ToListAsync();

            return result;
        }

        public async Task<PurchaseOrder?> GetPurchaseOrderByIdCreatedAsync(Guid PurchaseOrderId)
        {
            var result = await Context.PurchaseOrders
              .Include(x => x.MWO)
              .Include(x => x.Supplier)
              .Include(x => x.PurchaseOrderItems).ThenInclude(x => x.BudgetItem)
              .AsNoTracking()
              .AsSplitQuery()
              .AsQueryable()
              .SingleOrDefaultAsync(x => x.Id == PurchaseOrderId);

            return result;
        }
        public async Task<PurchaseOrder?> GetPurchaseOrderByIdToReceiveAsync(Guid PurchaseOrderId)
        {
            var result = await Context.PurchaseOrders
              .Include(x => x.MWO)
              .Include(x => x.Supplier)
              .Include(x => x.PurchaseOrderItems).ThenInclude(x => x.BudgetItem)
              .Include(x => x.PurchaseOrderItems).ThenInclude(x => x.PurchaseOrderReceiveds)
              .AsNoTracking()
              .AsSplitQuery()
              .AsQueryable()
              .SingleOrDefaultAsync(x => x.Id == PurchaseOrderId);

            return result;
        }
        public async Task<MWO> GetSapAdjustsByMWOId(Guid MWOId)
        {

            var context = await Context.MWOs
                .Include(x => x.BudgetItems)
                .Include(x => x.SapAdjusts)
                 .AsNoTracking()
              .AsSplitQuery()
              .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == MWOId);



            return context!;

        }
    }
}
