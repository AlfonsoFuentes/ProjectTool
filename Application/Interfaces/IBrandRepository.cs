using Domain.Entities.Data;

namespace Application.Interfaces
{
    public interface IBrandRepository : IRepository
    {
        Task UpdateBrand(Brand entity);
        Task AddBrand(Brand mWO);
        Task<bool> ReviewNameExist(string name);
        Task<bool> ReviewNameExist(Guid id, string name);
        Task<IQueryable<Brand>> GetBrandList();
        Task<Brand> GetBrandById(Guid id);
    }
}
