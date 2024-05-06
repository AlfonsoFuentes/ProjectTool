using Azure.Core;
using Shared.NewModels.PurchaseOrders.Request;

namespace Application.Caches
{
    public static class Cache
    {
        public static string GetAllSoftwareVersion = "all-SoftwareVersion";
        public static string GetAllBrands = "all-Brands";
        public static string GetAllSuppliers = "all-Suppliers";
        public static string GetAllMWOs = "all-MWOs";
        public static string GetAllMWOsCreated = "all-MWOs-Created";
        public static string GetAllMWOsApproved = "all-MWOs-Approved";
        public static string GetMWOByCreated = "MWO-ById-Created";
        public static string GetMWOByApproved = "MWO-ById-Approved";
     
   
        public static string GetBudgetItemMWOApproved = "BudgetItem-MWO-Approved";
        public static string GetAllPurchaseOrderCreated = "all-PurchaseOrder-Created";
        public static string GetAllPurchaseOrderApproved = "all-PurchaseOrder-Approved";
        public static string GetAllPurchaseOrderClosed = "all-PurchaseOrder-Closed";
        public static string[] GetParamsCacheMWO(Guid MWOId)
        {
            List<string> parametros = new List<string>();
            parametros.Add(Cache.GetAllMWOsCreated);
            parametros.Add(Cache.GetAllMWOsApproved);
            parametros.Add($"{Cache.GetMWOByApproved}:{MWOId}");
            parametros.Add($"{Cache.GetMWOByCreated}:{MWOId}");

            return parametros.ToArray();
        }
        public static string[] GetParamsCachePurchaseOrderCreated(Guid MWOId, List<NewPurchaseOrderCreateItemRequest> PurchaseOrderItems)
        {
            List<string> parametros = new List<string>();
            foreach (var row in PurchaseOrderItems)
            {
                parametros.Add($"{Cache.GetBudgetItemMWOApproved}:{row.BudgetItemId}");
            }

            parametros.Add($"{Cache.GetAllPurchaseOrderCreated}");
            parametros.Add($"{Cache.GetAllPurchaseOrderApproved}");
            parametros.Add($"{Cache.GetAllPurchaseOrderClosed}");
            parametros.Add($"{Cache.GetMWOByApproved}:{MWOId}");
            return parametros.ToArray();
        }
        public static string[] GetParamsCachePurchaseOrderCreated(Guid MWOId)
        {
            List<string> parametros = new List<string>();
            

            parametros.Add($"{Cache.GetAllPurchaseOrderCreated}");
            parametros.Add($"{Cache.GetAllPurchaseOrderApproved}");
            parametros.Add($"{Cache.GetAllPurchaseOrderClosed}");
            parametros.Add($"{Cache.GetMWOByApproved}:{MWOId}");
            return parametros.ToArray();
        }
    }
}
