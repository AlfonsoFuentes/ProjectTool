namespace Infrastructure.Persistence.Repositories
{
    internal class PurchaseOrderValidatorRepository : IPurchaseOrderValidatorRepository
    {
        public IAppDbContext Context { get; }

        public PurchaseOrderValidatorRepository(IAppDbContext context)
        {
            this.Context = context;
        }

        public async Task<bool> ValidateNameExist(Guid MWOId, string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return await Context.PurchaseOrders.Where(x => x.MWOId == MWOId).AnyAsync(x => x.PurchaseorderName == name);
        }

        public async Task<bool> ValidatePurchaseRequisition(string purchaserequisition)
        {
            if (string.IsNullOrEmpty(purchaserequisition)) return false;
            var result = await Context.PurchaseOrders.AnyAsync(x => x.PurchaseRequisition == purchaserequisition);
            return result;
        }
        public async Task<bool> ValidateNameExist(Guid MWOId, Guid PurchaseOrderId, string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return await Context.PurchaseOrders.Where(x => x.MWOId == MWOId && x.Id != PurchaseOrderId).AnyAsync(x => x.PurchaseorderName == name);
        }
        public async Task<bool> ValidatePurchaseRequisition(Guid PurchaseOrderId, string purchaserequisition)
        {
            if (string.IsNullOrEmpty(purchaserequisition)) return false;
            return await Context.PurchaseOrders.Where(x => x.Id != PurchaseOrderId).AnyAsync(x => x.PurchaseRequisition == purchaserequisition);
        }
        public async Task<bool> ValidatePONumber(Guid PurchaseOrderId, string ponumber)
        {
            if (string.IsNullOrEmpty(ponumber)) return false;
            return await Context.PurchaseOrders.Where(x => x.Id != PurchaseOrderId).AnyAsync(x => x.PONumber == ponumber);
        }

        public async Task<bool> ValidatePONumber(string ponumber)
        {
            if (string.IsNullOrEmpty(ponumber)) return false;
            return await Context.PurchaseOrders.AnyAsync(x => x.PONumber == ponumber);
        }
    }
}
