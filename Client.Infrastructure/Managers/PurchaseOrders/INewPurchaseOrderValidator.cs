namespace Client.Infrastructure.Managers.PurchaseOrders
{
    public interface INewPurchaseOrderValidator : IManager
    {
        Task<bool> ValidateNameExistInPurchaseOrder(Guid MWOId, string name);
        Task<bool> ValidatePurchaseRequisitionExistInPurchaseOrder(string Purchaserequisition);
        Task<bool> ValidateNameExistInPurchaseOrder(Guid MWOId, Guid PurchaseOrderId, string name);
        Task<bool> ValidatePurchaseRequisitionExistInPurchaseOrder(Guid PurchaseOrderId, string purchaserequisition);
        Task<bool> ValidatePONumberExistInPurchaseOrder(Guid PurchaseOrderId, string ponumber);
        Task<bool> ValidatePONumberExistInPurchaseOrder(string ponumber);
    }
    public class NewPurchaseOrderValidator : INewPurchaseOrderValidator
    {
        IHttpClientService Http;

        public NewPurchaseOrderValidator(IHttpClientService http)
        {
            Http = http;
        }

        public async Task<bool> ValidateNameExistInPurchaseOrder(Guid MWOId, string name)
        {

            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidateNameExist/{MWOId}/{name}");
            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ValidatePurchaseRequisitionExistInPurchaseOrder(string name)
        {

            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidatePurchaseRequisitionExist/{name}");
            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ValidateNameExistInPurchaseOrder(Guid MWOId, Guid PurchaseOrderId, string name)
        {

            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidateNameExist/{MWOId}/{PurchaseOrderId}/{name}");
            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ValidatePurchaseRequisitionExistInPurchaseOrder(Guid PurchaseOrderId, string purchaserequisition)
        {

            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidatePurchaseRequisitionExist/{PurchaseOrderId}/{purchaserequisition}");
            return await httpresult.ToObject<bool>();
        }
        public async Task<bool> ValidatePONumberExistInPurchaseOrder(Guid PurchaseOrderId, string ponumber)
        {

            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidatePONumberExist/{PurchaseOrderId}/{ponumber}");
            return await httpresult.ToObject<bool>();
        }

        public async Task<bool> ValidatePONumberExistInPurchaseOrder(string ponumber)
        {
            var httpresult = await Http.GetAsync($"PurchaseOrderValidator/ValidatePONumberExist/{ponumber}");
            return await httpresult.ToObject<bool>();
        }
    }
}
