


namespace Application.Interfaces
{
    public interface IVersionRepository
    {
        Task<List<BudgetItem>> GetItemsToUpdateVersion1();
        Task<List<PurchaseOrderItem>> GetPurchaseOrderItemsToUpdateVersion2();
        Task<List<PurchaseOrderItemReceived>> GetPurchaseOrderItemsReceivedToUpdateVersion3();
    }
}
