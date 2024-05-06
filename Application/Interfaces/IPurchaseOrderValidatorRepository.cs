
namespace Application.Interfaces
{
    public interface IPurchaseOrderValidatorRepository 
    {
        Task<bool> ValidateNameExist(Guid MWOId,string name);
        Task<bool> ValidateNameExist(Guid MWOId, Guid PurchaseOrderId, string name);
        Task<bool> ValidatePONumber(Guid PurchaseOrderId, string ponumber);
        Task<bool> ValidatePONumber(string ponumber);
        Task<bool> ValidatePurchaseRequisition(string purchaserequisition);
        Task<bool> ValidatePurchaseRequisition(Guid PurchaseOrderId, string purchaserequisition);
    }
}
