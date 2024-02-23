using Domain.Entities.Data;
using Shared.Commons.Results;
using System.Linq.Expressions;

namespace Application.Interfaces
{

    public interface IMWORepository : IRepository
    {
        Task UpdateMWO(MWO entity);
        Task AddMWO(MWO mWO);
        Task<bool> ReviewNameExist(string name);
        Task<bool> ReviewIfNumberExist(string cecNumber);
        Task<bool> ReviewNameExist(Guid id,string name);
        Task<IQueryable<MWO>> GetMWOList();
        Task<MWO> GetMWOById(Guid id);
        Task<MWO> GetMWOWithItemsById(Guid id);
    }
}
