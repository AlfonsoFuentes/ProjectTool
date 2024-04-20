using Domain.Entities.Data;

namespace Application.Interfaces
{
    public interface ISapAdjustRepository:IRepository
    {
        Task AddSapAdAjust(SapAdjust sapAdjust);
        Task UpdateSapAdjust(SapAdjust sapAdjust);
        Task DeleteSapAdAjust(Guid sapAdAjustId);
        Task<MWO> GetSapAdjustsByMWOId(Guid MWOId);
        Task<MWO> GetMWOById(Guid MWOId);
        Task<SapAdjust> GetSapAdAjustsById(Guid SapAsjustId);
    }
}
