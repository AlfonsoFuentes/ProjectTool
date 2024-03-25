using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    internal class PurchaseOrderValidatorRepository : IPurchaseOrderValidatorRepository
    {
        public IAppDbContext Context { get; }

        public PurchaseOrderValidatorRepository(IAppDbContext context)
        {
            this.Context = context;
        }

        public async Task<bool> ValidateNameExist(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return await Context.PurchaseOrders.AnyAsync(x=>x.PurchaseorderName == name);
        }
       
        public async Task<bool> ValidatePurchaseRequisition(string purchaserequisition)
        {
            if (string.IsNullOrEmpty(purchaserequisition)) return false;
            var result= await Context.PurchaseOrders.AnyAsync(x => x.PurchaseRequisition == purchaserequisition);
            return result;
        }
        public async Task<bool> ValidateNameExist(Guid PurchaseOrderId, string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return await Context.PurchaseOrders.Where(x => x.Id != PurchaseOrderId).AnyAsync(x => x.PurchaseorderName == name);
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
    }
}
