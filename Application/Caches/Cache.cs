namespace Application.Caches
{
    public static class Cache
    {
        public static string GetSapAdjust = "get-SapAdjust";
        public static string GetAllSoftwareVersion = "all-SoftwareVersion";
        public static string GetAllBrands = "all-Brands";
        public static string GetAllSuppliers = "all-Suppliers";
        public static string GetAllMWOs = "all-MWOs";
        public static string GetAllMWOsCreated = "all-MWOs-Created";
        public static string GetAllMWOsApproved = "all-MWOs-Approved";
        public static string GetMWOByCreated = "MWO-ById-Created";
        public static string GetMWOByApproved = "MWO-ById-Approved";
        public static string GetMWOPurchaseOrderById= "MWO-PurchaseOrderd-ById";
        public static string GetBudgetItemMWOApproved = "BudgetItem-MWO-Approved";
        public static string GetAllPurchaseOrderCreated = "all-PurchaseOrder-Created";
        public static string GetAllPurchaseOrderApproved = "all-PurchaseOrder-Approved";
        public static string GetAllPurchaseOrderClosed = "all-PurchaseOrder-Closed";
        public static string[] GetParamsCacheMWO(MWO mwo)
        {
            List<string> parametros = new List<string>();
            parametros.Add(Cache.GetAllMWOsCreated);
            parametros.Add(Cache.GetAllMWOsApproved);
            parametros.Add($"{Cache.GetMWOByApproved}:{mwo.Id}");
            parametros.Add($"{Cache.GetMWOByCreated}:{mwo.Id}");
            parametros.Add($"{Cache.GetMWOPurchaseOrderById}:{mwo.Id}");
            if (mwo.BudgetItems != null)
            {
                foreach (var item in mwo.BudgetItems)
                {
                    parametros.Add($"{Cache.GetBudgetItemMWOApproved}:{item.Id}");
                }
            }

            return parametros.ToArray();
        }

        public static string[] GetParamsCachePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            List<string> parametros = new List<string>();


            parametros.Add($"{Cache.GetAllPurchaseOrderCreated}");
            parametros.Add($"{Cache.GetAllPurchaseOrderApproved}");
            parametros.Add($"{Cache.GetAllPurchaseOrderClosed}");
            parametros.Add($"{Cache.GetMWOByApproved}:{purchaseOrder.MWOId}");
            parametros.Add($"{Cache.GetMWOPurchaseOrderById}:{purchaseOrder.MWOId}");
            if (purchaseOrder.PurchaseOrderItems != null)
            {
                foreach(var item in purchaseOrder.PurchaseOrderItems)
                {
                    parametros.Add($"{Cache.GetBudgetItemMWOApproved}:{item.BudgetItemId}");
                }
            }

            return parametros.ToArray();
        }
    }
}
