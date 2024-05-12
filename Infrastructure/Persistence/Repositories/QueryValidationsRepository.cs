namespace Infrastructure.Persistence.Repositories
{
    internal class QueryValidationsRepository : IQueryValidationsRepository
    {
        public IAppDbContext Context { get; set; }

        public QueryValidationsRepository(IAppDbContext context)
        {
            Context = context;
        }
        public async Task<bool> ReviewIfSoftwareVersionNameExist(string name)
        {
            return await Context.SoftwareVersions
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .AnyAsync(x => x.Name.ToLower() == name);
        }

        public async Task<bool> ReviewIfSoftwareVersionNameExist(Guid Id, string name)
        {
            return await Context.SoftwareVersions
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id != Id).AnyAsync(x => x.Name.ToLower() == name);
        }
        public async Task<bool> ReviewIfBrandNameExist(string name)
        {
            return await Context.Brands
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .AnyAsync(x => x.Name.ToLower() == name);
        }

        public async Task<bool> ReviewIfBrandNameExist(Guid Id, string name)
        {
            return await Context.Brands
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id != Id).AnyAsync(x => x.Name.ToLower() == name);
        }
        public async Task<bool> ReviewIfSupplierNameExist(string name)
        {
            return await Context.Suppliers
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .AnyAsync(x => x.Name.ToLower() == name);
        }

        public async Task<bool> ReviewIfSupplierNameExist(Guid Id, string name)
        {
            return await Context.Suppliers
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id != Id).AnyAsync(x => x.Name.ToLower() == name);
        }
        public async Task<bool> ReviewSupplierVendorCodeExist(string vendorcode)
        {

            var retorno = await Context.Suppliers
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .AnyAsync(x => x.VendorCode == vendorcode);
            return retorno;
        }
        public async Task<bool> ReviewSupplierVendorCodeExist(Guid Id, string vendorcode)
        {

            var retorno = await Context.Suppliers
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id != Id).AnyAsync(x => x.VendorCode == vendorcode);
            return retorno;
        }

        public async Task<bool> ReviewSupplierEmailExist(string? email)
        {
            return await Context.Suppliers
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .AnyAsync(x => x.ContactEmail == email);
        }
        public async Task<bool> ReviewSupplierEmailExist(Guid Id, string? email)
        {
            return await Context.Suppliers
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id != Id).AnyAsync(x => x.ContactEmail == email);
        }
        public async Task<bool> ReviewIfMWONameExist(string name)
        {
            return await Context.MWOs
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .AnyAsync(x => x.Name.ToLower() == name);
        }

        public async Task<bool> ReviewIfMWONameExist(Guid Id, string name)
        {
            return await Context.MWOs
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id != Id).AnyAsync(x => x.Name.ToLower() == name);
        }
        public async Task<bool> ReviewIfMWONumberExist(Guid Id, string mwonumber)
        {
            return await Context.MWOs
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id != Id).AnyAsync(x => x.MWONumber.ToLower() == mwonumber);
        }
        public async Task<bool> ValidatePurchaseOrderNameExist(Guid MWOId, Guid PurchaseOrderId, string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return await Context.PurchaseOrders
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.MWOId == MWOId && x.Id != PurchaseOrderId).AnyAsync(x => x.PurchaseorderName == name);
        }
        public async Task<bool> ValidatePurchaseOrderNameExist(Guid MWOId, string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return await Context.PurchaseOrders
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.MWOId == MWOId).AnyAsync(x => x.PurchaseorderName == name);
        }
        public async Task<bool> ValidatePurchaseNumberExist(string ponumber)
        {
            if (string.IsNullOrEmpty(ponumber)) return false;
            return await Context.PurchaseOrders
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .AnyAsync(x => x.PONumber == ponumber);
        }
        public async Task<bool> ValidatePurchaseNumberExist(Guid PurchaseOrderId, string ponumber)
        {
            if (string.IsNullOrEmpty(ponumber)) return false;
            return await Context.PurchaseOrders
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id != PurchaseOrderId).AnyAsync(x => x.PONumber == ponumber);
        }
        public async Task<bool> ValidatePurchaseRequisitionExist(string purchaserequisition)
        {
            if (string.IsNullOrEmpty(purchaserequisition)) return false;
            var result = await Context.PurchaseOrders
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .AnyAsync(x => x.PurchaseRequisition == purchaserequisition);
            return result;
        }
        public async Task<bool> ValidatePurchaseRequisitionExist(Guid PurchaseOrderId, string purchaserequisition)
        {
            if (string.IsNullOrEmpty(purchaserequisition)) return false;
            return await Context.PurchaseOrders
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id != PurchaseOrderId).AnyAsync(x => x.PurchaseRequisition == purchaserequisition);
        }
        public async Task<bool> ReviewIfBudgetItemNameExist(Guid MWOId, string name)
        {

            return await Context.BudgetItems
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .AnyAsync(x => x.MWOId == MWOId && x.Name == name);
        }
        public async Task<bool> ReviewIfBudgetItemNameExist(Guid Id, Guid MWOId, string name)
        {

            return await Context.BudgetItems
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Where(x => x.Id != Id && x.MWOId == MWOId).AnyAsync(x => x.Name == name);
        }
    }
}
