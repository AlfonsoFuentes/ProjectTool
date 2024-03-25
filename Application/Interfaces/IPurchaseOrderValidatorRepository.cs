
namespace Application.Interfaces
{
    public interface IPurchaseOrderValidatorRepository : IRepository
    {
        Task<bool> ValidateNameExist(string name);
        Task<bool> ValidateNameExist(Guid PurchaseOrderId, string name);
        Task<bool> ValidatePONumber(Guid PurchaseOrderId, string ponumber);
        Task<bool> ValidatePurchaseRequisition(string purchaserequisition);
        Task<bool> ValidatePurchaseRequisition(Guid PurchaseOrderId, string purchaserequisition);
    }
}
