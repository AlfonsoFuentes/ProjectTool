namespace Application.Interfaces
{
    public interface IQueryValidationsRepository
    {
        Task<bool> ReviewIfSoftwareVersionNameExist(string name);
        Task<bool> ReviewIfSoftwareVersionNameExist(Guid Id, string name);
        Task<bool> ReviewIfBrandNameExist(string name);
        Task<bool> ReviewIfBrandNameExist(Guid Id, string name);
        Task<bool> ReviewIfSupplierNameExist(string name);
        Task<bool> ReviewIfSupplierNameExist(Guid Id, string name);
        Task<bool> ReviewSupplierEmailExist(string? email);
        Task<bool> ReviewSupplierEmailExist(Guid Id, string? email);
        Task<bool> ReviewSupplierVendorCodeExist(string vendorcode);
        Task<bool> ReviewSupplierVendorCodeExist(Guid Id, string vendorcode);

        Task<bool> ReviewIfMWONameExist(string name);
        Task<bool> ReviewIfMWONameExist(Guid Id, string name);
        Task<bool> ReviewIfMWONumberExist(Guid Id, string mwonumber);
        Task<bool> ValidatePurchaseOrderNameExist(Guid MWOId, Guid PurchaseOrderId, string name);
        Task<bool> ValidatePurchaseOrderNameExist(Guid MWOId, string name);
        Task<bool> ValidatePurchaseNumberExist(string ponumber);
        Task<bool> ValidatePurchaseNumberExist(Guid PurchaseOrderId, string ponumber);
        Task<bool> ValidatePurchaseRequisitionExist(string purchaserequisition);
        Task<bool> ValidatePurchaseRequisitionExist(Guid PurchaseOrderId, string purchaserequisition);
        Task<bool> ReviewIfBudgetItemNameExist(Guid MWOId, string name);
        Task<bool> ReviewIfBudgetItemNameExist(Guid Id, Guid MWOId, string name);
    }
}
