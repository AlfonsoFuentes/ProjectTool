﻿using Domain.Entities.Data;

namespace Application.Interfaces
{
    public interface IMWORepository 
    {
        Task UpdateMWO(MWO entity);
        Task AddMWO(MWO mWO);
        Task<bool> ReviewIfNameExist(string name);
        Task<bool> ReviewIfNumberExist(Guid MWOId, string cecNumber);
        Task<bool> ReviewIfNameExist(Guid id,string name);
               
        Task<MWO> GetMWOById(Guid id);
        Task<MWO> GetMWOWithItemsById(Guid id);
        Task<IQueryable<PurchaseOrder>> GetPurchaseOrdersByMWOId(Guid MWOId);
        Task<IQueryable<BudgetItem>> GetBudgetItemsByMWOId(Guid MWOId);
        Task<IEnumerable<MWO>> GetMWOApprovedList();
        Task<BudgetItem> GetBudgetItemsSalary(Guid MWOId);
        Task<BudgetItem> GetBudgetItemsContingency(Guid MWOId);
        Task UpdateBudgetItem(BudgetItem entity);
        Task<IEnumerable<MWO>> GetMWOCreatedList();
        Task<IEnumerable<MWO>> GetMWOClosedList();
        Task<MWO> GetMWOWithBudgetItemsPurchaseOrdersById(Guid id);
    }
}
