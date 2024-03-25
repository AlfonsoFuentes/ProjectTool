using Domain.Entities.Data;
using Shared.Commons.Results;
using System.Linq.Expressions;

namespace Application.Interfaces
{

    public interface IMWORepository : IRepository
    {
        Task UpdateMWO(MWO entity);
        Task AddMWO(MWO mWO);
        Task<bool> ReviewIfNameExist(string name);
        Task<bool> ReviewIfNumberExist(string cecNumber);
        Task<bool> ReviewIfNameExist(Guid id,string name);
        Task<IQueryable<MWO>> GetMWOList();
        Task<MWO> GetMWOById(Guid id);
        Task<MWO> GetMWOWithItemsById(Guid id);
        Task<IQueryable<PurchaseOrder>> GetPurchaseOrdersByMWOId(Guid MWOId);
        Task<IQueryable<BudgetItem>> GetBudgetItemsByMWOId(Guid MWOId);
    }
}
