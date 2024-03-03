using Domain.Entities.Data;

namespace Application.Interfaces
{
    public interface IBrandRepository : IRepository
    {
        Task UpdateBrand(Brand entity);
        Task AddBrand(Brand mWO);
        Task<bool> ReviewIfNameExist(string name);
        Task<bool> ReviewIfNameExist(Guid id, string name);
        Task<IQueryable<Brand>> GetBrandList();
        Task<Brand> GetBrandById(Guid id);
    }
}
