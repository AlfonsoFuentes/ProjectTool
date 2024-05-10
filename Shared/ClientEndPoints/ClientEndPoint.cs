namespace Shared.ClientEndPoints
{
    public static class ClientEndPoint
    {

        public static class Actions
        {
            public static string ReOpen = "ReOpen";
            public static string Create = "Create";
            public static string CreateSalary="CreateSalary";
            public static string EditCreate = "EditCreate";
            public static string EditApproved = "EditApproved";
            public static string CreateAndReponse = "Create";
            public static string UnApprove = "UnApprove";
            public static string Approve = "Approve";
            public static string Receive = "Receive";
            public static string EditReceive = "EditReceive";
            public static string EditSalary = "EditSalary";
            public static string Update = "Update";
            public static string GetAll = "GetAll";
            public static string GetToApproved = "GetToApproved";
            public static string GetAllCreated = "GetAllCreated";
            public static string GetAllApproved = "GetAllApproved";
            public static string GetAllClosed = "GetAllClosed";
            public static string GetAllApprovedForCreatePurchaseOrder = "GetAllApprovedForCreatePurchaseOrder";
            public static string GetAllCreatedWithItems = "GetAllMWOCreatedWithItems";
            public static string GetAllApprovedWithItems = "GetAllMWOApprovedWithItems";
            public static string Delete = "Delete";
            public static string GetById = "GetById";
            public static string GetByIdToUpdate = "GetByIdUpdate";
            public static string GetByIdMWOApproved = "GetByIdMWOApproved";
            public static string GetCreateToApprove = "GetToApprove";
            public static string ExportToExcel = "ExportToExcel";
            public static string GetMWOItemsToApplyTaxes = "GetMWOItemsToApplyTaxes";
            public static string GetCreatedToEdit = "GetCreatedToEdit";
            public static string GetApprovedToReceive = "GetApprovedToReceive";
            public static string GetReceivedToEdit = "GetReceivedToEdit";
            public static string GetApprovedToEdit = "GetApprovedToEdit";
            public static string GetSalaryToEdit = "GetSalaryToEdit";
            public static string GetEBPReport = "GetEBPReport";
        }
        public static class Controller
        {
            public static string Brand = "NewBrand";
            public static string Supplier = "NewSupplier";
            public static string MWO = "NewMWO";
            public static string BudgetItem = "NewBudgetItem";
           
            public static string SoftwareVersion = "SoftwareVersion";
            public static string PurchaseOrder = "NewPurchaseOrder";
        }
        
        public static class NewBrand
        {
            public static string Create = $"{Controller.Brand}/{Actions.Create}";
            public static string CreateAndReponse = $"{Controller.Brand}/{Actions.CreateAndReponse}";
            public static string Update = $"{Controller.Brand}/{Actions.Update}";
            public static string GetAll = $"{Controller.Brand}/{Actions.GetAll}";
            public static string GetById = $"{Controller.Brand}/{Actions.GetById}";
            public static string Delete = $"{Controller.Brand}/{Actions.Delete}";
        }
        public static class NewSoftwareVersion
        {
            public static string Create = $"{Controller.SoftwareVersion}/{Actions.Create}";
            public static string GetAll = $"{Controller.SoftwareVersion}/{Actions.GetAll}";

        }
        public static class NewSupplier
        {
            public static string Create = $"{Controller.Supplier}/{Actions.Create}";
            public static string CreateAndReponse = $"{Controller.Supplier}/{Actions.CreateAndReponse}";
            public static string Update = $"{Controller.Supplier}/{Actions.Update}";
            public static string GetAll = $"{Controller.Supplier}/{Actions.GetAll}";
            public static string GetById = $"{Controller.Supplier}/{Actions.GetById}";
            public static string Delete = $"{Controller.Supplier}/{Actions.Delete}";
            public static string ExportToExcel = $"{Controller.Supplier}/{Actions.ExportToExcel}";
        }
        public static class NewMWO
        {
            public static string GetAllCreated = $"{Controller.MWO}/{Actions.GetAllCreated}";
            public static string GetAllApproved = $"{Controller.MWO}/{Actions.GetAllApproved}";
            public static string GetToApproved = $"{Controller.MWO}/{Actions.GetToApproved}";
            public static string GetEBPReport = $"{Controller.MWO}/{Actions.GetEBPReport}";


            public static string Create = $"{Controller.MWO}/{Actions.Create}";
            public static string Delete = $"{Controller.MWO}/{Actions.Delete}";
            public static string Update = $"{Controller.MWO}/{Actions.Update}";
            public static string UnApprove = $"{Controller.MWO}/{Actions.UnApprove}";
            public static string Approve = $"{Controller.MWO}/{Actions.Approve}";
        }
        public static class NewBudgetItem
        {
            public static string GetAllCreated = $"{Controller.BudgetItem}/{Actions.GetAllCreated}";
            public static string Create = $"{Controller.BudgetItem}/{Actions.Create}";
            public static string Delete = $"{Controller.BudgetItem}/{Actions.Delete}";
            public static string Update = $"{Controller.BudgetItem}/{Actions.Update}";
            public static string GetByIdToUpdate = $"{Controller.BudgetItem}/{Actions.GetByIdToUpdate}";
            public static string GetAllCreatedWithItems = $"{Controller.BudgetItem}/{Actions.GetAllCreatedWithItems}";
            public static string GetAllApprovedWithItems = $"{Controller.BudgetItem}/{Actions.GetAllApprovedWithItems}";
            public static string GetMWOItemsToApplyTaxes = $"{Controller.BudgetItem}/{Actions.GetMWOItemsToApplyTaxes}";

            public static string GetBudgetItemByIdMWOApproved = $"{Controller.BudgetItem}/{Actions.GetByIdMWOApproved}";
            public static string GetAllApprovedForCreatePurchaseOrder = $"{Controller.BudgetItem}/{Actions.GetAllApprovedForCreatePurchaseOrder}";
        }
        public static class NewPurchaseOrder
        {
            public static string Approve = $"{Controller.PurchaseOrder}/{Actions.Approve}";
            public static string Receive = $"{Controller.PurchaseOrder}/{Actions.Receive}";
            public static string ReOpen = $"{Controller.PurchaseOrder}/{Actions.ReOpen}";
            public static string UnApprove = $"{Controller.PurchaseOrder}/{Actions.UnApprove}";
            public static string Create = $"{Controller.PurchaseOrder}/{Actions.Create}";
            public static string CreateSalary = $"{Controller.PurchaseOrder}/{Actions.CreateSalary}";

            public static string Delete = $"{Controller.PurchaseOrder}/{Actions.Delete}";
            public static string GetAllCreated = $"{Controller.PurchaseOrder}/{Actions.GetAllCreated}";
            public static string GetAllApproved = $"{Controller.PurchaseOrder}/{Actions.GetAllApproved}";
            public static string GetAllClosed = $"{Controller.PurchaseOrder}/{Actions.GetAllClosed}";           
            
            
            public static string EditCreate = $"{Controller.PurchaseOrder}/{Actions.EditCreate}";
            public static string EditReceive = $"{Controller.PurchaseOrder}/{Actions.EditReceive}";
            public static string EditApproved = $"{Controller.PurchaseOrder}/{Actions.EditApproved}";
            public static string EditSalary = $"{Controller.PurchaseOrder}/{Actions.EditSalary}";

            public static string GetCreateToApprove = $"{Controller.PurchaseOrder}/{Actions.GetCreateToApprove}";
            public static string GetApprovedToReceive = $"{Controller.PurchaseOrder}/{Actions.GetApprovedToReceive}";
            public static string GetReceivedToEdit = $"{Controller.PurchaseOrder}/{Actions.GetReceivedToEdit}";
            public static string GetApprovedToEdit = $"{Controller.PurchaseOrder}/{Actions.GetApprovedToEdit}";
            public static string GetSalaryToEdit = $"{Controller.PurchaseOrder}/{Actions.GetSalaryToEdit}";
            public static string GetCreateToEdit = $"{Controller.PurchaseOrder}/{Actions.GetCreatedToEdit}";
        }
    }
}
