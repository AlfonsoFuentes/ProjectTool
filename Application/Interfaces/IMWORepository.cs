using Domain.Entities.Data;
using Shared.Commons.Results;
using Shared.Commons.UserManagement;
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
               
        Task<MWO> GetMWOById(Guid id);
        Task<MWO> GetMWOWithItemsById(Guid id);
        Task<IQueryable<PurchaseOrder>> GetPurchaseOrdersByMWOId(Guid MWOId);
        Task<IQueryable<BudgetItem>> GetBudgetItemsByMWOId(Guid MWOId);

        //Task UpdateDataForNotApprovedMWO(Guid MWOId, CancellationToken token);
        //Task UpdateDataForApprovedMWO(Guid MWOId, CancellationToken token);
        Task<IEnumerable<MWO>> GetMWOList(CurrentUser CurrentUser);
    }
}
