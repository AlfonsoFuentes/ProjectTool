using Domain.Entities.Data;

namespace Application.Interfaces
{
    public interface ISupplierRepository:IRepository
    {
        Task UpdateSupplier(Supplier entity);
        Task AddSupplier(Supplier mWO);
        Task<bool> ReviewNameExist(string name);
        Task<bool> ReviewVendorCodeExist(string vendorcode);
        Task<bool> ReviewEmailExist(string? email);
        Task<IQueryable<Supplier>> GetSupplierList();
        Task<Supplier> GetSupplierById(Guid id);
        Task<bool> ReviewNameExist(Guid Id, string name);
        Task<bool> ReviewVendorCodeExist(Guid Id, string vendorcode);
        Task<bool> ReviewEmailExist(Guid Id, string? email);
    }
}
