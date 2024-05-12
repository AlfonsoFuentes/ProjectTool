namespace Application.Messages
{
    public static class ResponseMessages
    {
        public static string ReponseSuccesfullyMessage(string rowName,string responsetype, string tablename) => 
            $"{rowName} was {responsetype} succesfully in table: {tablename}";
        public static string ReponseFailMessage(string rowName, string responsetype, string tablename) => 
            $"{rowName} was not {responsetype} succesfully in table: {tablename}";
       
    }
    public static class ResponseType
    {
        public static string Created = "created";
        public static string Updated = "updated";
        public static string Delete = "delete";
        public static string NotFound = "found";
        public static string UnApprove = "un approved";
        public static string Approve = "approved";
        public static string Reopen = "re opened";
        public static string Received = "received";
    }
    public static class ClassNames
    {
        public static string SoftwareVersion = "SoftwareVersion";
        public static string Brand = "Brand";
        public static string Supplier = "Supplier";
        public static string MWO = "MWO";
        public static string BudgetItems = "Budget Items";
        public static string PurchaseOrders = "Purchase Orders";
        public static string SapAdjust = "SAP Conciliation";
    }
}
